using Microsoft.Extensions.Logging;

namespace Server.GitShell.Logging;

public class CommandLogger : ILogger
{

    private static CommandLogger? _Default = null;

    public static CommandLogger Default
    {
        get
        {
            _Default ??= new CommandLogger();
            return _Default;
        }
        set { _Default = value; }
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        throw new NotImplementedException();
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        Console.Write($"{logLevel}: ");
        Console.WriteLine(state);
    }
}