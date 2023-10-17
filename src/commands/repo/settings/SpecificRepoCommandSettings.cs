using System.ComponentModel;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo.Settings;

public class SpecificRepoCommandSettings : BaseRepoCommandSettings
{
    [Description("The name of the repository.")]
    [CommandArgument(0, "<repository>")]
    public string? Repo { get; init; }
}