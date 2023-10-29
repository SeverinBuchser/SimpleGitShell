using System.ComponentModel;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.Settings;

public class BaseGroupSettings : CommandSettings
{
    [Description("The base group in which to perform the command.")]
    [CommandOption("-b|--base-group")]
    [DefaultValue("root")]
    public string? BaseGroup { get; init; }


    public string CheckBaseGroupName()
    {
        GroupUtils.ThrowOnEmptyGroupName(BaseGroup);
        GroupUtils.ThrowOnGroupNameNotValid(BaseGroup!);
        return BaseGroup!;
    }
}
