using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : Command<CreateGroupCommand.Settings>
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
        if (settings.Force && Directory.Exists(settings.Group))
        {
            Directory.Move(settings.Group, settings.Group + "___tmp");
        }
        else if (Directory.Exists(settings.Group))
        {
            throw new Exception($"The group \"{settings.Group}\" already exists.");
        }

        Directory.CreateDirectory(settings.Group);

        if (settings.Force && Directory.Exists(settings.Group + "___tmp")) 
        {
            Directory.Delete(settings.Group + "___tmp", true);
            AnsiConsole.Markup(
                "[yellow]Group \"{0}\" already exists. Old group removed.\n[/]", 
                settings.Group
            );
        }

        AnsiConsole.Markup("[green]Created group \"{0}\".\n[/]", settings.Group);
        return 0;
    }
}