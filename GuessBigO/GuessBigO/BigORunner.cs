
using AngouriMath;
using System.Diagnostics;
using System.Linq;

namespace GuessBigO;

public sealed class BigORunner<T> where T : Target, new()
{
    private readonly ILogger logger;
    public BigORunner(ILogger? logger = null)
        => this.logger = logger ?? new EmptyLogger();

    private readonly static (Entity Expression, Func<double, double> Compiled)[] complexities = 
        new Entity[]
        {
            "1",
            "sqrt(n)",
            "n",
            "n^2",
            "n^3",
            "e^n",
            "2^n"
        }
        .Select(c => (c, c.Compile<double, double>("n")))
        .ToArray();

    public (Entity answer, double error) Approximate(int steps = 100, int iterPerStep = 100)
    {
        var (times, Ns) = GetTimes(steps, iterPerStep);
        ((var expr, _), var error) =
            complexities.Select(
                c =>
                    (
                        c,
                        GetDeviation(
                            Ns
                            .Select(n => c.Compiled(n))
                            .Zip(times)
                            .Select(pair => pair.First / pair.Second)
                            .ToArray()
                        )
                    )
                ).MinBy(c => c.Item2);

        logger.WriteLine($"Best error: {error}. The found approximation is O({expr})");

        return (expr, error);
    }

    private double GetDeviation(double[] ratios, double percentile = 0.8)
    {
        var toExcl = (int)(ratios.Length * ((1 - percentile) / 2));
        var sumPer = ratios[toExcl..^toExcl].Sum();
        var meanPer = sumPer / (ratios.Length - toExcl * 2);
        var median = ratios.OrderBy(c => c).ToArray()[ratios.Length / 2];
        var diff = Math.Max(meanPer / median, median / meanPer);
        return diff;
    }

    private (double[] times, int[] Ns) GetTimes(int steps, int iterPerStep)
    {
        var times = new double[steps];
        var Ns = new int[steps];
        for (int i = 0; i < steps; i++)
        {
            var instance = new T();
            var n = (instance.MaxValue * i + instance.MinValue * (steps - i)) / steps;
            instance.N = n;

            logger.WriteLine($"Running N = {n}, iterations = {iterPerStep}");

            instance.Setup();
            var time = GetAverage(instance, iterPerStep);

            logger.WriteLine($"Average time for it was {time} ms");

            times[i] = time;
            Ns[i] = n;
        }
        return (times, Ns);
    }

    private double GetAverage(T target, int iterCount)
    {
        var sw = new Stopwatch();
        GC.Collect(2, GCCollectionMode.Forced, true, true);
        GC.WaitForPendingFinalizers();
        GC.Collect();
        sw.Start();
        for (int i = 0; i < iterCount; i++)
            target.Run();
        sw.Stop();
        return (double)sw.ElapsedMilliseconds / iterCount;
    }
}
