namespace Server.GitShell.Lib.Exceptions.SSH;

public class UsernameNotValidException : ArgumentException 
{
    public UsernameNotValidException(string? username) : base($"The username \"{ username }\" is not valid.") {}
}