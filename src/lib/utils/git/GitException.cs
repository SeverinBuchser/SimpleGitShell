namespace Server.GitShell.Lib.Utils.Git;

public class GitException : ArgumentException 
{
    public GitException(string message) : base(message) {}
}
