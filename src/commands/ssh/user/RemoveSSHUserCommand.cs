using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.SSH.Settings;
using Server.GitShell.Lib.Exceptions.SSH;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Reading;
using Server.GitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.User;

public class RemoveSSHUserCommand : Command<BaseSSHCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] BaseSSHCommandSettings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.PublicKey)) throw new PublicKeyNotValidException();
        if (!SSHUtils.DoesKeyExist(settings.PublicKey)) throw new PublicKeyDoesNotExistException();
        var comment = SSHUtils.Comment(settings.PublicKey);
        Logger.Instance.Warn($"Please confirm by typing the comment of the public key ({ comment }):");
        if (Reader.Instance.ReadLine() != comment) 
        {
            Logger.Instance.Warn("The input did not match the comment of the public key. Aborting.");
            return 0;
        }


        SSHUtils.RemoveKey(settings.PublicKey);
        Logger.Instance.Info($"Removed { comment }'s ssh-key form the authorized keys.");
        return 0;
    }
}