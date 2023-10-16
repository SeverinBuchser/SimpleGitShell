namespace Server.GitShell.Lib.Exceptions.Group;
public class GroupNonEmptyException : GroupException 
{
    public GroupNonEmptyException(string group) : 
    base($"The group \"{group}\" is not empty.") {}
}