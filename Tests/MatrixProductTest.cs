namespace Tests;

public class MatrixProductTset
{
    [Fact]
    public void Test()
    {
        var runner = new BigORunner<MatrixProduct>();
        var (expr, _) = runner.Approximate(iterPerStep: 2500, steps: 10);
        Assert.Equal("n ^ 3", expr);
    }
}

public sealed class MatrixProduct : Target
{
    public override int MaxValue => 50;
    public override int MinValue => 3;

    public override int N { set; get; }
    public int[,] A;
    public int[,] B;
    public int[,] C;

    public override void Setup()
    {
        A = new int[N, N];
        B = new int[N, N];
        C = new int[N, N];
    }

    public override void Run()
    {
        for (int x = 0; x < N; x++)
        {
            for (int y = 0; y < N; y++)
            {
                var s = 0;
                for (int i = 0; i < N; i++)
                    s += A[x, i] * B[i, y];
                C[x, y] = s;
            }
        }
    }
}