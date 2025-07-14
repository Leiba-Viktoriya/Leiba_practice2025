using System;
using System.Threading;

namespace practice2025.Task14
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
                int index = i;
                new Thread(() =>
                {
                    double start = a + segmentLength * index;
                    double end = (index == threadsNumber - 1)
                        ? b
                        : start + segmentLength;

                    double localSum = 0.0;
                    for (double x = start; x < end; x += step)
                    {
                        double next = Math.Min(x + step, end);
                        localSum += (function(x) + function(next)) * (next - x) / 2.0;
                    }

                    double initial, computed;
                    do
                    {
                        initial = total;
                        computed = initial + localSum;
                    }
                    while (Interlocked.CompareExchange(ref total, computed, initial) != initial);

                    barrier.SignalAndWait();
                })
                { IsBackground = true }
                .Start();
            }

            barrier.SignalAndWait();
            return total;
        }
    }
}
