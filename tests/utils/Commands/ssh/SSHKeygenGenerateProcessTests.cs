using SimpleGitShellrary.Utils;
using SimpleGitShellrary.Utils.Processes.SSH;
using Tests.SimpleGitShell.TestUtils;

namespace Tests.SimpleGitShellrary.Utils.Commands.SSH;

[Collection("File System Sequential")]
public class SSHKeygenGenerateProcessTests : FileSystemTests
{

    [Fact]
    public void CreateSSHKeypairNonExistingKeyfilesGeneratesKeypair()
    {
        // Given
        var privateKeyfile = Path.Combine(SSHUtils.SSHPath, "id_rsa");
        var publicKeyfile = Path.Combine(SSHUtils.SSHPath, "id_rsa.pub");
        var email = "some-email@host.com";
        CreateDirectory(SSHUtils.SSHPath);
        var sshKeygenGenerateProcess = new SSHKeygenGenerateProcess(privateKeyfile, email);

        // When
        var exitCode = sshKeygenGenerateProcess.Start();

        // Then
        Assert.Equal(0, exitCode);

        Assert.True(File.Exists(privateKeyfile));
        var privateKeyfileText = File.ReadAllText(privateKeyfile);
        Assert.Contains("-----BEGIN OPENSSH PRIVATE KEY-----", privateKeyfileText, StringComparison.Ordinal);
        Assert.Contains("-----END OPENSSH PRIVATE KEY-----", privateKeyfileText, StringComparison.Ordinal);

        Assert.True(File.Exists(publicKeyfile));
        var publicKeyfileText = File.ReadAllText(publicKeyfile);
        Assert.Contains("ssh-rsa", publicKeyfileText, StringComparison.Ordinal);
        Assert.Contains(email, publicKeyfileText, StringComparison.Ordinal);

        // Finally
        sshKeygenGenerateProcess.Dispose();
    }

    [Fact]
    public void CreateSSHKeypairExistingKeyfilesDoesNotOverrideExistingFiles()
    {
        // Given
        var sshDir = ".ssh";
        var privateKeyfile = Path.Combine(sshDir, "id_rsa");
        var publicKeyfile = Path.Combine(sshDir, "id_rsa.pub");
        var firstKeygenEmail = "first-email@host.com";
        var secondKeygenEmail = "second-email@host.com";
        CreateDirectory(sshDir);

        var firstKeygen = new SSHKeygenGenerateProcess(privateKeyfile, firstKeygenEmail);
        firstKeygen.Start();
        var firstKeygenPrivateKeyfileText = File.ReadAllText(privateKeyfile);
        var firstKeygenPublicKeyfileText = File.ReadAllText(publicKeyfile);

        var secondKeygen = new SSHKeygenGenerateProcess(privateKeyfile, secondKeygenEmail);

        // When
        var exitCode = secondKeygen.Start();

        // Then
        Assert.Equal(1, exitCode);
        var secondKeygenPrivateKeyfileText = File.ReadAllText(privateKeyfile);
        var secondKeygenPublicKeyfileText = File.ReadAllText(publicKeyfile);
        Assert.Equal(firstKeygenPrivateKeyfileText, secondKeygenPrivateKeyfileText);
        Assert.Equal(firstKeygenPublicKeyfileText, secondKeygenPublicKeyfileText);

        // Finally
        DeleteDirectory(".ssh");
        firstKeygen.Dispose();
        secondKeygen.Dispose();
    }
}
