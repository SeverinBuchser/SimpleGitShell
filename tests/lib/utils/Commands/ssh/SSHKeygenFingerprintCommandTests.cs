using Server.GitShell.Lib.Utils.Processes.SSH;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Lib.Utils.Commands.SSH;

[Collection("File System Sequential")]
public class SSHKeygenFingerprintProcessTests : FileSystemTests
{

    [Fact]
    public void ValidateFingerPrint_InvalidSSHPublicKey_FailsWith255()
    {
        // Given
        _CreateFile("id_rsa.pub", "invalid content");
        var sshKeygenFingerprintProcess = new SSHKeygenFingerprintProcess("id_rsa.pub");
        
        // When
        var exitCode = sshKeygenFingerprintProcess.StartSync();

        // Then
        Assert.Equal(255, exitCode);

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
        new SSHKeygenGenerateProcess(privateKeyfile, email).StartSync();
        var sshKeygenFingerprintCommand = new SSHKeygenFingerprintProcess(publicKeyfile);
        
        // When
        var exitCode = sshKeygenFingerprintCommand.StartSync();

        // Then
        Assert.Equal(0, exitCode);

        // Finally
        _DeleteFile(privateKeyfile);
        _DeleteFile(publicKeyfile);
    }
}