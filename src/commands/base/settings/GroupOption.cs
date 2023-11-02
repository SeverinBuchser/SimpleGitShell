using System.ComponentModel;
using SimpleGitShell.Exceptions;
using SimpleGitShell.Utils.Validation;
using Spectre.Console;
using Spectre.Console.Cli;


namespace SimpleGitShell.Commands.Base.Settings;

public class GroupOption : BaseGroupOption
{
    [Description("The name of the group.")]
    [CommandArgument(0, "<group>")]
    public string Group { get; set; } = "root";

    public string GroupPath { get; private set; } = "";

    public override ValidationResult Validate()
    {
        var result = base.Validate();
        if (!result.Successful)
        {
            return result;
        }
        try
        {
            NameValidationUtils.ThrowOnNameNotValid("group", Group);
            GroupPath = Path.Combine(BaseGroupPath, Group);
        }
        catch (Exception e) when (e is InvalidNameException or DirectoryNotFoundException)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
