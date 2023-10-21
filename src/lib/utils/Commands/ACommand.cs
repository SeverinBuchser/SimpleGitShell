using System.Diagnostics;

namespace Server.GitShell.Lib.Utils.Commands;

public abstract class ACommand
{
    public abstract string BaseCommand();
    public abstract string Args();

    public virtual string[] Input()
    {
        return Array.Empty<string>();
    }

    public Process Start() {
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = BaseCommand(), 
                Arguments = Args(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        foreach (var line in Input()) process.StandardInput.WriteLine(line);
        process.WaitForExit();
        return process;
    }
}
