using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Repo.Settings;
using SimpleGitShell.Logging;
using SimpleGitShell.Reading;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo;

[Description("Removes a repository.")]
public class RemoveRepoCommand : Command<RemoveRepoOption>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] RemoveRepoOption settings)
    {
        Logger.Instance.Warn($"Please confirm by typing the name of the repository ({settings.RepoPath}):");
        if (Reader.Instance.ReadLine() != settings.RepoPath)
        {
            Logger.Instance.Warn("The input did not match the name of the repository. Aborting.");
            return 0;
        }

        Directory.Delete(settings.RepoPath, true);
        Logger.Instance.Info($"Removed repository \"{settings.Repo}\" of group \"{settings.BaseGroup}\".");
        return 0;
    }
}
