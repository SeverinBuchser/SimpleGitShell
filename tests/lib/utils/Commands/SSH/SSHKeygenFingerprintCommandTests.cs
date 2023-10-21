using Server.GitShell.Lib.Utils.Commands.SSH;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Lib.Utils.Commands.SSH;

[Collection("File System Sequential")]
public class SSHKeygenFingerprintCommandTests : FileSystemTests
{

    [Fact]
    public void ValidateFingerPrint_InvalidSSHPublicKey_FailsWith255()
    {
        // Given
        _CreateFile("id_rsa.pub", "invalid content");
        var sshKeygenFingerprintCommand = new SSHKeygenFingerprintCommand("id_rsa.pub");
        
        // When
        var process = sshKeygenFingerprintCommand.Start();

        // Then
        Assert.Equal(255, process.ExitCode);

        // Finally
        _DeleteFile("id_rsa.pub");
    }

    [Fact]
    public void CreateSSHKeypair_ExistingKeyfiles_DoesNotOverrideExistingFiles()
    {
        // Given
        var privateKeyfile = "id_rsa";
        var publicKeyfile = $"{ privateKeyfile }.pub";
        var email = "some-email@host.com";
        new SSHKeygenCommand(privateKeyfile, email).Start();
        var sshKeygenFingerprintCommand = new SSHKeygenFingerprintCommand(publicKeyfile);
        
        // When
        var process = sshKeygenFingerprintCommand.Start();

        // Then
        Assert.Equal(0, process.ExitCode);

        // Finally
        _DeleteFile(privateKeyfile);
        _DeleteFile(publicKeyfile);
    }
}