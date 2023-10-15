using System.CommandLine;

namespace Server.GitShell.Commands.Group;

public class GroupBaseCommand : Command
{
    protected Argument<string> _GroupNameArgument = new Argument<string>(
        name: "groupname",
        description: "The name of the Group."
    ); 

    protected Option<bool> _ForceOption = new Option<bool>(
        aliases: new string[] {"--force", "-f"},
        description: "Performce command by force.",
        getDefaultValue: () => false
    );

    public GroupBaseCommand(string name, string description) : base(name, description)
    {
        AddArgument(_GroupNameArgument);
        AddOption(_ForceOption);
    }
}