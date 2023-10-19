using System.Text.RegularExpressions;
using Server.GitShell.Commands.Repo;
using Server.GitShell.Lib.Exceptions.Group;
using Server.GitShell.Lib.Exceptions.Repo;
using Server.GitShell.Lib.Utils.Git;
using Spectre.Console.Testing;

namespace Tests.Server.GitShell.Commands.Repo;

[Collection("File System Sequential")]
public class CreateRepoCommandTests : BaseRepoCommandTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<CreateRepoCommand>();
        return app;
    }

    [Fact]
    public void Run_EmptyRepo_ThrowsEmptyRepoNameException()
    {
        // Given
        var args = new string[]{_InvalidRepo};
        
        // When
        var result = App().RunAndCatch<EmptyRepoNameException>(args);

        // Then
        Assert.IsType<EmptyRepoNameException>(result.Exception);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("  ")]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void Run_InvalidRepo_ThrowsRepoNameNotValidException(string repo)
    {
        // Given
        var args = new string[]{repo};
        
        // When
        var result = App().RunAndCatch<RepoNameNotValidException>(args);

        // Then
        Assert.IsType<RepoNameNotValidException>(result.Exception);
    }


    [Fact]
    public void Run_ValidRepoNonExisting_CreatesRepo()
    {
        // Given
        var args = new string[]{_ValidRepo};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidRepoPath));
    }

    [Fact]
    public void Run_ValidRepoExisting_ThrowsRepoAlreadyExistsException()
    {
        // Given
        new GitInitBareCommand(_ValidRepoPath).Start();
        var args = new string[]{_ValidRepo};
        
        
        // When
        var result = App().RunAndCatch<RepoAlreadyExistsException>(args);

        // Then
        Assert.IsType<RepoAlreadyExistsException>(result.Exception);
    }

    [Fact]
    public void Run_ValidRepoValidGroupNonExisting_ThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[]{_ValidRepo, $"--group={ _ValidGroup }"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void Run_ValidRepoNonExistingValidGroupExisting_CreatesRepoInGroup()
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var args = new string[]{_ValidRepo, $"--group={ _ValidGroup }"};
        
        // When
        var result = App().Run(args);

        // Then
        var repoPath = Path.Combine(_ValidGroup, _ValidRepoPath);
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(repoPath));
    }
    
    [Fact]
    public void Run_ValidRepoExistingValidGroupExisting_ThrowsRepoAlreadyExistsException()
    {
        // Given
        _CreateDirectory(_ValidGroup);
        var repoPath = Path.Combine(_ValidGroup, _ValidRepoPath);
        new GitInitBareCommand(repoPath).Start();
        var args = new string[]{_ValidRepo, $"--group={ _ValidGroup }"};
        
        // When
        var result = App().RunAndCatch<RepoAlreadyExistsException>(args);

        // Then
        Assert.IsType<RepoAlreadyExistsException>(result.Exception);
    }
} 