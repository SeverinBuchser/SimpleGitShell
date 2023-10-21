using Server.GitShell.Commands.SSH.User;
using Server.GitShell.Lib.Exceptions.SSH;
using Server.GitShell.Lib.Utils;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.SSH.User;

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
    public void Run_InvalidPublicKey_ThrowsPublicKeyNotValidException(string publicKey)
    {
        // Given
        var args = new string[]{publicKey};
        
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
    public void Run_NonExistingPublicKey_ThrowsPublicKeyDoesNotExistException(string publicKey)
    {
        // Given
        var args = new string[]{publicKey};
        
        // When
        var result = App().RunAndCatch<PublicKeyDoesNotExistException>(args);

        // Then
        Assert.IsType<PublicKeyDoesNotExistException>(result.Exception);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void Run_ExistingPublicKey_PromptsUserForConfirmation(string publicKeyToRemove, params string[] existingKeys)
    {
        // Given
        _CreateDirectory(SSHUtils.SSH_PATH);
        _CreateFile(SSHUtils.SSH_AUTORIZED_KEYS, string.Join("\n", existingKeys));
        _SetInput("abort");
        var args = new string[]{publicKeyToRemove};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", _CaptureWriter.ToString());
        
        // Finally
        _DeleteDirectory(SSHUtils.SSH_PATH);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void Run_ExistingPublicKeyAbort_DoesNotRemovePublicKey(string publicKeyToRemove, params string[] existingKeys)
    {
        // Given
        _CreateDirectory(SSHUtils.SSH_PATH);
        _CreateFile(SSHUtils.SSH_AUTORIZED_KEYS, string.Join("\n", existingKeys));
        _SetInput("abort");
        var args = new string[]{publicKeyToRemove};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var authorizedKeys = File.ReadAllText(SSHUtils.SSH_AUTORIZED_KEYS);
        var authorizedKeysList = SSHUtils.ReadKeys();
        Assert.Contains(publicKeyToRemove, authorizedKeysList);
        Assert.Contains(publicKeyToRemove, authorizedKeys);
        foreach(var publicKey in existingKeys)
        {
            Assert.Contains(publicKey, authorizedKeysList);
            Assert.Contains(publicKey, authorizedKeys);
        }
        
        // Finally
        _DeleteDirectory(SSHUtils.SSH_PATH);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void Run_ExistingPublicKeyConfirm_RemovesPublicKey(string publicKeyToRemove, params string[] existingPublicKeys)
    {
        // Given
        _CreateDirectory(SSHUtils.SSH_PATH);
        _CreateFile(SSHUtils.SSH_AUTORIZED_KEYS, string.Join("\n", existingPublicKeys));
        _SetInput("hello1");
        var args = new string[]{publicKeyToRemove};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var authorizedKeys = File.ReadAllText(SSHUtils.SSH_AUTORIZED_KEYS);
        var authorizedKeysList = SSHUtils.ReadKeys();
        Assert.DoesNotContain(publicKeyToRemove, authorizedKeysList);
        Assert.DoesNotContain(publicKeyToRemove, authorizedKeys);

        // Finally
        _DeleteDirectory(SSHUtils.SSH_PATH);
    }
} 