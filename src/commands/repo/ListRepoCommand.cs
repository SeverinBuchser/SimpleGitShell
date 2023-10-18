using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Server.GitShell.Commands.Repo.Settings;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo;

[Description("Lists all repositories.")]
public class ListRepoCommand : Command<BaseRepoCommandSettings>
{

    public override int Execute([NotNull] CommandContext context, [NotNull] BaseRepoCommandSettings settings)
    {
        var path = ".";
        var group = "root";
        if (!string.IsNullOrEmpty(settings.Group)) {
            GroupUtils.ThrowOnNonExistingGroup(settings.Group);
            path = Path.Combine(path, settings.Group);
            group = settings.Group;
        }

        Logger.Instance.Info($"Available repositories in group \"{group}\":\n");
        var directories = Directory.GetDirectories(path)
            .Where(dir => dir.EndsWith(".git")).OrderBy(s => s);

        if (!directories.Any()) {
            Logger.Instance.Info($"There are no repositories.\n");
        } else {
            var rows = new List<string[]>();
            foreach (var directory in directories)
            {                
                rows.Add(new string[] {
                    Path.GetFileName(directory),
                    Directory.GetCreationTime(directory).ToString()
                });
            }
            Logger.Instance.Table(new string[] {"Repository", "Creation Time"}, rows);
        }
        return 0;
    }
}