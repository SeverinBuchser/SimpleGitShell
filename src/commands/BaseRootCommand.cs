using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

namespace Server.GitShell.Commands;

public class BaseRootCommand : RootCommand
{
    protected CommandLineBuilder _Builder;

    public BaseRootCommand(string description = "") : base(description) 
    {
        _Builder = new DefaultCommandLineBuilder(this);
    }

    protected int Invoke(string[] args)
    {
        return _Builder.Build().Invoke(args);
    }
}