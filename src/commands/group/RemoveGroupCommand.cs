using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Group.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Reading;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group;

[Description("Removes a group.")]
public class RemoveGroupCommand : Command<SpecificGroupCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificGroupCommandSettings settings)
    {
        var group = settings.CheckGroupName();
        var baseGroup = settings.CheckBaseGroupName();
        var baseGroupPath = baseGroup != "root" ? baseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(baseGroupPath);
        var groupPath = Path.Combine(baseGroupPath, group);
        GroupUtils.ThrowOnNonExistingGroup(groupPath);

        Logger.Instance.Warn($"Please confirm by typing the name of the group ({groupPath}):");
        if (Reader.Instance.ReadLine() != groupPath)
        {
            Logger.Instance.Warn("The input did not match the name of the group. Aborting.");
            return 0;
        }

        Directory.Delete(groupPath, true);
        Logger.Instance.Info($"Removed group \"{group}\" of group \"{baseGroup}\".");
        return 0;
    }
}
