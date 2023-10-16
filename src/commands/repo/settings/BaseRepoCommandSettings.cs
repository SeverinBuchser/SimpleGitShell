using System.ComponentModel;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo.Settings;

public class BaseRepoCommandSettings : CommandSettings
{    
    [Description("The name of the Group.")]
    [CommandOption("-g|--group")]
    public string? Group { get; init; }
}