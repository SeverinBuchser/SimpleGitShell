using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Library.Logging;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.Commands.List;

public abstract class AListCommand<TSettings> : Command<TSettings> where TSettings : CommandSettings
{
    protected abstract string AvailableMessage { get; }
    protected abstract string NoElementsMessage { get; }
    protected abstract IEnumerable<string> Columns { get; }
    protected abstract IEnumerable<string> GetElements();
    protected abstract string[] ToRow(string element);

    protected TSettings? Settings { get; private set; }

    public override int Execute([NotNull] CommandContext context, [NotNull] TSettings settings)
    {
        Settings = settings;
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

public abstract class AListCommand : AListCommand<EmptyCommandSettings> { }
