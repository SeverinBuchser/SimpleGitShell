using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Repo.Settings;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo;

[Description("Removes a repository.")]
public class RemoveRepoCommand : Command<SpecificRepoCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificRepoCommandSettings settings)
    {
        return 0;
    }
}