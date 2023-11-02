using System.Diagnostics.CodeAnalysis;
using ConsoleTables;

namespace SimpleGitShell.Logging;

public static class LoggerExtensions
{
    public static void Debug([NotNull] this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.DEBUG);
    }

    public static void Info([NotNull] this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.INFO);
    }

    public static void Warn([NotNull] this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.WARN);
    }

    public static void Error([NotNull] this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.ERROR);
    }

    public static void Table([NotNull] this ILogger logger, IEnumerable<string> headers, [NotNull] IEnumerable<string[]> rows, LogLevel level = LogLevel.INFO)
    {
        var writer = new StringWriter();
        var table = new ConsoleTable(new ConsoleTableOptions
        {
            OutputTo = writer,
            Columns = headers
        });
        foreach (var row in rows)
        {
            table.AddRow(row);
        }

        table.Write();
        logger.Log(writer.ToString(), level);
    }
}
