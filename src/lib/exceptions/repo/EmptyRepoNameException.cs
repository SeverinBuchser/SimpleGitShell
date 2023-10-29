namespace SimpleGitShell.Library.Exceptions.Repo;

public class EmptyRepoNameException : ArgumentException
{
    public EmptyRepoNameException() : base("The name of the repository cannot be empty.") { }
}
