namespace Server.GitShell.Lib.Exceptions.Group;
public class GroupAlreadyExistsException : GroupException
{
    public GroupAlreadyExistsException(string group) : 
    base($"The group \"{group}\" already exists.") {}
}