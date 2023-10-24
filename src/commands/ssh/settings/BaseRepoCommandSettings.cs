using System.ComponentModel;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.Settings;

public class BaseSSHCommandSettings : CommandSettings
{
    [Description("The public key in of the user. The key needs to be in plain text.")]
    [CommandArgument(0, "<public-key>")]
    public string? PublicKey { get; init; }
}