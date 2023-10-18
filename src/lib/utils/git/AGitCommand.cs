using System.Diagnostics;

namespace Server.GitShell.Lib.Utils.Git;

public abstract class AGitCommand
{
    public static readonly string BaseCommand = "git";
    public abstract string Args();

    public Process Start() {
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = BaseCommand, 
                Arguments = Args(),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        process.Start();
        process.WaitForExit();
        return process;
    }
}
