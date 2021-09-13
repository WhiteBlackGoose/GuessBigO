
namespace GuessBigO;

public abstract class Target
{
    public virtual int MinValue => 3;
    public virtual int MaxValue => 10_000;

    public abstract int N { set; get; }

    public abstract void Setup();

    public abstract void Run();
}