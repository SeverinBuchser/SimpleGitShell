using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using SimpleGitShellrary.Exceptions.Group;

namespace SimpleGitShellrary.Utils;

public static partial class GroupUtils
{

    public static void ThrowOnEmptyGroupName([NotNullWhen(true)] string? group)
    {
        if (string.IsNullOrWhiteSpace(group))
        {
            throw new EmptyGroupNameException();
        }
    }

    public static void ThrowOnGroupNameNotValid(string repo)
    {
        if (!MyRegex().IsMatch(repo))
        {
            throw new GroupNameNotValidException(repo);
        }
    }

    public static void ThrowOnNonExistingGroup(string group)
    {
        if (!Directory.Exists(group))
        {
            throw new GroupDoesNotExistException(group);
        }
    }

    [GeneratedRegex("^[A-Za-z\\d-]*$")]
    private static partial Regex MyRegex();
}
