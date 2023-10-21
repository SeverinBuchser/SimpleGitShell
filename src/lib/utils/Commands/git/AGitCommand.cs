namespace Server.GitShell.Lib.Utils.Commands.Git;

public abstract class AGitCommand : ACommand
{
    public override string BaseCommand() => "git";
}
