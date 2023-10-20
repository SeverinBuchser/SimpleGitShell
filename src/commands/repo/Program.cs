﻿using Spectre.Console;
using Spectre.Console.Cli;

namespace Server.GitShell.Commands.Repo;

public class Program
{
    static int Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config => 
        {
            config.AddCommand<ListRepoCommand>("list");
            config.AddCommand<CreateRepoCommand>("create");
            config.AddCommand<RemoveRepoCommand>("remove");
        });
        try 
        {
            return app.Run(args);
        }
        catch (Exception e) {
            AnsiConsole.WriteException(e);
            return 128;
        }
    }
}