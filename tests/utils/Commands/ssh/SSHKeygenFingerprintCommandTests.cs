using SimpleGitShellrary.Utils.Processes.SSH;
using Tests.SimpleGitShell.TestUtils;

namespace Tests.SimpleGitShellrary.Utils.Commands.SSH;

[Collection("File System Sequential")]
public class SSHKeygenFingerprintProcessTests : FileSystemTests
{

    [Fact]
    public void ValidateFingerPrintInvalidSSHPublicKeyFailsWith255()
    {
        // Given
        CreateFile("id_rsa.pub", "invalid content");
        var sshKeygenFingerprintProcess = new SSHKeygenFingerprintProcess("id_rsa.pub");

        // When
        var exitCode = sshKeygenFingerprintProcess.Start();

        // Then
        Assert.Equal(255, exitCode);

        // Finally
        DeleteFile("id_rsa.pub");
        sshKeygenFingerprintProcess.Dispose();
    }

    [Fact]
    public void CreateSSHKeypairExistingKeyfilesDoesNotOverrideExistingFiles()
    {
        // Given
        var privateKeyfile = "id_rsa";
        var publicKeyfile = $"{privateKeyfile}.pub";
        var email = "some-email@host.com";
        var firstSSHKeygenFingerprintCommand = new SSHKeygenGenerateProcess(privateKeyfile, email);
        firstSSHKeygenFingerprintCommand.Start();
        var secondSSHKeygenFingerprintCommand = new SSHKeygenFingerprintProcess(publicKeyfile);

        // When
        var exitCode = secondSSHKeygenFingerprintCommand.Start();

        // Then
        Assert.Equal(0, exitCode);

        // Finally
        DeleteFile(privateKeyfile);
        DeleteFile(publicKeyfile);
        firstSSHKeygenFingerprintCommand.Dispose();
        secondSSHKeygenFingerprintCommand.Dispose();
    }
}
