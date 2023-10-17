using System.ComponentModel;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group.Settings;

public class SpecificGroupCommandSettings : BaseGroupCommandSettings
{
    [Description("The name of the group.")]
    [CommandArgument(0, "<group>")]
    public string? Group { get; init; }
}