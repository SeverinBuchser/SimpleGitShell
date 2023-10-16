using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Repo;

public class BaseRepoCommandTests : FileSystemCommandTests
{
    protected static string _ValidGroup = "Group";
    protected static string _SubDir = "subdir";
    protected static string _InvalidGroup = "";
}