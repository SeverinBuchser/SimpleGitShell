using System.ComponentModel;
using SimpleGitShell.Exceptions;
using SimpleGitShell.Utils.Validation;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.Settings;

public class BaseGroupOption : CommandSettings
{
    [Description("The base group in which to perform the command.")]
    [CommandOption("-b|--base-group")]
    [DefaultValue("root")]
    public string BaseGroup { get; set; } = "root";

    public string BaseGroupPath { get; private set; } = ".";

    public override ValidationResult Validate()
    {
        try
        {
            foreach (var group in BaseGroup.Split("/"))
            {
                NameValidationUtils.ThrowOnNameNotValid("base group", group);
            }
            BaseGroupPath = BaseGroup != "root" ? BaseGroup : ".";
            FileSystemValidationUtils.ThrowOnNonExistingDirectory(BaseGroupPath);
        }
        catch (Exception e) when (e is InvalidNameException or DirectoryNotFoundException)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
