namespace SimpleGitShell.Utils.Processes.SSH;

public abstract class SSHKeygenProcess : RedirectedProcess
{
    protected SSHKeygenProcess(string args) : this(args, Array.Empty<string>()) { }

    protected SSHKeygenProcess(string args, string[] input) : base("ssh-keygen", args, input) { }
}
