using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Group.Settings;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Removes a group.")]
public class RemoveGroupCommand : Command<SpecificGroupCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificGroupCommandSettings settings)
    {
        GroupUtils.ThrowOnEmptyGroupName(settings.Group);
        GroupUtils.ThrowOnNonExistingGroup(settings.Group!);
        if (!settings.Force) GroupUtils.ThrowOnNonEmptyGroup(settings.Group!);

        Directory.Delete(settings.Group!, true);
        AnsiConsole.Markup("[green]Removed group \"{0}\".\n[/]", settings.Group!);
        return 0;
    }
}