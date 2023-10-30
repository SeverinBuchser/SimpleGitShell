using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShellrary.Exceptions;
using SimpleGitShellrary.Utils.Validation;
using Spectre.Console;

namespace SimpleGitShell.Commands.Repo.Settings;

public class RemoveRepoOption : RepoOption
{
    public override ValidationResult Validate()
    {
        var result = base.Validate();
        if (!result.Successful)
        {
            return result;
        }
        try
        {
            FileSystemValidationUtils.ThrowOnNonExistingDirectory(RepoPath);
        }
        catch (Exception e) when (e is InvalidNameException or DirectoryNotFoundException)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
