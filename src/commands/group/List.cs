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
        var table = new Table().AddColumns("Groupname", "Creation Time");
        var directories = Directory.GetDirectories(".")
            .Where(dir => !dir.EndsWith(".git") && !dir.StartsWith("./.") && !dir.Equals("./git-shell-commands"));
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