using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using SimpleGitShell.Lib.Exceptions.Group;

namespace SimpleGitShell.Lib.Utils;

public static class GroupUtils 
{
    public static string PATTERN = @"^[A-Za-z\d-]*$";

    public static void ThrowOnEmptyGroupName([NotNullWhen(true)] string? group)
    {
        if (string.IsNullOrWhiteSpace(group)) throw new EmptyGroupNameException();
    }

    public static void ThrowOnGroupNameNotValid(string repo)
    {
        if (!Regex.IsMatch(repo, PATTERN)) throw new GroupNameNotValidException(repo);
    }

    public static void ThrowOnNonExistingGroup(string group) 
    {
        if (!Directory.Exists(group)) throw new GroupDoesNotExistException(group);
    }
}