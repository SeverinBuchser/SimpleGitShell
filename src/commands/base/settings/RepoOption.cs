using System.ComponentModel;
using SimpleGitShell.Exceptions;
using SimpleGitShell.Utils.Validation;
using Spectre.Console;
using Spectre.Console.Cli;


namespace SimpleGitShell.Commands.Base.Settings;

public class RepoOption : BaseGroupOption
{
    [Description("The name of the repository.")]
    [CommandArgument(0, "<repository>")]
    public string Repo { get; init; } = "";

    public string RepoPath { get; private set; } = "";

    public override ValidationResult Validate()
    {
        var result = base.Validate();
        if (!result.Successful)
        {
            return result;
        }
        try
        {
            NameValidationUtils.ThrowOnNameNotValid("repository", Repo);
            RepoPath = Path.Combine(BaseGroupPath, Repo + ".git");
        }
        catch (Exception e) when (e is InvalidNameException or DirectoryNotFoundException)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
