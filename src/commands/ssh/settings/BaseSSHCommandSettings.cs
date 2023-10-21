using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.Settings;

public class BaseSSHCommandSettings : CommandSettings
{    
    public static readonly string SSH_PATH = ".ssh";
    public static readonly string SSH_AUTORIZED_KEYS = Path.Combine(SSH_PATH, "authorized_keys");
}