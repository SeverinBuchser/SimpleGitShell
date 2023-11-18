using System.ComponentModel;
using SimpleGitShell.Exceptions.SSH;
using SimpleGitShell.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.SSH.User.Settings;

public class BaseSSHCommandSettings : CommandSettings
{
    [Description("The public key in of the user. The key needs to be in plain text.")]
    [CommandOption("-p|--public-key")]
    public string? PublicKey { get; private set; }

    public override ValidationResult Validate()
    {
        try
        {
            PublicKey = SSHUtils.ValidateKey(PublicKey);
        }
        catch (Exception e) when (e is PublicKeyNotValidException)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
