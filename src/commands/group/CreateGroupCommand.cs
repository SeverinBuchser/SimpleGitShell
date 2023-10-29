using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Base.Commands.Confirmation;
using SimpleGitShell.Commands.Group.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Utils;

namespace SimpleGitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : AOverridePathCommand<SpecificGroupCommandSettings>
{
    private string? Group;
    private string? BaseGroup;
    private string? BaseGroupPath;

    protected override string AlreadyExistsMessage => "The group already exists. The group will be removed and created again!";

    protected override string OverridePath => Path.Combine(BaseGroupPath!, Group!);

    protected override void PreComnfirm([NotNull] SpecificGroupCommandSettings settings)
    {
        Group = settings.CheckGroupName();
        BaseGroup = settings.CheckBaseGroupName();
        BaseGroupPath = BaseGroup != "root" ? BaseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(BaseGroupPath);
    }

    protected override void OnConfirm()
    {
        Directory.Delete(OverridePath, true);
    }

    protected override void PostConfirm()
    {
        Directory.CreateDirectory(OverridePath);
        Logger.Instance.Info($"Created group \"{Group}\" of group \"{BaseGroup}\".");
    }
}
