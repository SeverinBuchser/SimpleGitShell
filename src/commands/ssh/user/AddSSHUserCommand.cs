using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.SSH.Settings;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.User;

public class AddSSHUserCommand : Command<BaseSSHCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] BaseSSHCommandSettings settings)
    {
        SSHUtils.AddKey(settings.PublicKey);
        Logger.Instance.Info("New public key added to authorized keys.");
        return 0;
    }
}