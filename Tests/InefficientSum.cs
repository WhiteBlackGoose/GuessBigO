namespace Tests;

public class InefficientSumTest
{
    [Fact]
    public void Test()
    {
        var runner = new BigORunner<InefficientSum>();
        var (expr, _) = runner.Approximate(iterPerStep: 250, steps: 10);
        Assert.Equal("2 ^ n", expr);
    }
}

public sealed class InefficientSum : Target
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
        R = Ohno(N);
        int Ohno(int a)
            => a switch
            {
                0 => 0,
                1 => 1,
                var n => Ohno(n - 1) + Ohno(n - 1)
            };
    }
}
