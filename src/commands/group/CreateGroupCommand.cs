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
        GroupUtils.ThrowOnEmptyGroupName(settings.Group);
        if (!settings.Force) GroupUtils.ThrowOnExistingGroup(settings.Group!);
        if (settings.Force) GroupUtils.RenameTmp(settings.Group!);
        try
        {
            Directory.CreateDirectory(settings.Group!);
        } catch (System.Exception e) 
        {
            // Rollback
            GroupUtils.UndoRenameTmp(settings.Group!);
            throw new System.Exception(e.Message);
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