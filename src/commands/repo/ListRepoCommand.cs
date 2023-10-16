using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Repo.Settings;
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
            path = Path.Combine(path, group);
            group = settings.Group;
        }

        AnsiConsole.WriteLine($"Available Repos in Group \"{group}\":");
        var table = new Table().AddColumns("Repository", "Creation Time");
        var directories = Directory.GetDirectories(path)
            .Where(dir => dir.EndsWith(".git"));
        foreach (var directory in directories)
        {                
            table.AddRow(
                Path.GetFileName(directory),
                Directory.GetCreationTime(directory).ToString()
            );
        }
        AnsiConsole.Write(table);
        return 0;
    }
}