using System.Globalization;
using System.IO;
using System.Linq;
using ScottPlot;
using System;

namespace practice2025.Task15
{
    public static class PlotBuilder
    {
        public static void BuildPlot(string csvPath, string imageOut, double targetStep = 1e-4)
        {
            var lines = File.ReadAllLines(csvPath)
                            .Skip(1)
                            .Where(line => line.StartsWith($"{targetStep}"))
                            .Select(line => line.Split(','))
                            .ToList();

            var threads = lines.Select(parts => double.Parse(parts[1], CultureInfo.InvariantCulture)).ToArray();
            var times = lines.Select(parts => double.Parse(parts[2], CultureInfo.InvariantCulture)).ToArray();

            var plt = new Plot();
            plt.Add.Scatter(threads, times);

            plt.Title("Время выполнения Solve от количества потоков");
            plt.XLabel("Потоки");
            plt.YLabel("Время (мс)");

            plt.SavePng(imageOut, 600, 400);
            Console.WriteLine($"График сохранён: {imageOut}");
        }
    }
}
