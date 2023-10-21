using System.Diagnostics;

namespace Tests.Server.GitShell.Utils;

public class GitTests : FileSystemTests 
{
    protected static Process _GitLog(string repo, string? extraArgs)
    {
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "git", 
                Arguments = $"git -C { repo } log { extraArgs }",
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

    protected static Process _GitBranch(string repo)
    {
        var process = new Process {
            StartInfo = new ProcessStartInfo {
                FileName = "git", 
                Arguments = $"git -C { repo } rev-parse --abbrev-ref HEAD",
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