using Server.GitShell.Lib.Logging;

namespace Server.GitShell.Lib.Utils.Processes;

public class Process : System.Diagnostics.Process
{
    private bool _attached = false;
    private string[] _inputs;

    public Process(string fileName) : this(fileName, "") {}
    public Process(string fileName, string args) : this(fileName, args, Array.Empty<string>()) {}
    public Process(string fileName, string args, params string[] inputs) 
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

    public void Attach() 
    {
        OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler((sender, e) => 
        {
            if (e.Data != null)
            {
                Logger.Instance.Info(e.Data);
            }
        });
        
        ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler((sender, e) => 
        {
            if (e.Data != null)
            {
                Logger.Instance.Error(e.Data);
            }
        });
        _attached = true;
    }

    public new bool Start() 
    {
        var started = base.Start();
        if (_attached) 
        {
            BeginOutputReadLine();
            BeginErrorReadLine();
        }
        return started;
    }

    public int StartSync() {
        Start();
        foreach (var line in _inputs) StandardInput.WriteLine(line);
        WaitForExit();
        return ExitCode;
    }
}
