using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.SSH.Settings;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.User;

public class RemoveSSHUserCommand : Command<RemoveSSHUserCommand.Settings>
{
    public class Settings : BaseSSHCommandSettings
    {
        [Description("The user or the users email to remove. This is the \"comment\" of the ssh-key.")]
        [CommandArgument(0, "<user-or-email>")]
        public string? Comment { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        return 0;
    }
}