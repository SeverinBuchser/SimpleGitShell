namespace Tests.Server.GitShell.Utils;

public class TestConsole : IDisposable
{
    protected StringWriter _OutWriter = new();
    protected StringWriter _ErrWriter = new();

    public StringWriter OutWriter { 
        get => _OutWriter;
    }
    public StringWriter ErrWriter { 
        get => _OutWriter;
    }

    public TestConsole()
    {
        if (Environment.GetEnvironmentVariable("VERBOSE") != "true") 
        {
            _DisableConsole();
        }
    }

    public virtual void Dispose()
    {
        _EnableConsole();
    }

    private void _DisableConsole() 
    {
        SetOut(OutWriter);
        SetError(ErrWriter);
    }

    private void _EnableConsole()
    {
        var stdOut = new StreamWriter(Console.OpenStandardOutput())
        {
            AutoFlush = true
        };
        SetOut(stdOut);

        var stdError = new StreamWriter(Console.OpenStandardError())
        {
            AutoFlush = true
        };
        SetError(stdError);
    }

    public void WriteDebug(string message)
    {
        _EnableConsole();
        Console.Write(message);
        _DisableConsole();
    }

    public void WriteLineDebug(string message)
    {
        _EnableConsole();
        Console.WriteLine(message);
        _DisableConsole();
    }

    public static void SetOut(TextWriter textWriter)
    {
        Console.SetOut(textWriter);
    }

    public static void SetError(TextWriter textWriter)
    {
        Console.SetError(textWriter);
    }
} 