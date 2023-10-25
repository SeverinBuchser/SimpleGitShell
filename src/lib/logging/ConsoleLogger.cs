using Server.GitShell.Lib.Logging;

public class ConsoleLogger : ILogger
{
    public void Log(string message, LogLevel level)
    {
        if (!message.EndsWith("\n")) message += "\n";
        Console.Write(message);    
    }
}