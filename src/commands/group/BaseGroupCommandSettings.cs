using System.ComponentModel;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

public class BaseGroupCommandSettings : CommandSettings
{
    [Description("The name of the Group.")]
    [CommandArgument(0, "<group>")]
    public string? Group { get; init; }
}