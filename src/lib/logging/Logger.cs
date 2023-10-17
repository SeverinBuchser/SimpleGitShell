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

    private static readonly int _levelWidth = 7;

    private Logger() {}

    public void Log(string message, LogLevel level)
    {
        var completeMessage = "";
        var levelString = $"[{ Enum.GetName(typeof(LogLevel), level)! }]".PadRight(_levelWidth);
        var lines = message.Split("\n");

        if (lines.Length > 2) 
        {
            completeMessage += $"{ levelString }\n";
        } else {
            completeMessage += $"{ levelString }";
        }
        completeMessage += message;
        Write(completeMessage);
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