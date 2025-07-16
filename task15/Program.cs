using System;
using practice2025.Task15;

namespace practice2025.Task15
{
    class Program
    {
        static void Main()
        {
            double[] steps = { 1e-1, 1e-2, 1e-3, 1e-4, 1e-5, 1e-6 };
            int maxThreads = Environment.ProcessorCount;

            PerformanceRunner.Run(
                from: -100,
                to: 100,
                function: Math.Sin,
                steps: steps,
                maxThreads: maxThreads);

            PlotBuilder.BuildPlot(
                csvPath: "performance.csv",
                imageOut: "performance.png",
                targetStep: 1e-4);
        }
    }
}
