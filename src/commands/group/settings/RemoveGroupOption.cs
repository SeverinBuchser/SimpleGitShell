using SimpleGitShell.Commands.Base.Settings;
using SimpleGitShell.Exceptions;
using SimpleGitShell.Utils.Validation;
using Spectre.Console;

namespace SimpleGitShell.Commands.Group.Settings;

public class RemoveGroupOption : GroupOption
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
            FileSystemValidationUtils.ThrowOnNonExistingDirectory(GroupPath);
        }
        catch (Exception e) when (e is InvalidNameException or DirectoryNotFoundException)
        {
            return ValidationResult.Error(e.Message);
        }
        return ValidationResult.Success();
    }
}
