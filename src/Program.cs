using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Commands.Group;
using SimpleGitShell.Commands.Repo;
using SimpleGitShell.Commands.SSH.User;
using SimpleGitShell.Logging;
using SimpleGitShell.Utils;
using SimpleGitShell.Utils.Processes;
using Spectre.Console;
using Spectre.Console.Cli;

[assembly: CLSCompliant(false)]
namespace SimpleGitShell;

public static class Program
{
    public static int Main([NotNull] string[] args)
    {
        if (args.Length == 2 && args[0] == "-c")
        {
            args = args[1].Split(" ").Select(arg => arg.Replace("'", "\"")).ToArray();
            switch (args[0])
            {
                case "git-receive-pack":
                case "git-upload-pack":
                    var process = new StdandardInputProcess(args[0], string.Join(" ", args.Skip(1)));
                    process.Start();
                    process.WaitForExit();
                    return process.ExitCode;

                default:
                    return Shell(args);
            }
        }
        return 128;
    }

    private static int Shell(string[] args)
    {
        var app = BuildShell();
        return app.Run(args);
    }

    private static CommandApp BuildShell()
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.SetApplicationVersion(VerstionUtils.InformationalVersion());
            config.SetExceptionHandler(e =>
            {
                Logger.Instance.Error(e.Message);
                return 1;
            });
            config.AddBranch("group", config =>
            {
                config.AddCommand<ListGroupCommand>("list");
                config.AddCommand<CreateGroupCommand>("create");
                config.AddCommand<RemoveGroupCommand>("remove");
            });

            config.AddBranch("repo", config =>
            {
                config.AddCommand<ListRepoCommand>("list");
                config.AddCommand<CreateRepoCommand>("create");
                config.AddCommand<RemoveRepoCommand>("remove");
            });
            config.AddBranch("ssh", config =>
            {
                config.AddBranch("user", config =>
                {
                    config.AddCommand<ListSSHUserCommand>("list");
                    config.AddCommand<AddSSHUserCommand>("add");
                    config.AddCommand<RemoveSSHUserCommand>("remove");
                });
            });
        });
        return app;
    }
}
