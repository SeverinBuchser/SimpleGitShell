namespace Tests.Server.GitShell.Utils;

public class DisableConsole : IDisposable
{
    public DisableConsole()
    {
        Console.SetOut(TextWriter.Null);
        Console.SetError(TextWriter.Null);
        Console.SetIn(TextReader.Null);
    }

    public virtual void Dispose()
    {
        var stdOut = new StreamWriter(Console.OpenStandardOutput())
        {
            AutoFlush = true
        };
        Console.SetOut(stdOut);

        var stdErr = new StreamWriter(Console.OpenStandardError())
        {
            AutoFlush = true
        };
        Console.SetError(stdErr);

        Console.SetIn(new StreamReader(Console.OpenStandardInput()));
    }
} 