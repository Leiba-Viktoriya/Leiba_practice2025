using System;
using System.Collections.Generic;
using Task18;
using Xunit;

namespace task18tests
{
    class DummyCommand : ICommand
    {
        public readonly int Id;
        public DummyCommand(int id) => Id = id;
        public void Execute() { }
    }

    public class SchedulerTests
    {
        [Fact]
        public void RoundRobin_SelectsInCycle()
        {
            var sch = new RoundRobinScheduler();
            sch.Add(new DummyCommand(1));
            sch.Add(new DummyCommand(2));
            sch.Add(new DummyCommand(3));

            var order = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                var cmd = (DummyCommand)sch.Select();
                order.Add(cmd.Id);
                sch.Add(cmd);
            }

            Assert.Equal(new[] { 1, 2, 3, 1, 2 }, order);
        }

        [Fact]
        public void HasCommandReflectsEmpty()
        {
            var sch = new RoundRobinScheduler();
            Assert.False(sch.HasCommand());
            sch.Add(new DummyCommand(1));
            Assert.True(sch.HasCommand());
            sch.Select();
            Assert.False(sch.HasCommand());
        }
    }
}
