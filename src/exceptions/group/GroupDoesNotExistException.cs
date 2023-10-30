namespace SimpleGitShellrary.Exceptions.Group;

public class GroupDoesNotExistException : GroupException
{
    public GroupDoesNotExistException(string group) :
    base($"The group \"{group}\" does not exist.")
    { }
}
