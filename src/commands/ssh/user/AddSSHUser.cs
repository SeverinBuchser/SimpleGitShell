using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.SSH.User;

class AddSSHUser : Command<AddSSHUser.Settings>
{
    public class Settings : CommandSettings
    {
        [Description("")]
        [CommandArgument(0, "<public-key>")]
        public string? PublicKey { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        return 0;
    }
}