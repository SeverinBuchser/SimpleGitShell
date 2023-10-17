namespace Server.GitShell.Lib.Logging;

public class Logger : ILogger
{
    private static TextWriter _writer = Console.Out;
    private static readonly Logger _instance = new();

    public static Logger Instance
    {
        get => _instance;
    }

    static Logger() {}

    private static readonly int _levelWidth = 7;

    private Logger() {}

    public void Log(string message, LogLevel level)
    {
        var levelString = $"[{ Enum.GetName(typeof(LogLevel), level)! }]".PadRight(_levelWidth);
        var lines = message.Split("\n");

        if (lines.Length > 2) 
        {
            _writer.Write($"{ levelString }\n");
        }
        _writer.Write(message);
    }

    public static void SetOut(TextWriter writer) {
        _writer = writer;
    }
}