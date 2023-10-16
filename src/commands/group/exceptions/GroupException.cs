namespace Server.GitShell.Commands.Group.Exceptions;

public class GroupException : Exception 
{
    public GroupException(string message) : base(message) {}
}