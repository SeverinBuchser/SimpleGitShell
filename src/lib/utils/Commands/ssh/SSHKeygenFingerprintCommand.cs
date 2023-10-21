namespace Server.GitShell.Lib.Utils.Commands.SSH;

public class SSHKeygenFingerprintCommand : ASSHKeygenCommand
{
    public string PublicKeyfile;

    public SSHKeygenFingerprintCommand(string publicKeyfile) 
    {
        PublicKeyfile = publicKeyfile;
    }

    public override string Args() {
        return $"-l -f { PublicKeyfile }";
    }
}
