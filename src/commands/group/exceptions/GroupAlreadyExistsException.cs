namespace Server.GitShell.Commands.Group.Exception;

public class GroupAlreadyExistsException : GroupException
{
    public GroupAlreadyExistsException(string group) : 
    base($"The group \"{group}\" already exists.") {}
}