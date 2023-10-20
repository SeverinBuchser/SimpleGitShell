namespace Server.GitShell.Lib.Exceptions.Group;

public class GroupNameNotValidException : ArgumentException 
{
    public GroupNameNotValidException(string group) : base($"The name \"{ group }\" is not valid. The group name can only contain word characters, digits and hyphens (\"-\").") {}
}