namespace SimpleGitShell.Library.Utils.Processes.Git;

public class GitInitBareProcess : GitProcess
{
    public GitInitBareProcess(string repo) : base($"init {repo} --bare") { }
}
