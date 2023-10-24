using Server.GitShell.Lib.Utils.Processes;
using Server.GitShell.Lib.Utils.Processes.Git;

namespace Tests.Server.GitShell.Utils;

public class GitTests : FileSystemTests 
{
    protected static Process _GitLog(string repo, string? extraArgs)
    {
        var process = new GitProcess($"git -C { repo } log { extraArgs }");
        process.Start();
        process.WaitForExit();
        return process;
    }

    protected static Process _GitBranch(string repo)
    {
        var process = new GitProcess($"git -C { repo } rev-parse --abbrev-ref HEAD");
        process.Start();
        process.WaitForExit();
        return process;
    }
}