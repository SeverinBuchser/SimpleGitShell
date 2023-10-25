using System.ComponentModel;
using SimpleGitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group.Settings;

public class BaseGroupCommandSettings : CommandSettings
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