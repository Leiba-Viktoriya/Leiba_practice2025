using System;
using System.Collections.Generic;
using System.IO;

namespace Task19
{
    class Program
    {
        static void Main()
        {
            var scheduler = new RoundRobinScheduler();
            using var server = new ServerThread(scheduler);

            var timeline = new List<(DateTime ts, int id)>();
            for (int i = 1; i <= 5; i++)
                server.Enqueue(new TestCommand(i, 3, timeline));

            server.Enqueue(new HardStopCommand(server));
            server.Wait();

            File.WriteAllLines("timeline.csv",
                timeline.ConvertAll(x => $"{x.ts:O},{x.id}"));
        }
    }
}
