using System.ComponentModel;
using Server.GitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo.Settings;

public class BaseRepoCommandSettings : CommandSettings
{    
    [Description("The name of the group.")]
    [CommandOption("-g|--group")]
    [DefaultValue("root")]
    public string? Group { get; init; }

    public string CheckGroupName()
    {
        GroupUtils.ThrowOnEmptyGroupName(Group);
        GroupUtils.ThrowOnGroupNameNotValid(Group!);
        return Group!;
    }
}