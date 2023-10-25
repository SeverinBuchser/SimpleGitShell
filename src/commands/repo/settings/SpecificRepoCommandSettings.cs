using System.ComponentModel;
using SimpleGitShell.Lib.Utils;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo.Settings;

public class SpecificRepoCommandSettings : BaseRepoCommandSettings
{
    [Description("The name of the repository.")]
    [CommandArgument(0, "<repository>")]
    public string? Repo { get; init; }

    public string CheckRepoName()
    {
        RepoUtils.ThrowOnEmptyRepoName(Repo);
        RepoUtils.ThrowOnRepoNameNotValid(Repo!);
        return Repo!;
    }
}