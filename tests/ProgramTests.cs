using SimpleGitShell;
using Tests.SimpleGitShell.TestUtils;

namespace Tests.SimpleGitShell;

[Collection("File System Sequential")]
public class ProgramTests : FileSystemTests
{
    [Theory]
    [InlineData()]
    [InlineData("1")]
    [InlineData("1", "2", "3")]
    [InlineData("not -c", "2")]
    [InlineData("-c", "___no_such_command___")]
    public void MainInvalidArgumentsFails(params string[] args)
    {
        // Given

        // When
        int exitCode = Program.Main(args);

        // Then
        Assert.NotEqual(0, exitCode);
    }

    [Theory]
    [InlineData("git-receive-pack")]
    [InlineData("git-upload-pack")]
    public void MainGitReceiveAndGitUpload(string gitCommand)
    {
        // Given
        var args = new string[] { "-c", gitCommand };

        // When
        int exitCode = Program.Main(args);

        // Then
        Assert.Equal(129, exitCode);
    }

    [Theory]
    [InlineData("group", "list")]
    [InlineData("repo", "list")]
    [InlineData("ssh", "user", "list")]
    public void MainValidListCommandsReturnsZero(params string[] args)
    {
        // Given
        args = new string[] { "-c", string.Join(" ", args) };

        // When
        int exitCode = Program.Main(args);

        // Then
        Assert.Equal(0, exitCode);
    }

    [Fact]
    public void MainGroupCreate()
    {
        // Given
        var args = new string[] { "-c", "group create group" };

        // When
        int exitCode = Program.Main(args);

        // Then
        Assert.Equal(0, exitCode);
        Assert.True(Directory.Exists("group"));

        // Finally
        DeleteDirectory("group");
    }

    [Fact]
    public void MainRepoCreate()
    {
        // Given
        var args = new string[] { "-c", "repo create repo" };

        // When
        int exitCode = Program.Main(args);

        // Then
        Assert.Equal(0, exitCode);
        Assert.True(Directory.Exists("repo.git"));

        // Finally
        DeleteDirectory("repo.git");
    }

    [Fact]
    public void MainRepoCreateInvalid()
    {
        // Given
        var args = new string[] { "-c", "repo create r3p@@" };

        // When
        int exitCode = Program.Main(args);

        // Then
        Assert.Equal(1, exitCode);
    }
}
