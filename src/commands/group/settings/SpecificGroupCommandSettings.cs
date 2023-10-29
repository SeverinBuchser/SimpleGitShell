using System.ComponentModel;
using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group.Settings;

public class SpecificGroupCommandSettings : BaseGroupSettings
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
