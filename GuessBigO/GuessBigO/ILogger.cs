
namespace GuessBigO;

public interface ILogger
{
    void WriteLine(string message);
}

public sealed class ConsoleLogger : ILogger
{
    public void WriteLine(string message)
        => Console.WriteLine(message);
}

public sealed class EmptyLogger : ILogger
{
    public void WriteLine(string message) { }
}