using System.Text.RegularExpressions;
using Server.GitShell.Lib.Exceptions.Repo;

namespace Server.GitShell.Lib.Utils;

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

    public static void ThrowOnExistingRepo(string repo) 
    {
        if (Directory.Exists(repo)) throw new RepoAlreadyExistsException(repo);
    }

    public static void ThrowOnNonExistingRepo(string repo) 
    {
        if (!Directory.Exists(repo)) throw new RepoDoesNotExistException(repo);
    }

    public static void ThrowOnNonEmptyRepo(string repo)
    {
        if(Directory.EnumerateFileSystemEntries(repo).Any()) throw new RepoNonEmptyException(repo);
    }
}