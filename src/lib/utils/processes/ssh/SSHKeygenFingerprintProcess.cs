namespace SimpleGitShell.Library.Utils.Processes.SSH;

public class SSHKeygenFingerprintProcess : SSHKeygenProcess
{
    public SSHKeygenFingerprintProcess(string publicKeyfile) : base($"-l -f {publicKeyfile}") { }
}
