using System;
using System.Globalization;
using System.IO;
using System.Linq;
using ScottPlot;

namespace practice2025.Task15
{
    public static class PlotBuilder
    {
        public static void BuildPlot(string csvPath, string imageOut, double targetStep = 1e-4)
        {
            var data = File.ReadAllLines(csvPath)
                .Skip(1)
                .Select(line => line.Split(','))
                .Select(parts => new
                {
                    Step    = double.Parse(parts[0], CultureInfo.InvariantCulture),
                    Threads = int.Parse(parts[1], CultureInfo.InvariantCulture),
                    Time    = double.Parse(parts[2], CultureInfo.InvariantCulture)
                })
                .Where(d => Math.Abs(d.Step - targetStep) < 1e-9)
                .OrderBy(d => d.Threads)
                .ToList();

            double[] xs = data.Select(d => (double)d.Threads).ToArray();
            double[] ys = data.Select(d => d.Time).ToArray();

            var plt = new Plot();
            plt.Add.Scatter(xs, ys);
            plt.Title($"Step = {targetStep:G}");
            plt.XLabel("Количество потоков");
            plt.YLabel("Время, мс");

            plt.SavePng(imageOut, 600, 400);
            Console.WriteLine($"График сохранён: {imageOut}");
        }
    }
}
