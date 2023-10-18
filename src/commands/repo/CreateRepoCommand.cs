using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Repo.Settings;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Utils;
using Server.GitShell.Lib.Utils.Git;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo;

[Description("Creates a repository.")]
public class CreateRepoCommand : Command<SpecificRepoCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificRepoCommandSettings settings)
    {
        RepoUtils.ThrowOnEmptyRepoName(settings.Repo);
        RepoUtils.ThrowOnRepoNameNotValid(settings.Repo!);
        string repoPath = settings.Repo!;
        if (!string.IsNullOrEmpty(settings.Group)) {
            GroupUtils.ThrowOnNonExistingGroup(settings.Group);
            repoPath = Path.Combine(settings.Group, repoPath);
        }
        repoPath += ".git";
        RepoUtils.ThrowOnExistingRepo(repoPath);
        var gitInitCommand = new GitInitBareCommand(repoPath);
        var process = gitInitCommand.Start();
        if (process.ExitCode != 0) {
            throw new GitException(process.StandardError.ReadToEnd());
        }

        Logger.Instance.Info($"Created repository \"{ repoPath }\".\n");
        return 0;
    }
}