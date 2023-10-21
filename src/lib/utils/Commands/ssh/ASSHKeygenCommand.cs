namespace Server.GitShell.Lib.Utils.Commands.SSH;

public abstract class ASSHKeygenCommand : ACommand
{
    public override string BaseCommand() => "ssh-keygen";
}
