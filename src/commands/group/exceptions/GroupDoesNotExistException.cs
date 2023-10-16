namespace Server.GitShell.Commands.Group.Exception;

public class GroupDoesNotExistException : GroupException 
{
    public GroupDoesNotExistException(string group) : 
    base($"The group \"{group}\" does not exist.") {}
}
