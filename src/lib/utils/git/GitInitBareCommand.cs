namespace Server.GitShell.Lib.Utils.Git;

public class GitInitBareCommand : AGitCommand
{
    public static readonly string Command = "init";
    public string Repo;
    public string? InitalBranch;

    public GitInitBareCommand(string repo) 
    {
        Repo = repo;
    }

    public override string Args() {
        return $"{ Command } { Repo } --bare";
    }
}
