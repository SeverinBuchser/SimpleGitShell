using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Group.Settings;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Utils;
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
        Logger.Instance.Info($"Removed group \"{ settings.Group }\".\n");
        return 0;
    }
}