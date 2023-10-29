using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Reading;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo;

[Description("Removes a repository.")]
public class RemoveRepoCommand : Command<SpecificRepoCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificRepoCommandSettings settings)
    {
        var repo = settings.CheckRepoName();
        var baseGroup = settings.CheckBaseGroupName();
        var baseGroupPath = baseGroup != "root" ? baseGroup : ".";
        GroupUtils.ThrowOnNonExistingGroup(baseGroupPath);
        var repoPath = Path.Combine(baseGroupPath, repo + ".git");
        RepoUtils.ThrowOnNonExistingRepo(repoPath);

        Logger.Instance.Warn($"Please confirm by typing the name of the repository ({repoPath}):");
        if (Reader.Instance.ReadLine() != repoPath)
        {
            Logger.Instance.Warn("The input did not match the name of the repository. Aborting.");
            return 0;
        }

        Directory.Delete(repoPath, true);
        Logger.Instance.Info($"Removed repository \"{repo}\" of group \"{baseGroup}\".");
        return 0;
    }
}
