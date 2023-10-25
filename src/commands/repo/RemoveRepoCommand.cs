using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Lib.Logging;
using SimpleGitShell.Lib.Reading;
using SimpleGitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo;

[Description("Removes a repository.")]
public class RemoveRepoCommand : Command<SpecificRepoCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificRepoCommandSettings settings)
    {
        var repo = settings.CheckRepoName();
        var group = settings.CheckGroupName();
        var groupPath = group != "root" ? group : ".";
        GroupUtils.ThrowOnNonExistingGroup(groupPath);
        var repoPath = Path.Combine(groupPath, repo + ".git");
        RepoUtils.ThrowOnNonExistingRepo(repoPath);

        Logger.Instance.Warn($"Please confirm by typing the name of the repository ({ repoPath }):");
        if (Reader.Instance.ReadLine() != repoPath) 
        {
            Logger.Instance.Warn("The input did not match the name of the repository. Aborting.");
            return 0;
        }

        Directory.Delete(repoPath, true);
        Logger.Instance.Info($"Removed repository \"{ repo }\" of group \"{ group }\".");
        return 0;
    }
}