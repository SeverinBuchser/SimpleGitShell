using System.ComponentModel;
using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShell.Library.Exceptions.Group;
using SimpleGitShell.Library.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group.Settings;

public class SpecificGroupCommandSettings : BaseGroupSettings
{
    [Description("The name of the group.")]
    [CommandArgument(0, "<group>")]
    public string Group { get; init; } = "";

    public override ValidationResult Validate()
    {
        var result = base.Validate();
        if (!result.Successful)
        {
            return result;
        }
        try
        {
            GroupUtils.ThrowOnEmptyGroupName(Group);
            GroupUtils.ThrowOnGroupNameNotValid(Group);
        }
        catch (GroupException e)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
