using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Group.Settings;
using Server.GitShell.Lib.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : Command<SpecificGroupCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificGroupCommandSettings settings)
    {
        GroupUtils.ThrowOnEmptyGroupName(settings.Group);
        if (!settings.Force) GroupUtils.ThrowOnExistingGroup(settings.Group!);
        if (settings.Force) GroupUtils.RenameTmp(settings.Group!);
        try
        {
            Directory.CreateDirectory(settings.Group!);
        } catch (Exception e) 
        {
            // Rollback
            GroupUtils.UndoRenameTmp(settings.Group!);
            throw new Exception(e.Message);
        }

        if (GroupUtils.RemoveTmp(settings.Group!)) {
            AnsiConsole.Markup(
                "[yellow]Group \"{0}\" already exists. Old group removed.\n[/]", 
                settings.Group!
            );
        } 

        AnsiConsole.Markup("[green]Created group \"{0}\".\n[/]", settings.Group!);
        return 0;
    }
}