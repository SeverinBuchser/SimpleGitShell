
using Server.GitShell.Lib.Utils.Processes.Git;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Git;

class GitCommand : Command
{
    public override int Execute(CommandContext context)
    {
        var arguments = context.Remaining;

        if (arguments.Raw.Count == 0)
        {
            throw new ArgumentException("Please provide a Git command");
        }

        var gitProcess = new GitProcess(string.Join(" ", arguments.Raw));
        gitProcess.Attach();
        gitProcess.StartSync();

        return gitProcess.ExitCode;
    }
}