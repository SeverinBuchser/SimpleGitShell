namespace Server.GitShell.Lib.Logging;

public interface ILogger {
    public void Log(string message, LogLevel level);
}

