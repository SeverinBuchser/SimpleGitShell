using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Server.GitShell.Lib.Logging;

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

    public CommandLogger() : base()
    {
        
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
        var style = "[bold ";
        switch (logLevel)
        {
            case LogLevel.Information:
                style += "blue";
                break;
            case LogLevel.Warning:
                style += "yellow";
                break;
            case LogLevel.Critical:
                style += "maroon";
                break;
            case LogLevel.Error:
                style += "red";
                break;
        }
        style += "]";

        var prefix = new Markup($"{style}{$"[{logLevel}]".EscapeMarkup()}[/]");
        var message = new Markup($"{state}");
        var columns = new Columns(prefix, message);
        var rows = new Rows(columns);

        AnsiConsole.Write(rows);
    }
}