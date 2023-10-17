using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

public class BaseGroupCommandTests : FileSystemCommandTests
{
    protected static string _ValidGroup = "Group";
    protected static string _SubDir = "subdir";
    protected static string _InvalidGroup = "";

    protected static string[] _GitDirs = new string[] {
        "git1.git", "git2.git", "git3.git", "git4.git"
    };
}