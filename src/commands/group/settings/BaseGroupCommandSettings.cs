using System.ComponentModel;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group.Settings;

public class BaseGroupCommandSettings : CommandSettings
{
    [Description("Overrides group if it already exists.")]
    [CommandOption("-f|--force")]
    [DefaultValue(false)]
    public bool Force { get; init; }

    [Description("The base group in which to perform the command.")]
    [CommandOption("-b|--base-group")]
    [DefaultValue(".")]
    public string? BaseGroup { get; init; }
}