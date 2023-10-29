using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Library.Logging;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.List;

public abstract class AListCommandSettings<TSettings> : Command<TSettings> where TSettings : CommandSettings
{
    protected abstract string AvailableMessage { get; }
    protected abstract string NoElementsMessage { get; }
    protected abstract IEnumerable<string> Columns { get; }
    protected abstract void PreExecute([NotNull] TSettings settings);
    protected abstract IEnumerable<string> GetElements();
    protected abstract string[] ToRow(string element);

    public override int Execute([NotNull] CommandContext context, [NotNull] TSettings settings)
    {
        PreExecute(settings);
        Logger.Instance.Info(AvailableMessage);
        var elements = GetElements().ToArray();
        if (!elements.Any())
        {
            Logger.Instance.Info(NoElementsMessage);
        }
        else
        {
            var rows = new List<string[]>();
            foreach (var element in elements)
            {
                rows.Add(ToRow(element));
            }
            Logger.Instance.Table(Columns, rows);
        }
        return 0;
    }
}
