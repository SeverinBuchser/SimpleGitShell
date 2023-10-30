using System.Diagnostics.CodeAnalysis;
using SimpleGitShellrary.Logging;
using SimpleGitShellrary.Reading;
using Spectre.Console.Cli;

namespace SimpleGitShell.Commands.Base.Commands.Confirmation;

public abstract class AOverridePathCommand<TSettings> : Command<TSettings> where TSettings : CommandSettings
{
    protected abstract string AlreadyExistsMessage { get; }
    protected abstract string OverridePath { get; }
    protected abstract void PostConfirm();

    protected TSettings? Settings { get; private set; }

    public override int Execute([NotNull] CommandContext context, [NotNull] TSettings settings)
    {
        Settings = settings;
        if (Directory.Exists(OverridePath))
        {
            Logger.Instance.Warn(AlreadyExistsMessage);
            Logger.Instance.Warn($"Please confirm by typing the path which will be overriden ({OverridePath}), or anything else to abort:");
            if (Reader.Instance.ReadLine() != OverridePath)
            {
                Logger.Instance.Warn("The input did not match the name of the path. Aborting.");
                return 0;
            }
            Directory.Delete(OverridePath, true);
        }
        PostConfirm();
        return 0;
    }
}
