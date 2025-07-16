using System;
using System.Collections.Concurrent;
using System.Threading;
using Task17;
using Xunit;

namespace task17tests
{
    public class ServerThreadTests
    {
        class CounterCommand : ICommand
        {
            public int Id { get; }
            public static ConcurrentBag<int> ExecutedIds = new();
            public CounterCommand(int id) => Id = id;
            public void Execute() => ExecutedIds.Add(Id);
        }

        [Fact]
        public void SoftStop_ProcessesAllCommands_ThenStops()
        {
            CounterCommand.ExecutedIds = new ConcurrentBag<int>();
            using var server = new ServerThread();
            for (int i = 0; i < 5; i++)
                server.Enqueue(new CounterCommand(i));
            server.Enqueue(new SoftStopCommand(server));
            server.WaitForCompletion();
            Assert.Equal(5, CounterCommand.ExecutedIds.Count);
            for (int i = 0; i < 5; i++)
                Assert.Contains(i, CounterCommand.ExecutedIds);
        }

        [Fact]
        public void HardStop_StopsImmediately_LeavesRemainingCommands()
        {
            CounterCommand.ExecutedIds = new ConcurrentBag<int>();
            using var server = new ServerThread();
            var firstDone = new ManualResetEventSlim();
            server.Enqueue(new ActionCommand(() =>
            {
                CounterCommand.ExecutedIds.Add(0);
                firstDone.Set();
            }));
            server.Enqueue(new HardStopCommand(server));
            server.Enqueue(new CounterCommand(2));
            firstDone.Wait();
            server.WaitForCompletion();
            Assert.Single(CounterCommand.ExecutedIds);
            Assert.Contains(0, CounterCommand.ExecutedIds);
            Assert.DoesNotContain(2, CounterCommand.ExecutedIds);
        }

        [Fact]
        public void StopCommand_CalledFromWrongThread_Throws()
        {
            using var server = new ServerThread();
            var hard = new HardStopCommand(server);
            var soft = new SoftStopCommand(server);
            Assert.Throws<InvalidOperationException>(() => hard.Execute());
            Assert.Throws<InvalidOperationException>(() => soft.Execute());
            server.RequestHardStop();
        }

        class ActionCommand : ICommand
        {
            private readonly Action _action;
            public ActionCommand(Action action) => _action = action;
            public void Execute() => _action();
        }
    }
}
