using Server.GitShell.Lib.Utils.Git;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Lib.utils;

[Collection("File System Sequential")]
public class GitInitBareCommandTests : GitTests
{

    [Fact]
    public void CreateGitRepo_ValidRepo_CreatesGitRepository()
    {
        // Given
        var repo = "repo";
        var initCommand = new GitInitBareCommand(repo);
        
        // When
        var process = initCommand.Start();

        // Then
        var fullRepoDir = Path.GetFullPath(repo) + "/";
        Assert.Equal(0, process.ExitCode);
        Assert.Equal($"Initialized empty Git repository in { fullRepoDir }", process.StandardOutput.ReadLine());
        Assert.True(Directory.Exists(repo));

        // Finally
        _DeleteDirectory(repo);
    }
}