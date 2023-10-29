using System.ComponentModel;
using SimpleGitShell.Commands.Base.Commands.Confirmation;
using SimpleGitShell.Commands.Group.Settings;
using SimpleGitShell.Library.Logging;

namespace SimpleGitShell.Commands.Group;

[Description("Creates a group.")]
public class CreateGroupCommand : AOverridePathCommand<SpecificGroupCommandSettings>
{
    protected override string AlreadyExistsMessage => "The group already exists. The group will be removed and created again!";

    protected override string OverridePath => Path.Combine(Settings!.BaseGroupPath, Settings!.Group);

    protected override void PostConfirm()
    {
        Directory.CreateDirectory(OverridePath);
        Logger.Instance.Info($"Created group \"{Settings!.Group}\" of group \"{Settings!.BaseGroup}\".");
    }
}
