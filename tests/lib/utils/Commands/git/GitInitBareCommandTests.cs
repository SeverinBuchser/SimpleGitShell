using SimpleGitShell.Lib.Utils.Processes.Git;
using Tests.SimpleGitShell.Utils;

namespace Tests.SimpleGitShell.Lib.Utils.Commands.Git;

[Collection("File System Sequential")]
public class GitInitBareCommandTests : FileSystemTests
{

    [Fact]
    public void CreateGitRepo_ValidRepo_CreatesGitRepository()
    {
        // Given
        var repo = "repo";
        var initProcess = new GitInitBareProcess(repo);
        
        // When
        var exitCode = initProcess.Start();

        // Then
        var fullRepoDir = Path.GetFullPath(repo) + "/";
        Assert.Equal(0, exitCode);
        Assert.Equal($"Initialized empty Git repository in { fullRepoDir }", initProcess.StandardOutput.ReadLine());
        Assert.True(Directory.Exists(repo));

        // Finally
        _DeleteDirectory(repo);
    }
}