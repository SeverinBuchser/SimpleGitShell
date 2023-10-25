using System.ComponentModel;
using SimpleGitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group.Settings;

public class SpecificGroupCommandSettings : BaseGroupCommandSettings
{
    [Description("The name of the group.")]
    [CommandArgument(0, "<group>")]
    public string? Group { get; init; }
    
    public string CheckGroupName()
    {
        GroupUtils.ThrowOnEmptyGroupName(Group);
        GroupUtils.ThrowOnGroupNameNotValid(Group!);
        return Group!;
    }
}