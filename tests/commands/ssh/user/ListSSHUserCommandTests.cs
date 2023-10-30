using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.SSH.User;
using SimpleGitShellrary.Utils;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.TestUtils;
using Tests.SimpleGitShell.TestUtils.DataAttributes;

namespace Tests.SimpleGitShell.Commands.SSH.User;

[Collection("File System Sequential")]
public partial class ListSSHUserCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<ListSSHUserCommand>();
        return app;
    }

    [Fact]
    public void RunNoExistingKeysListsNoKeys()
    {
        // Given
        var args = Array.Empty<string>();

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var output = CaptureWriter.ToString();
        Assert.Contains("There are no ssh users.", output);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0)]
    [LinesFileData("data/keys/keys.txt", 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 2)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 2, 3)]
    public void RunExistingSSHKeysListsKeys([NotNull] params string[] existingKeys)
    {
        // Given
        CreateDirectory(SSHUtils.SSHPath);
        CreateFile(SSHUtils.SSHAuthorizedKeys, string.Join("\n", existingKeys));
        var args = Array.Empty<string>();

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var output = CaptureWriter.ToString();
        foreach (var publicKey in existingKeys)
        {
            Assert.Contains(SSHUtils.Comment(publicKey), output);
        }

        // Finally
        DeleteDirectory(SSHUtils.SSHPath);
    }
}
