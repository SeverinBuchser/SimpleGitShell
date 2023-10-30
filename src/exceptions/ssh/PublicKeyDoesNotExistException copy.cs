namespace SimpleGitShellrary.Exceptions.SSH;

public class PublicKeyDoesNotExistException : SSHException
{
    public PublicKeyDoesNotExistException() : base($"The public key does not exist.") { }
}