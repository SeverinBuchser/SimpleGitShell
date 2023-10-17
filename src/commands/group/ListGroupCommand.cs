using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Lib.Logging;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Lists all groups.")]
public class ListGroupCommand : Command
{

    public override int Execute([NotNull] CommandContext context)
    {
        Logger.Instance.Info($"Available Groups:\n");
        var directories = Directory.GetDirectories(".")
            .Where(dir => !dir.EndsWith(".git") && !dir.StartsWith("./.") && !dir.Equals("./git-shell-commands"));
        if (!directories.Any()) {
            Logger.Instance.Info($"There are no Groups.\n");
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