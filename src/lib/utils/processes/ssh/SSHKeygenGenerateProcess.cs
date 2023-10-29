namespace SimpleGitShell.Library.Utils.Processes.SSH;

public class SSHKeygenGenerateProcess : SSHKeygenProcess
{
    public SSHKeygenGenerateProcess(string privateKeyfile, string email) : base($"-q -t rsa -C {email} -N '' -f {privateKeyfile}", new string[] { "n" }) { }
}
