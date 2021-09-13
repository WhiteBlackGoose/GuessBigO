## Guess big O

So for now that's a fun thing which takes your algorithm as a black box and tries to guess its time complexity.

### Bubble sort example

Let's write a bubble sort class:

```cs
public sealed class BubbleSort : Target
{
    public override int MaxValue => 300;
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
```

Now, pass it to the thing:

```cs
var runner = new BigORunner<BubbleSort>();
var (expr, _) = runner.Approximate(iterPerStep: 250, steps: 10);
```

and run. `expr` will be `n ^ 2`.

### Matrix multiplication

Let's write the corresponding class:

```cs
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
```

and the entry point:
```cs
var runner = new BigORunner<MatrixProduct>();
var (expr, _) = runner.Approximate(iterPerStep: 2500, steps: 10);
```

`iterPerStep` is high to improve the precision. Anyway, run and get `n ^ 3`.