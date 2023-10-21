using Server.GitShell.Lib.Utils.Commands.SSH;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Lib.Utils.Commands.SSH;

[Collection("File System Sequential")]
public class SSHKeygenCommandTests : FileSystemTests
{

    [Fact]
    public void CreateSSHKeypair_NonExistingKeyfiles_GeneratesKeypair()
    {
        // Given
        var sshDir = ".ssh";
        var privateKeyfile = Path.Combine(sshDir, "id_rsa");
        var publicKeyfile = Path.Combine(sshDir, "id_rsa.pub");
        var email = "some-email@host.com";
        _CreateDirectory(sshDir);
        var sshKeygenCommand = new SSHKeygenCommand(privateKeyfile, email);
        
        // When
        var process = sshKeygenCommand.Start();

        // Then
        Assert.Equal(0, process.ExitCode);

        Assert.True(File.Exists(privateKeyfile));
        var privateKeyfileText = File.ReadAllText(privateKeyfile);
        Assert.Contains("-----BEGIN OPENSSH PRIVATE KEY-----", privateKeyfileText);
        Assert.Contains("-----END OPENSSH PRIVATE KEY-----", privateKeyfileText);

        Assert.True(File.Exists(publicKeyfile));
        var publicKeyfileText = File.ReadAllText(publicKeyfile);
        Assert.Contains("ssh-rsa", publicKeyfileText);
        Assert.Contains(email, publicKeyfileText);
    }

    [Fact]
    public void CreateSSHKeypair_ExistingKeyfiles_DoesNotOverrideExistingFiles()
    {
        // Given
        var sshDir = ".ssh";
        var privateKeyfile = Path.Combine(sshDir, "id_rsa");
        var publicKeyfile = Path.Combine(sshDir, "id_rsa.pub");
        var firstKeygenEmail = "first-email@host.com";
        var secondKeygenEmail = "second-email@host.com";
        _CreateDirectory(sshDir);

        var firstKeygen = new SSHKeygenCommand(privateKeyfile, firstKeygenEmail);
        firstKeygen.Start();
        var firstKeygenPrivateKeyfileText = File.ReadAllText(privateKeyfile);
        var firstKeygenPublicKeyfileText = File.ReadAllText(publicKeyfile);
        
        var secondKeygen = new SSHKeygenCommand(privateKeyfile, secondKeygenEmail);
        
        // When
        var process = secondKeygen.Start();

        // Then
        Assert.Equal(1, process.ExitCode);
        var secondKeygenPrivateKeyfileText = File.ReadAllText(privateKeyfile);
        var secondKeygenPublicKeyfileText = File.ReadAllText(publicKeyfile);
        Assert.Equal(firstKeygenPrivateKeyfileText, secondKeygenPrivateKeyfileText);
        Assert.Equal(firstKeygenPublicKeyfileText, secondKeygenPublicKeyfileText);

        // Finally
        _DeleteDirectory(".ssh");
    }
}