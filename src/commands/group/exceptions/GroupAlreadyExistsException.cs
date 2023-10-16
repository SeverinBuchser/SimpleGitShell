namespace Server.GitShell.Commands.Group.Exceptions;

public class GroupAlreadyExistsException : GroupException
{
    public GroupAlreadyExistsException(string group) : 
    base($"The group \"{group}\" already exists.") {}
}