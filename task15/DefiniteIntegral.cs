using System;
using System.Threading;

namespace practice2025.Task15
{
    public static class DefiniteIntegral
    {
        public static double Solve(
            double a,
            double b,
            Func<double, double> function,
            double step,
            int threadsNumber)
        {
            if (threadsNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(threadsNumber));

            double total = 0.0;
            var barrier = new Barrier(threadsNumber + 1);
            double segmentLength = (b - a) / threadsNumber;

            for (int i = 0; i < threadsNumber; i++)
            {
                int idx = i;
                new Thread(() =>
                {
                    double start = a + segmentLength * idx;
                    double end = (idx == threadsNumber - 1) ? b : start + segmentLength;

                    double localSum = 0.0;
                    for (double x = start; x < end; x += step)
                    {
                        double next = Math.Min(x + step, end);
                        localSum += (function(x) + function(next)) * (next - x) / 2.0;
                    }

                    double prev, updated;
                    do
                    {
                        prev = total;
                        updated = prev + localSum;
                    }
                    while (Interlocked.CompareExchange(ref total, updated, prev) != prev);

                    barrier.SignalAndWait();
                })
                { IsBackground = true }
                .Start();
            }

            barrier.SignalAndWait();
            return total;
        }

        public static double SolveSingleThread(
            double a,
            double b,
            Func<double, double> function,
            double step)
        {
            double sum = 0.0;
            for (double x = a; x < b; x += step)
            {
                double next = Math.Min(x + step, b);
                sum += (function(x) + function(next)) * (next - x) / 2.0;
            }
            return sum;
        }
    }
}
