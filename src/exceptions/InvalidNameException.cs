namespace SimpleGitShellrary.Exceptions;

public class InvalidNameException : Exception
{
    public string Kind { get; }

    public InvalidNameException(string kind, string name) : base($"The {kind} name \"{name}\" is not valid. The name can only contain word characters, digits and hyphens (\"-\").")
    {
        Kind = kind;
    }

}
