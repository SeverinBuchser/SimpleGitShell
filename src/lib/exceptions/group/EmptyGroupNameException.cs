namespace SimpleGitShell.Lib.Exceptions.Group;

public class EmptyGroupNameException : ArgumentException 
{
    public EmptyGroupNameException() : base("The name of the group cannot be empty.") {}
}
