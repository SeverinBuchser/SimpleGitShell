namespace SimpleGitShell.Lib.Exceptions.SSH;

public class PublicKeyNotValidException : SSHException
{
    public PublicKeyNotValidException() : base("Public key is not a valid ssh-key.") {}
}