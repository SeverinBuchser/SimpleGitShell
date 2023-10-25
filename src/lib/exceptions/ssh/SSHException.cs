namespace SimpleGitShell.Lib.Exceptions.SSH;

public class SSHException : Exception 
{
    public SSHException(string message) : base(message) {}
}