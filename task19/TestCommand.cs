using System;
using System.Collections.Generic;

namespace Task19
{
    public class TestCommand : ILongRunningCommand
    {
        private readonly int _id;
        private int _remaining;
        private int _count;
        private readonly List<(DateTime ts, int id)>? _log;

        public bool IsCompleted => _remaining <= 0;

        public TestCommand(int id, int iterations, List<(DateTime, int)>? log = null)
        {
            _id = id;
            _remaining = iterations;
            _log = log;
        }

        public void Execute()
        {
            _count++;
            Console.WriteLine($"Поток {_id} вызов {_count}");
            _remaining--;
            _log?.Add((DateTime.UtcNow, _id));
        }
    }
}
