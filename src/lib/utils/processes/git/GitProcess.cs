namespace Server.GitShell.Lib.Utils.Processes.Git;

public class GitProcess : Process
{
    public GitProcess() : this("") {}
    public GitProcess(string args) : base("git", args) {}
}
