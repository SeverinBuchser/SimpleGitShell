using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.SSH.Settings;
using Server.GitShell.Lib.Exceptions.SSH;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Utils.Commands.SSH;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.User;

public class AddSSHUserCommand : Command<AddSSHUserCommand.Settings>
{
    public class Settings : BaseSSHCommandSettings
    {
        [Description("The public key in of the new user. The key needs to be in plain text.")]
        [CommandArgument(0, "<public-key>")]
        public string? PublicKey { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.PublicKey)) throw new PublicKeyNotValidException();
        Directory.CreateDirectory(".tmp");
        var uuid = Guid.NewGuid();
        var tmpFile = $"id_rsa{ uuid }.pub";
        var writer = File.CreateText(tmpFile);
        writer.Write(settings.PublicKey);
        writer.Close();

        var sshKeygenFingerprintCommand = new SSHKeygenFingerprintCommand(tmpFile);
        var process = sshKeygenFingerprintCommand.Start();
        if (process.ExitCode != 0) 
        {
            File.Delete(tmpFile);
            throw new PublicKeyNotValidException();
        }
        File.Delete(tmpFile);

        if (!Directory.Exists(BaseSSHCommandSettings.SSH_PATH)) Directory.CreateDirectory(BaseSSHCommandSettings.SSH_PATH);
        StreamWriter authorizedKeys = File.AppendText(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS);
        string authorizedKeysContent = File.ReadAllText(BaseSSHCommandSettings.SSH_AUTORIZED_KEYS);
        if (authorizedKeysContent.Length > 0 && !authorizedKeysContent.EndsWith("\n"))
        {
            authorizedKeys.WriteLine();
        }
        authorizedKeys.WriteLine(settings.PublicKey);
        authorizedKeys.Close();

        Logger.Instance.Info("New public key added to authorized keys.");
        return 0;
    }
}