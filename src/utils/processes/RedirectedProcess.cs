using System.Diagnostics;
using SimpleGitShellrary.Logging;

namespace SimpleGitShellrary.Utils.Processes;

public class RedirectedProcess : Process
{
    private readonly string[] _inputs;

    public RedirectedProcess(string fileName, string args) : this(fileName, args, Array.Empty<string>()) { }
    public RedirectedProcess(string fileName, string args, params string[] inputs)
    {
        _inputs = inputs;
        StartInfo = new ProcessStartInfo
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

    public new int Start()
    {
        if (base.Start())
        {
            foreach (var line in _inputs)
            {
                StandardInput.WriteLine(line);
            }

            WaitForExit();

            return ExitCode;
        }
        return 1;
    }
}
