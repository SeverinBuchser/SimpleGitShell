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
        // check if the group name is valid
        GroupUtils.ThrowOnEmptyGroupName(settings.Group);
        // change to base group directory
        GroupUtils.ThrowOnNonExistingGroup(settings.BaseGroup!);
        
        var basePath = settings.BaseGroup == "." ? "" : settings.BaseGroup!;
        var groupPath = Path.Combine(basePath, settings.Group!);

        // check if the new group would override an existing group
        if (!settings.Force) GroupUtils.ThrowOnExistingGroup(groupPath);
        // rename the old group, if it exists
        if (settings.Force) DirectoryUtils.RenameTmp(groupPath);
        try
        {
            Directory.CreateDirectory(groupPath);
        } catch (Exception e) 
        {
            // rename the old group back to its original name, if it exists
            DirectoryUtils.UndoRenameTmp(groupPath);
            throw new Exception(e.Message);
        }

        // on success, remove the old group
        if (DirectoryUtils.RemoveTmp(groupPath)) {
            Logger.Instance.Warn($"Group \"{ groupPath }\" already exists. Old group removed.");
        } 
        
        Logger.Instance.Info($"Created group \"{ groupPath }\".");
        return 0;
    }
}