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
        if (string.IsNullOrEmpty(settings.Group)) 
        {
            throw new ArgumentException("The name of the Group cannot be empty.");
        }
        
        if (!Directory.Exists(settings.Group))
        {
            throw new Exception($"The group \"{settings.Group}\" does not exist.");
        }

        if (!settings.Force && Directory.EnumerateFileSystemEntries(settings.Group).Any())
        {
            throw new Exception($"The group \"{settings.Group}\" is not empty. To remove anyway use option \"-f\".");
        }
        Directory.Delete(settings.Group, true);
        AnsiConsole.Markup("[green]Removed group \"{0}\".\n[/]", settings.Group);
        return 0;
    }
}