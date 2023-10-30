using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.SSH.User;
using SimpleGitShellrary.Exceptions.SSH;
using SimpleGitShellrary.Utils;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.TestUtils;
using Tests.SimpleGitShell.TestUtils.DataAttributes;

namespace Tests.SimpleGitShell.Commands.SSH.User;

[Collection("File System Sequential")]
public class RemoveSSHUserCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveSSHUserCommand>();
        return app;
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("      ")]
    public void RunInvalidPublicKeyThrowsPublicKeyNotValidException(string publicKey)
    {
        // Given
        var args = new string[] { publicKey };

        // When
        var result = App().RunAndCatch<PublicKeyNotValidException>(args);

        // Then
        Assert.IsType<PublicKeyNotValidException>(result.Exception);
    }

    [Theory]
    [LineFileData("data/keys/keys.txt", 0)]
    [LineFileData("data/keys/keys.txt", 1)]
    [LineFileData("data/keys/keys.txt", 2)]
    [LineFileData("data/keys/keys.txt", 3)]
    public void RunNonExistingPublicKeyThrowsPublicKeyDoesNotExistException(string publicKey)
    {
        // Given
        var args = new string[] { publicKey };

        // When
        var result = App().RunAndCatch<PublicKeyDoesNotExistException>(args);

        // Then
        Assert.IsType<PublicKeyDoesNotExistException>(result.Exception);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void RunExistingPublicKeyPromptsUserForConfirmation(string publicKeyToRemove, params string[] existingKeys)
    {
        // Given
        CreateDirectory(SSHUtils.SSHPath);
        CreateFile(SSHUtils.SSHAuthorizedKeys, string.Join("\n", existingKeys));
        SetInput("abort");
        var args = new string[] { publicKeyToRemove };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", CaptureWriter.ToString());

        // Finally
        DeleteDirectory(SSHUtils.SSHPath);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void RunExistingPublicKeyAbortDoesNotRemovePublicKey(string publicKeyToRemove, [NotNull] params string[] existingKeys)
    {
        // Given
        CreateDirectory(SSHUtils.SSHPath);
        CreateFile(SSHUtils.SSHAuthorizedKeys, string.Join("\n", existingKeys));
        SetInput("abort");
        var args = new string[] { publicKeyToRemove };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var authorizedKeys = File.ReadAllText(SSHUtils.SSHAuthorizedKeys);
        var authorizedKeysList = SSHUtils.ReadKeys();
        Assert.Contains(publicKeyToRemove, authorizedKeysList);
        Assert.Contains(publicKeyToRemove, authorizedKeys);
        foreach (var publicKey in existingKeys)
        {
            Assert.Contains(publicKey, authorizedKeysList);
            Assert.Contains(publicKey, authorizedKeys);
        }

        // Finally
        DeleteDirectory(SSHUtils.SSHPath);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void RunExistingPublicKeyConfirmRemovesPublicKey(string publicKeyToRemove, params string[] existingPublicKeys)
    {
        // Given
        CreateDirectory(SSHUtils.SSHPath);
        CreateFile(SSHUtils.SSHAuthorizedKeys, string.Join("\n", existingPublicKeys));
        SetInput("user@hello1");
        var args = new string[] { publicKeyToRemove };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var authorizedKeys = File.ReadAllText(SSHUtils.SSHAuthorizedKeys);
        var authorizedKeysList = SSHUtils.ReadKeys();
        Assert.DoesNotContain(publicKeyToRemove, authorizedKeysList);
        Assert.DoesNotContain(publicKeyToRemove, authorizedKeys);

        // Finally
        DeleteDirectory(SSHUtils.SSHPath);
    }
}
