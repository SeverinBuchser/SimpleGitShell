namespace SimpleGitShell.Library.Exceptions.Repo;

public class RepoNameNotValidException : ArgumentException
{
    public RepoNameNotValidException(string repo) : base($"The name \"{repo}\" is not valid. The repo name can only contain word characters, digits and hyphens (\"-\").") { }
}
