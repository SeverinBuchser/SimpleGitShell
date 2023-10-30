using System.ComponentModel;
using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShellrary.Exceptions.Repo;
using SimpleGitShellrary.Utils;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Repo.Settings;

public class SpecificRepoCommandSettings : BaseGroupSettings
{
    [Description("The name of the repository.")]
    [CommandArgument(0, "<repository>")]
    public string Repo { get; init; } = "";

    public override ValidationResult Validate()
    {
        var result = base.Validate();
        if (!result.Successful)
        {
            return result;
        }
        try
        {
            RepoUtils.ThrowOnEmptyRepoName(Repo);
            RepoUtils.ThrowOnRepoNameNotValid(Repo);
        }
        catch (RepoException e)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
