using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;

using Server.GitShell.Lib.Logging;

namespace Server.GitShell.Lib.Commands;

public class DefaultCommandLineBuilder : CommandLineBuilder {
    public DefaultCommandLineBuilder(RootCommand rootCommand) : base(rootCommand) 
    {
        this.UseDefaults().UseExceptionHandler(HandleException);
    }

    private static void HandleException(Exception exception, InvocationContext context)
    {
        CommandLogger.Default.LogWarning(exception.Message);
    }
}