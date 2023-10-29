using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Reading;
using SimpleGitShell.Library.Utils;
using SimpleGitShell.Library.Utils.Processes.Git;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo;

[Description("Creates a repository.")]
public class CreateRepoCommand : Command<SpecificRepoCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificRepoCommandSettings settings)
    {
        var repo = settings.CheckRepoName();
        var group = settings.CheckGroupName();
        var groupPath = group != "root" ? group : ".";
        GroupUtils.ThrowOnNonExistingGroup(groupPath);
        var repoPath = Path.Combine(groupPath, repo + ".git");

        if (Directory.Exists(repoPath))
        {
            Logger.Instance.Warn($"The repository already exists. The repository will be removed and created again!");
            Logger.Instance.Warn($"Please confirm by typing the name of the repository ({repoPath}), or anything else to abort:");
            if (Reader.Instance.ReadLine() != repoPath)
            {
                Logger.Instance.Warn("The input did not match the name of the repository. Aborting.");
                return 0;
            }
            Directory.Delete(repoPath, true);
        }

        using (var gitInitBareProcess = new GitInitBareProcess(repoPath))
        {
            if (gitInitBareProcess.Start() != 0)
            {
                throw new GitException(gitInitBareProcess.StandardError.ReadToEnd());
            }
        }

        Logger.Instance.Info($"Created repository \"{repo}\" of group \"{group}\".");
        return 0;
    }
}
