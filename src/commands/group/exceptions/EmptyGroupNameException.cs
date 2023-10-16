namespace Server.GitShell.Commands.Group.Exceptions;

public class EmptyGroupNameException : ArgumentException 
{
    public EmptyGroupNameException() : base("The name of the Group cannot be empty.") {}
}
