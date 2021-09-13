using GuessBigO;

var runner = new BigORunner<BubbleSort>(new ConsoleLogger());
runner.Approximate(iterPerStep: 250, steps: 10);

public sealed class BubbleSort : Target
{
    public override int MaxValue => 3_00;
    public override int MinValue => 50;

    public override int N { set; get; }

    private int[] arr;
    private int[] dest;

    public override void Setup()
    {
        var rand = new Random();
        arr = Enumerable.Range(0, N).OrderBy(c => rand.Next(0, N)).ToArray();
        dest = new int[N];
    }

    public override void Run()
    {
        Array.Copy(arr, dest, N);
        for (int i = 0; i < N; i++)
            for (int j = 0; j < i; j++)
                if (arr[i] > arr[j])
                    (arr[i], arr[j]) = (arr[j], arr[i]);
    }
}
