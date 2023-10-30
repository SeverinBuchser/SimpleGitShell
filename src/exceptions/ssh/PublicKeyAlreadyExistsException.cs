namespace SimpleGitShellrary.Exceptions.SSH;

public class PublicKeyAlreadyExistsException : SSHException
{
    public PublicKeyAlreadyExistsException() : base("Public key already exists.") { }
}
