using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Group.Settings;
using Server.GitShell.Lib.Logging;
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
        if (settings.Force) DirectoryUtils.RenameTmp(settings.Group!);
        try
        {
            Directory.CreateDirectory(settings.Group!);
        } catch (Exception e) 
        {
            // Rollback
            DirectoryUtils.UndoRenameTmp(settings.Group!);
            throw new Exception(e.Message);
        }

        if (DirectoryUtils.RemoveTmp(settings.Group!)) {
            Logger.Instance.Warn($"Group \"{ settings.Group }\" already exists. Old group removed.\n");
        } 

        Logger.Instance.Info($"Created group \"{ settings.Group }\".\n");
        return 0;
    }
}