using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Repo;

public class BaseRepoCommandTests : FileSystemCommandTests
{
    protected static string _ValidGroup = "Group";
    protected static string _SubDir = "subdir";
    protected static string _InvalidGroup = "";
    
    protected static string _ValidRepo = "Repo";
    protected static string _ValidRepoPath = _ValidRepo + ".git";
    protected static string _InvalidRepo = "";
    protected static string[] _ValidRepos = new string[] {
        "git1.git", "git2.git", "git3.git", "git4.git"
    };
}