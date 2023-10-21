namespace Server.GitShell.Lib.Utils.Commands.Git;

public class GitInitBareCommand : AGitCommand
{
    public string Repo;
    public string? InitalBranch;

    public GitInitBareCommand(string repo) 
    {
        Repo = repo;
    }

    public override string Args() {
        return $"init { Repo } --bare";
    }
}
