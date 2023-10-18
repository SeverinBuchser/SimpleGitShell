namespace Server.GitShell.Lib.Exceptions.Repo;
public class RepoNonEmptyException : RepoException 
{
    public RepoNonEmptyException(string repo) : 
    base($"The repository \"{repo}\" is not empty.") {}
}