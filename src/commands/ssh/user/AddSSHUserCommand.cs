using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.SSH.User.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.SSH.User;

public class AddSSHUserCommand : Command<BaseSSHCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] BaseSSHCommandSettings settings)
    {
        SSHUtils.AddKey(settings.PublicKey);
        Logger.Instance.Info("New public key added to authorized keys.");
        return 0;
    }
}
