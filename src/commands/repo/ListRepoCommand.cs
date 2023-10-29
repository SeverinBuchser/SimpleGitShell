using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo;

[Description("Lists all repositories.")]
public class ListRepoCommand : Command<BaseRepoCommandSettings>
{

    public override int Execute([NotNull] CommandContext context, [NotNull] BaseRepoCommandSettings settings)
    {
        var group = settings.CheckGroupName();
        var groupPath = group != "root" ? group : ".";
        GroupUtils.ThrowOnNonExistingGroup(groupPath);

        Logger.Instance.Info($"Available repositories in group \"{group}\":");
        var directories = Directory.GetDirectories(groupPath)
            .Where(dir => dir.EndsWith(".git")).OrderBy(s => s).ToList();

        if (!directories.Any())
        {
            Logger.Instance.Info($"There are no repositories in group \"{group}\":");
        }
        else
        {
            var rows = new List<string[]>();
            foreach (var directory in directories)
            {
                rows.Add(new string[] {
                    Path.GetFileName(directory),
                    Directory.GetCreationTime(directory).ToString("dd/MM/yyyy HH:mm:ss")
                });
            }
            Logger.Instance.Table(new string[] { "Repository", "Creation Time" }, rows);
        }
        return 0;
    }
}
