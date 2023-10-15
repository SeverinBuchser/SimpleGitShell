using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Removes a group.")]
public class RemoveGroupCommand : Command<RemoveGroupCommand.Settings>
{
    public class Settings : BaseGroupCommandSettings
    {
        [Description("Overrides group if it already exists.")]
        [CommandOption("-f|--force")]
        [DefaultValue(false)]
        public bool Force { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        if (string.IsNullOrEmpty(settings.Groupname)) 
        {
            throw new ArgumentException("Groupname cannot be empty.");
        }
        
        if (!Directory.Exists(settings.Groupname))
        {
            throw new Exception($"The group \"{settings.Groupname}\" does not exist.");
        }

        if (!settings.Force && Directory.EnumerateFileSystemEntries(settings.Groupname).Any())
        {
            throw new Exception($"The group \"{settings.Groupname}\" is not empty. To remove anyway use option \"-f\".");
        }
        Directory.Delete(settings.Groupname, true);
        AnsiConsole.Markup("[green]Removed group \"{0}\".\n[/]", settings.Groupname);
        return 0;
    }
}