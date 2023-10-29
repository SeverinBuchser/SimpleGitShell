using System.ComponentModel;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo.Settings;

public class BaseRepoCommandSettings : CommandSettings
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
