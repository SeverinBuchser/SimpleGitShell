using System.Text.RegularExpressions;
using SimpleGitShell.Lib.Exceptions.Repo;

namespace SimpleGitShell.Lib.Utils;

public static class RepoUtils 
{
    public static string PATTERN = @"^[A-Za-z\d-]*$";

    public static void ThrowOnEmptyRepoName(string? repo)
    {
        if (string.IsNullOrWhiteSpace(repo)) throw new EmptyRepoNameException();
    }

    public static void ThrowOnRepoNameNotValid(string repo)
    {
        if (!Regex.IsMatch(repo, PATTERN)) throw new RepoNameNotValidException(repo);
    }

    public static void ThrowOnNonExistingRepo(string repo) 
    {
        if (!Directory.Exists(repo)) throw new RepoDoesNotExistException(repo);
    }
}