using SimpleGitShell.Lib.Logging;

namespace SimpleGitShell.Lib.Utils.Processes;

public class RedirectedProcess : LoggerProcess
{
    private string[] _inputs;

    public RedirectedProcess(string fileName) : this(fileName, "") {}
    public RedirectedProcess(string fileName, string args) : this(fileName, args, Array.Empty<string>()) {}
    public RedirectedProcess(string fileName, string args, params string[] inputs) 
    {
        _inputs = inputs;
        StartInfo = new System.Diagnostics.ProcessStartInfo 
        {
            FileName = fileName, 
            Arguments = args,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }

    public new int Start() {
        if (base.Start())
        {
            foreach (var line in _inputs) StandardInput.WriteLine(line);

            WaitForExit();
        
            foreach (var logger in _loggers) logger.Info(StandardOutput.ReadToEnd());
            foreach (var logger in _loggers) logger.Error(StandardError.ReadToEnd());

            return ExitCode;
        }
        return 1;
    }
}
