using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Task19;
using Xunit;

namespace task19tests
{
    public class ServerThreadTests
    {
        [Fact]
        public void ExecutesFiveCommandsThreeTimesThenStops()
        {
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);
            var sw = new StringWriter();
            Console.SetOut(sw);
            for (int i = 1; i <= 5; i++)
                server.Enqueue(new TestCommand(i, 3, new List<(DateTime, int)>()));
            server.Enqueue(new HardStopCommand(server));
            server.Wait();
            var lines = sw.ToString()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            Assert.Equal(5 * 3, lines.Length);
            var counts = new Dictionary<int, int>();
            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var id = int.Parse(parts[1]);
                counts[id] = counts.GetValueOrDefault(id) + 1;
            }
            foreach (var id in Enumerable.Range(1, 5))
                Assert.Equal(3, counts[id]);
        }

        [Fact]
        public void ImmediateHardStopWithoutCommands_ExitsWithoutOutput()
        {
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);
            var sw = new StringWriter();
            Console.SetOut(sw);
            server.Enqueue(new HardStopCommand(server));
            server.Wait();
            var output = sw.ToString().Trim();
            Assert.True(string.IsNullOrEmpty(output));
        }

        [Fact]
        public void StopCalledDirectly_NoCommands_NoException()
        {
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);
            server.Stop();
            Exception ex = Record.Exception(() => server.Wait());
            Assert.Null(ex);
        }

        [Fact]
        public void FairScheduling_MultiCommandsAlternate()
        {
            var log = new List<int>();
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);
            server.Enqueue(new CustomCommand(1, 2, log));
            server.Enqueue(new CustomCommand(2, 2, log));
            server.Enqueue(new HardStopCommand(server));
            server.Wait();
            Assert.Equal(new[] { 1, 2, 1, 2 }, log);
        }

        class CustomCommand : ILongRunningCommand
        {
            private readonly int _id;
            private int _remaining;
            private readonly List<int> _log;
            public bool IsCompleted => _remaining <= 0;
            public CustomCommand(int id, int steps, List<int> log)
            {
                _id = id;
                _remaining = steps;
                _log = log;
            }
            public void Execute()
            {
                if (_remaining-- > 0)
                    _log.Add(_id);
            }
        }
    }
}
