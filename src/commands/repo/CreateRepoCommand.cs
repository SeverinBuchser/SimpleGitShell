using System.ComponentModel;
using SimpleGitShell.Commands.Base.Commands.Confirmation;
using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShell.Logging;
using SimpleGitShell.Utils.Processes.Git;

namespace SimpleGitShell.Commands.Repo;

[Description("Creates a repository.")]
public class CreateRepoCommand : AOverridePathCommand<RepoOption>
{
    protected override string AlreadyExistsMessage => "The repository already exists. The repository will be removed and created again!";

    protected override string OverridePath => Path.Combine(Settings!.BaseGroupPath, Settings!.Repo + ".git");

    protected override void PostConfirm()
    {
        using (var gitInitBareProcess = new GitInitBareProcess(OverridePath))
        {
            if (gitInitBareProcess.Start() != 0)
            {
                throw new GitException(gitInitBareProcess.StandardError.ReadToEnd());
            }
        }
        Logger.Instance.Info($"Created repository \"{Settings!.Repo}\" of group \"{Settings!.BaseGroup}\".");
    }
}
