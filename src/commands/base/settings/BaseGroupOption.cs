using System.ComponentModel;
using SimpleGitShell.Library.Exceptions.Group;
using SimpleGitShell.Library.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.Settings;

public class BaseGroupSettings : CommandSettings
{
    private string _BaseGroup = "root";
    [Description("The base group in which to perform the command.")]
    [CommandOption("-b|--base-group")]
    [DefaultValue("root")]
    public string BaseGroup
    {
        get => _BaseGroup;
        set
        {
            _BaseGroup = value;
            BaseGroupPath = value != "root" ? value : ".";
        }
    }

    public string BaseGroupPath { get; private set; } = ".";

    public override ValidationResult Validate()
    {
        try
        {
            GroupUtils.ThrowOnEmptyGroupName(BaseGroup);
            GroupUtils.ThrowOnGroupNameNotValid(BaseGroup);
            GroupUtils.ThrowOnNonExistingGroup(BaseGroupPath);
        }
        catch (GroupException e)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
