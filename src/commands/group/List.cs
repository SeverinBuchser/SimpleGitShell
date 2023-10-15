using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Group;

[Description("Lists all groups.")]
public class ListGroupCommand : Command<BaseGroupCommandSettings>
{

    public override int Execute([NotNull] CommandContext context, [NotNull] BaseGroupCommandSettings settings)
    {
        return 0;
    }
}