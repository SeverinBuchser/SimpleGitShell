using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Base.Commands.Confirmation;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Utils;
using SimpleGitShell.Library.Utils.Processes.Git;

namespace SimpleGitShell.Commands.Repo;

[Description("Creates a repository.")]
public class CreateRepoCommand : AOverridePathCommand<SpecificRepoCommandSettings>
{
    private string? Repo;
    private string? BaseGroup;
    private string? BaseGroupPath;

    protected override string AlreadyExistsMessage => "The repository already exists. The repository will be removed and created again!";

    protected override string OverridePath => Path.Combine(BaseGroupPath!, Repo + ".git");

    protected override void PreComnfirm([NotNull] SpecificRepoCommandSettings settings)
    {
        Repo = settings.CheckRepoName();
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
        using (var gitInitBareProcess = new GitInitBareProcess(OverridePath))
        {
            if (gitInitBareProcess.Start() != 0)
            {
                throw new GitException(gitInitBareProcess.StandardError.ReadToEnd());
            }
        }
        Logger.Instance.Info($"Created repository \"{Repo}\" of group \"{BaseGroup}\".");
    }
}
