using Spectre.Console;

namespace Server.GitShell.Lib.Logging;

public class Logger : ILogger
{
    private static List<TextWriter> _writers = new() {
        Console.Out
    };
    private static readonly Logger _instance = new();

    public static Logger Instance
    {
        get => _instance;
    }

    static Logger() {
    }

    private static readonly int _prefixWidth = 28;

    private Logger() {}

    public void Log(string message, LogLevel level)
    {
        var levelString = Enum.GetName(typeof(LogLevel), level);
        var timeString = string.Format("{0:dd/mm/yyyy HH:mm:ss}", DateTime.Now);
        var prefix = $"[{ timeString } { levelString }]".PadRight(_prefixWidth);
        var lines = message.Split("\n").Select(line => prefix + line);
        var outMessage = string.Join("\n", lines);
        if (!outMessage.EndsWith("\n")) outMessage += "\n";
        Write(outMessage);
    }

    private void Write(string message)
    {
        foreach (var writer in _writers)
        {
            writer.Write(message);
        }
    }

    public static void SetOut(TextWriter writer) {
        _writers = new List<TextWriter>
        {
            writer
        };
    }

    public static void AddOut(TextWriter writer) {
        _writers.Add(writer);
    }
}