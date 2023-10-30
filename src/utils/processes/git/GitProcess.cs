namespace SimpleGitShellrary.Utils.Processes.Git;

public class GitProcess : RedirectedProcess
{
    public GitProcess(string args) : base("git", args) { }
}
