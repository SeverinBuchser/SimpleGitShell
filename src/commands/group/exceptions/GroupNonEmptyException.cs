namespace Server.GitShell.Commands.Group.Exceptions;

public class GroupNonEmptyException : GroupException 
{
    public GroupNonEmptyException(string group) : 
    base($"The group \"{group}\" is not empty.") {}
}