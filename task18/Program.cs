using System;
using System.Collections.Generic;

namespace Task18
{
    class Program
    {
        static void Main()
        {
            var log = new List<string>();
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);

            server.Enqueue(new DemoLongCommand("A", 3, log));
            server.Enqueue(new DemoLongCommand("B", 2, log));
            server.RequestSoftStop();
            server.WaitForCompletion();

            Console.WriteLine(string.Join(", ", log));
        }
    }

    class DemoLongCommand : ILongRunningCommand
    {
        private readonly string _id;
        private int _steps;
        private readonly List<string> _log;
        public bool IsCompleted => _steps <= 0;

        public DemoLongCommand(string id, int steps, List<string> log)
        {
            _id = id;
            _steps = steps;
            _log = log;
        }

        public void Execute()
        {
            if (_steps-- > 0)
                lock (_log)
                    _log.Add(_id);
        }
    }
}
