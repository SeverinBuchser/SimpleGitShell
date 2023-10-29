using System.Diagnostics.CodeAnalysis;
using SimpleGitShell.Library.Logging;
using SimpleGitShell.Library.Reading;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.Commands.Confirmation;

public abstract class AOverridePathCommand<TSettings> : Command<TSettings> where TSettings : CommandSettings
{
    protected abstract string AlreadyExistsMessage { get; }
    protected abstract string OverridePath { get; }
    protected abstract void PreComnfirm([NotNull] TSettings settings);
    protected abstract void OnConfirm();
    protected abstract void PostConfirm();

    public override int Execute([NotNull] CommandContext context, [NotNull] TSettings settings)
    {
        PreComnfirm(settings);
        if (Directory.Exists(OverridePath))
        {
            Logger.Instance.Warn(AlreadyExistsMessage);
            Logger.Instance.Warn($"Please confirm by typing the path which will be overriden ({OverridePath}), or anything else to abort:");
            if (Reader.Instance.ReadLine() != OverridePath)
            {
                Logger.Instance.Warn("The input did not match the name of the path. Aborting.");
                return 0;
            }
            OnConfirm();
        }
        PostConfirm();
        return 0;
    }
}
