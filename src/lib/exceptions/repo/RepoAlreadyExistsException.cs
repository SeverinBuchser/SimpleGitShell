namespace Server.GitShell.Lib.Exceptions.Repo;
public class RepoAlreadyExistsException : RepoException
{
    public RepoAlreadyExistsException(string repo) : 
    base($"The repository \"{repo}\" already exists.") {}
}