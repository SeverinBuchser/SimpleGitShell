using System.Text.RegularExpressions;
using SimpleGitShell.Commands.SSH.User;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.Utils;
using Tests.SimpleGitShell.Utils.DataAttributes;
using Match = System.Text.RegularExpressions.Match;

namespace Tests.SimpleGitShell.Commands.SSH.User;

[Collection("File System Sequential")]
public partial class ListSSHUserCommandTests : FileSystemTests
{

    [GeneratedRegex("ssh-rsa AAAA[0-9A-Za-z+/]+[=]{0,3} ([^@]+@[^@]+)")]
    private static partial Regex CommentRegex();

    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<ListSSHUserCommand>();
        return app;
    }

    [Fact]
    public void ExecuteNoExistingKeysListsNoKeys()
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
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 2, 3)]
    public void ExecuteExistingSSHKeysListsKeys(params string[] existingKeys)
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
            Match m = CommentRegex().Match(publicKey);
            Assert.Contains(m.Groups[1].ToString(), output);
        }

        // Finally
        DeleteDirectory(SSHUtils.SSHPath);
    }
}
