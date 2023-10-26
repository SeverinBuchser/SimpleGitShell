namespace SimpleGitShell.Library.Exceptions.Repo;

public class RepoDoesNotExistException : RepoException
{
    public RepoDoesNotExistException(string repo) :
    base($"The repository \"{repo}\" does not exist.")
    { }
}
