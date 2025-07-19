using System;
using System.Collections.Generic;
using Task18;
using Xunit;

namespace task18tests
{
    class StepCommand : ILongRunningCommand
    {
        public readonly int Id;
        public int Remaining;
        public static readonly object Lock = new();
        public static List<int> Log = new();

        public bool IsCompleted => Remaining <= 0;

        public StepCommand(int id, int steps)
        {
            Id = id;
            Remaining = steps;
        }

        public void Execute()
        {
            if (Remaining-- > 0)
                lock (Lock)
                    Log.Add(Id);
        }
    }

    public class ServerThreadTests
    {
        [Fact]
        public void ExecutesLongCommandsInterleaved()
        {
            StepCommand.Log = new List<int>();
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);

            server.Enqueue(new StepCommand(1, 2));
            server.Enqueue(new StepCommand(2, 2));
            server.Enqueue(new SoftStopCommand(server));
            server.WaitForCompletion();

            Assert.Equal(4, StepCommand.Log.Count);
            Assert.Equal(2, StepCommand.Log.FindAll(x => x == 1).Count);
            Assert.Equal(2, StepCommand.Log.FindAll(x => x == 2).Count);
        }
    }
}
