using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Server.GitShell.Commands.Repo.Settings;
using Server.GitShell.Lib.Logging;
using Server.GitShell.Lib.Reading;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo;

[Description("Removes a repository.")]
public class RemoveRepoCommand : Command<SpecificRepoCommandSettings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] SpecificRepoCommandSettings settings)
    {
        Logger.Instance.Info(Reader.Instance.ReadLine()!);
        return 0;
    }
}