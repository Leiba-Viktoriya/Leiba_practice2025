using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace practice2025.Task15
{
    public static class PerformanceRunner
    {
        public static void Run(
            double from,
            double to,
            Func<double, double> function,
            double[] steps,
            int maxThreads)
        {
            var csvPath = "performance.csv";
            using var writer = new StreamWriter(csvPath, false, System.Text.Encoding.UTF8);
            writer.WriteLine("Step,Threads,AvgTimeMs");

            foreach (double step in steps)
            {
                double singleMs = Measure(() =>
                    DefiniteIntegral.Solve(from, to, function, step, 1));
                writer.WriteLine(
                    $"{step.ToString("G", CultureInfo.InvariantCulture)},1," +
                    $"{singleMs.ToString("F2", CultureInfo.InvariantCulture)}");

                for (int threads = 1; threads <= Math.Min(maxThreads, 3); threads++)
                {
                    double multiMs = Measure(() =>
                        DefiniteIntegral.Solve(from, to, function, step, threads));
                    writer.WriteLine(
                        $"{step.ToString("G", CultureInfo.InvariantCulture)}," +
                        $"{threads}," +
                        $"{multiMs.ToString("F2", CultureInfo.InvariantCulture)}");
                }
            }
        }

        static double Measure(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();
            return sw.Elapsed.TotalMilliseconds;
        }
    }
}
