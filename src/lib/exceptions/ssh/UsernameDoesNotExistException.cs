namespace Server.GitShell.Lib.Exceptions.SSH;

public class UsernameDoesNotExistException : ArgumentException 
{
    public UsernameDoesNotExistException(string? username) : base($"The username \"{ username }\" does not Exist.") {}
}