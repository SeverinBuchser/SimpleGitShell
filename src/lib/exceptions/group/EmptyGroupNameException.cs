namespace Server.GitShell.Lib.Exceptions.Group;

public class EmptyGroupNameException : ArgumentException 
{
    public EmptyGroupNameException() : base("The name of the Group cannot be empty.") {}
}
