namespace SimpleGitShell.Exceptions;

public class InvalidNameException : Exception
{
    public InvalidNameException(string kind, string name) : base($"The {kind} name \"{name}\" is not valid. The name can only contain word characters, digits and hyphens (\"-\").") { }
}
