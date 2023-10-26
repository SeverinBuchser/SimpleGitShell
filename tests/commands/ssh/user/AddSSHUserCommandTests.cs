using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.SSH.User;
using SimpleGitShell.Library.Exceptions.SSH;
using SimpleGitShell.Library.Utils;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.Utils;
using Tests.SimpleGitShell.Utils.DataAttributes;

namespace Tests.SimpleGitShell.Commands.SSH.User;

[Collection("File System Sequential")]
public class AddSSHUserCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<AddSSHUserCommand>();
        return app;
    }

    [Theory]
    [InlineData("ssh-rsa = hello")]
    [InlineData("ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDr= hello")]
    [InlineData("ssh-rsa AAAB3NzaC1yc2EAAAADAQABAAABgQDwWPacdjBDrzWt2ddZchxa/axy0XznF6Wb6HPZQ8rBkOY8h74edi/pkmZm13SRNY8X6OYqBUaUYOTMek9KGsbyRtge2OyOwDZh0Hdt4RmsqwP2fvc8dnPYgMNAV4BU200JSuEJGC/2OEKAVIf+RkQcRZ9pnEQkjLSEc0zSvT7isPxyZktmhr1q23JvESWqrlMh1qAhOK2+nX230p5iV+qcH8EhNm9R48pD6wBNHZvGaYw0pNjS/YyjGDGWysf2PeDyYvcNhVk6e8Ce+/g+gMOHAJApEWtn596pFuTk8KgZuvwJMJ2L89M7xMGMFTUkLk6su3chqTfjjCDUB04Uf19ei9PJw0keXnFHpDVzQe6ST1aRz5S31aq989WWv26Jyw0edpUrmDSI3QN8foQYk2WcR2jWBfPT1R45UkUarUK937opo/XpuCFDY7mYhI5BZbQKGVbnmmoEkUiPQTmbvLgVxF89bFzJOlWZwC/fFtdUFOaSinMi6/M/U7b2nfTzxX8= hello")]
    [InlineData("  ")]
    [InlineData("")]
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
    public void RunValidPublicKeyWithNonExistingAuthorizedKeysCreatesAuthorizedKeysWithPublicKey(string publicKey)
    {
        // Given
        var args = new string[] { publicKey };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(".ssh"));
        Assert.True(File.Exists(SSHUtils.SSHAuthorizedKeys));
        Assert.Equal(publicKey, File.ReadAllText(SSHUtils.SSHAuthorizedKeys));

        // Finall
        DeleteDirectory(SSHUtils.SSHPath);
    }

    [Theory]
    [LineFileData("data/keys/keys.txt", 0)]
    [LineFileData("data/keys/keys.txt", 1)]
    [LineFileData("data/keys/keys.txt", 2)]
    [LineFileData("data/keys/keys.txt", 3)]
    public void RunExistingAuthorizedKeysWithoutNewLineAppendsPublicKeyOnNewLine(string publicKey)
    {
        // Given
        CreateDirectory(SSHUtils.SSHPath);
        CreateFile(SSHUtils.SSHAuthorizedKeys, "some other key");
        var args = new string[] { publicKey };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(SSHUtils.SSHPath));
        Assert.True(File.Exists(SSHUtils.SSHAuthorizedKeys));
        Assert.Equal("some other key\n" + publicKey, File.ReadAllText(SSHUtils.SSHAuthorizedKeys));

        // Finall
        DeleteDirectory(SSHUtils.SSHPath);
    }

    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 2)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 2, 3)]
    public void RunValidPublicKeysWithNonExistingAuthorizedKeysCreatesAuthorizedKeysWithMultiplePublicKeys([NotNull] params string[] publicKeys)
    {
        // Given
        var argsList = new List<string[]>();
        foreach (var publicKey in publicKeys)
        {
            argsList.Add(new string[] { publicKey });
        }


        // When
        var resultList = new List<CommandAppResult>();
        foreach (var args in argsList)
        {
            resultList.Add(App().Run(args));
        }

        // Then
        foreach (var result in resultList)
        {
            Assert.Equal(0, result.ExitCode);
        }
        Assert.True(Directory.Exists(SSHUtils.SSHPath));
        Assert.True(File.Exists(SSHUtils.SSHAuthorizedKeys));
        var authorizedKeys = File.ReadAllText(SSHUtils.SSHAuthorizedKeys);
        var authorizedKeysList = SSHUtils.ReadKeys();
        foreach (var publicKey in publicKeys)
        {
            Assert.Contains(publicKey, authorizedKeysList);
            Assert.Contains(publicKey, authorizedKeys);
        }

        // Finall
        DeleteDirectory(SSHUtils.SSHPath);
    }



    [Theory]
    [LinesFileData("data/keys/keys.txt", 0, 0)]
    [LinesFileData("data/keys/keys.txt", 0, 0, 1)]
    [LinesFileData("data/keys/keys.txt", 0, 1, 0)]
    public void RunDuplicatePublicKeyThrowsPublicKeyAlreadyExistsException(string publicKeyToAdd, params string[] existingPublicKeys)
    {
        // Given
        CreateDirectory(SSHUtils.SSHPath);
        CreateFile(SSHUtils.SSHAuthorizedKeys, string.Join("\n", existingPublicKeys));
        var args = new string[] { publicKeyToAdd };

        // When
        var result = App().RunAndCatch<PublicKeyAlreadyExistsException>(args);

        // Then
        Assert.IsType<PublicKeyAlreadyExistsException>(result.Exception);

        // Finall
        DeleteDirectory(SSHUtils.SSHPath);
    }

}
