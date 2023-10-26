using System.Text.RegularExpressions;
using SimpleGitShell.Library.Exceptions.Repo;

namespace SimpleGitShell.Library.Utils;

public static partial class RepoUtils
{

    public static void ThrowOnEmptyRepoName(string? repo)
    {
        if (string.IsNullOrWhiteSpace(repo))
        {
            throw new EmptyRepoNameException();
        }
    }

    public static void ThrowOnRepoNameNotValid(string repo)
    {
        if (!MyRegex().IsMatch(repo))
        {
            throw new RepoNameNotValidException(repo);
        }
    }

    public static void ThrowOnNonExistingRepo(string repo)
    {
        if (!Directory.Exists(repo))
        {
            throw new RepoDoesNotExistException(repo);
        }
    }

    [GeneratedRegex("^[A-Za-z\\d-]*$")]
    private static partial Regex MyRegex();
}
