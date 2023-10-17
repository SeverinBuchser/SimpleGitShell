using ConsoleTables;

namespace Server.GitShell.Lib.Logging;

public static class LoggerExtensions
{
    public static void Debug(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.DEBUG);
    }
    
    public static void Info(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.INFO);
    }
    
    public static void Warn(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.WARN);
    }
    
    public static void Error(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.ERROR);
    }

    public static void Table(this ILogger logger, string[] headers, IEnumerable<string[]> rows, LogLevel level = LogLevel.INFO)
    {
        var writer = new StringWriter();
        var table = new ConsoleTable(new ConsoleTableOptions {
            OutputTo = writer,
            Columns = headers
        });
        foreach (var row in rows) table.AddRow(row);
        table.Write(Format.Alternative);
        logger.Log(writer.ToString(), level);
    }
}