using System;
using System.Diagnostics;
using System.IO;
using practice2025.Task15;

namespace practice2025.Task15
{
    public static class PerformanceRunner
    {
        public static void Run(
            double from,
            double to,
            Func<double, double> function,
            double[] steps,
            int maxThreads,
            int repeats = 5,
            string outCsv = "performance.csv")
        {
            using var writer = new StreamWriter(outCsv);
            writer.WriteLine("Step,Threads,AvgTimeMs");

            foreach (var step in steps)
            {
                double singleMs = MeasureAvgTime(
                    () => DefiniteIntegral.SolveSingleThread(from, to, function, step),
                    repeats);
                Console.WriteLine($"[Single] Step={step:E} => {singleMs:F2}ms");

                for (int t = 1; t <= maxThreads; t++)
                {
                    double multiMs = MeasureAvgTime(
                        () => DefiniteIntegral.Solve(from, to, function, step, t),
                        repeats);
                    Console.WriteLine($"[Multi ] Step={step:E}, Threads={t} => {multiMs:F2}ms");
                    writer.WriteLine($"{step},{t},{multiMs:F2}");
                }
            }

            Console.WriteLine($"\nРезультаты сохранены в {outCsv}");
        }

        private static double MeasureAvgTime(Action action, int repeats)
        {
            var sw = new Stopwatch();
            double total = 0;
            for (int i = 0; i < repeats; i++)
            {
                sw.Restart();
                action();
                sw.Stop();
                total += sw.Elapsed.TotalMilliseconds;
            }
            return total / repeats;
        }
    }
}
