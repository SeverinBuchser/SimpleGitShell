using Server.GitShell.Commands.Repo;
using Spectre.Console.Testing;

namespace Tests.Server.GitShell.Commands.Repo;

[Collection("File System Sequential")]
public class RemoveRepoCommandTests : BaseRepoCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<RemoveRepoCommand>();
        return app;
    }

    [Fact]
    public void Run_RemoveExistingRepo_RemovesRepo()
    {
        // Given
        _SetInput("SampleInput");
        var args = new string[]{_InvalidGroup};
        
        // When
        var result = App().Run(args);

        // Then
    }
} 