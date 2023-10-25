using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Group.Settings;
using SimpleGitShell.Lib.Logging;
using SimpleGitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Group;

[Description("Lists all groups.")]
public class ListGroupCommand : Command<BaseGroupCommandSettings>
{

    public override int Execute([NotNull] CommandContext context, [NotNull] BaseGroupCommandSettings settings)
    {
        var baseGroup = settings.CheckBaseGroupName();
        var baseGroupPath = baseGroup != "root" ? baseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(baseGroupPath);
        
        Logger.Instance.Info($"Available groups in base group \"{ baseGroup }\":");
        var directories = Directory.GetDirectories(baseGroupPath)
            .Where(dir => !dir.EndsWith(".git") && !dir.Contains("/.") && !dir.Equals("./git-shell-commands"));
        if (!directories.Any()) {
            Logger.Instance.Info($"There are no groups in base group \"{ baseGroup }\":");
        } else {
            var rows = new List<string[]>();
            foreach (var directory in directories)
            {                
                rows.Add(new string[] {
                    Path.GetFileName(directory),
                    Directory.GetCreationTime(directory).ToString()
                });
            }
            Logger.Instance.Table(new string[] {"Group", "Creation Time"}, rows);
        }
        return 0;
    }
}