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
        GroupUtils.ThrowOnEmptyGroupName(settings.Group);
        GroupUtils.ThrowOnNonExistingGroup(settings.Group!);
        if (!settings.Force) GroupUtils.ThrowOnNonEmptyGroup(settings.Group!);
        
        Directory.Delete(settings.Group!, true);
        AnsiConsole.Markup("[green]Removed group \"{0}\".\n[/]", settings.Group!);
        return 0;
    }
}