namespace Server.GitShell.Lib.Utils.Commands.SSH;

public class SSHKeygenCommand : ASSHKeygenCommand
{
    public readonly string PrivateKeyfile;
    public readonly string Email;

    public SSHKeygenCommand(string privateKeyfile, string email) 
    {
        PrivateKeyfile = privateKeyfile;
        Email = email;
    }

    public override string Args() {
        return $"-q -t rsa -C { Email } -N '' -f { PrivateKeyfile }";
    }

    public override string[] Input()
    {
        return new string[] {"n"};
    }
}
