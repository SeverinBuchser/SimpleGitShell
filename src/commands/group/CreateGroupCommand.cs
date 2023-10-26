using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Group.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Reading;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : Command<SpecificGroupCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificGroupCommandSettings settings)
    {
        var group = settings.CheckGroupName();
        var baseGroup = settings.CheckBaseGroupName();
        var baseGroupPath = baseGroup != "root" ? baseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(baseGroupPath);
        var groupPath = Path.Combine(baseGroupPath, group);

        if (Directory.Exists(groupPath))
        {
            Logger.Instance.Warn($"The group already exists. The group will be removed and created again!");
            Logger.Instance.Warn($"Please confirm by typing the name of the group ({groupPath}), or anything else to abort:");
            if (Reader.Instance.ReadLine() != groupPath)
            {
                Logger.Instance.Warn("The input did not match the name of the group. Aborting.");
                return 0;
            }
            Directory.Delete(groupPath, true);
        }

        Directory.CreateDirectory(groupPath);
        Logger.Instance.Info($"Created group \"{group}\" of group \"{baseGroup}\".");
        return 0;
    }
}
