using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Lists all groups.")]
public class ListGroupCommand : Command
{

    public override int Execute([NotNull] CommandContext context)
    {
        AnsiConsole.WriteLine("Available Groups:");
        var directories = Directory.GetDirectories(".")
            .Where(dir => !dir.EndsWith(".git") && !dir.StartsWith("./.") && !dir.Equals("./git-shell-commands"));
        if (!directories.Any()) {
            AnsiConsole.Markup($"[blue]There are no Groups.\n[/]");
        } else {
            var table = new Table().AddColumns("Group", "Creation Time");
            foreach (var directory in directories)
            {                
                table.AddRow(
                    Path.GetFileName(directory),
                    Directory.GetCreationTime(directory).ToString()
                );
            }
            AnsiConsole.Write(table);
        }
        return 0;
    }
}