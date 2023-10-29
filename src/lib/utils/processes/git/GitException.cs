namespace SimpleGitShell.Library.Utils.Processes.Git;

public class GitException : ArgumentException
{
    public GitException(string message) : base(message) { }
}
