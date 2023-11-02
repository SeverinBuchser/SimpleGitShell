using System.ComponentModel;
using SimpleGitShell.Commands.Base.Commands.Confirmation;
using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShell.Logging;

namespace SimpleGitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : AOverridePathCommand<GroupOption>
{
    protected override string AlreadyExistsMessage => "The group already exists. The group will be removed and created again!";

    protected override string OverridePath => Path.Combine(Settings!.BaseGroupPath, Settings!.Group);

    protected override void PostConfirm()
    {
        Directory.CreateDirectory(OverridePath);
        Logger.Instance.Info($"Created group \"{Settings!.Group}\" of group \"{Settings!.BaseGroup}\".");
    }
}
