using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Repo.Settings;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Reading;
using Server.GitShell.Lib.Utils;
using Server.GitShell.Lib.Utils.Git;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo;

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

        if (Directory.Exists(repoPath)) {
            Logger.Instance.Warn($"The repository already exists. The repository will be removed and created again!");
            Logger.Instance.Warn($"Please confirm by typing the name of the repository ({ repoPath }), or anything else to abort:");
            if (Reader.Instance.ReadLine() != repoPath) 
            {
                Logger.Instance.Warn("The input did not match the name of the repository. Aborting.");
                return 0;
            }
            Directory.Delete(repoPath, true);
        }

        var gitInitCommand = new GitInitBareCommand(repoPath);
        var process = gitInitCommand.Start();
        if (process.ExitCode != 0) {
            throw new GitException(process.StandardError.ReadToEnd());
        }

        Logger.Instance.Info($"Created repository \"{ repo }\" of group \"{ group }\".");
        return 0;
    }
}