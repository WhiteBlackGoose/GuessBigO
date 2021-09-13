using GuessBigO;

var runner = new BigORunner<Fibonacci>(new ConsoleLogger());
runner.Approximate(iterPerStep: 250, steps: 20);

public sealed class Fibonacci : Target
{
    public override int MaxValue => 20;
    public override int MinValue => 2;

    public override int N { set; get; }

    public int R;

    public override void Setup()
    {

    }

    public override void Run()
    {
        R = Fib(N);
        int Fib(int a)
            => a switch
            {
                0 => 0,
                1 => 1,
                var n => Fib(n - 1) + Fib(n - 1)
            };
    }
}
