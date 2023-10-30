using System.Text.RegularExpressions;
using SimpleGitShellrary.Exceptions;

namespace SimpleGitShellrary.Utils.Validation;

public static partial class NameValidationUtils
{
    [GeneratedRegex("^[A-Za-z\\d-]+$")]
    private static partial Regex GroupNameRegex();

    public static void ThrowOnNameNotValid(string kind, string name)
    {
        if (!GroupNameRegex().IsMatch(name))
        {
            throw new InvalidNameException(kind, name);
        }
    }
}
