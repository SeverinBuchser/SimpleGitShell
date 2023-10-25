namespace SimpleGitShell.Lib.Utils.Processes.SSH;

public abstract class SSHKeygenProcess : RedirectedProcess
{
    public SSHKeygenProcess(string args) : this(args, Array.Empty<string>()) {}
    public SSHKeygenProcess(string args, string[] input) : base("ssh-keygen", args, input) {}
}
