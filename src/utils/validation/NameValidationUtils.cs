using System.Text.RegularExpressions;
using SimpleGitShell.Exceptions;

namespace SimpleGitShell.Utils.Validation;

public static partial class NameValidationUtils
{
    private static readonly Regex _GroupNameRegex = new(@"^[A-Za-z\d-]+$");

    public static void ThrowOnNameNotValid(string kind, string name)
    {
        if (!_GroupNameRegex.IsMatch(name))
        {
            throw new InvalidNameException(kind, name);
        }
    }
}
