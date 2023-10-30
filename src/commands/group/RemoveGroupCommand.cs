using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Group.Settings;
using SimpleGitShellrary.Logging;
using SimpleGitShellrary.Reading;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group;

[Description("Removes a group.")]
public class RemoveGroupCommand : Command<RemoveGroupOption>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] RemoveGroupOption settings)
    {

        Logger.Instance.Warn($"Please confirm by typing the name of the group ({settings.GroupPath}):");
        if (Reader.Instance.ReadLine() != settings.GroupPath)
        {
            Logger.Instance.Warn("The input did not match the name of the group. Aborting.");
            return 0;
        }

        Directory.Delete(settings.GroupPath, true);
        Logger.Instance.Info($"Removed group \"{settings.Group}\" of group \"{settings.BaseGroup}\".");
        return 0;
    }
}
