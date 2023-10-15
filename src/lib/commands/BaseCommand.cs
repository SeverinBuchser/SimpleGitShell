using System.CommandLine;

namespace Server.GitShell.Commands.Group;

public abstract class BaseCommand : Command
{
    protected List<Argument> _ArgumentList = new();
    protected List<Option> _Options = new();

    public BaseCommand(string name, string description) : base(name, description)
    {}

    protected void _AddArgument(Argument argument) 
    {
        _ArgumentList.Add(argument);
    }

    protected void _AddOption(Option option)
    {
        _Options.Add(option);
    }
}