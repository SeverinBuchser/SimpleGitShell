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
    public void Execute_EmptyRepo_ThrowsEmptyRepoNameException()
    {
        // Given
        var args = new string[]{_InvalidRepo};
        
        // When
        var result = App().RunAndCatch<EmptyRepoNameException>(args);

        // Then
        Assert.IsType<EmptyRepoNameException>(result.Exception);
        Assert.Equal($"The name of the repository cannot be empty.", result.Exception.Message);
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
    public void Execute_InvalidRepo_ThrowsRepoNameNotValidException(string repo)
    {
        // Given
        var args = new string[]{repo};
        
        // When
        var result = App().RunAndCatch<RepoNameNotValidException>(args);

        // Then
        Assert.IsType<RepoNameNotValidException>(result.Exception);
        Assert.Equal($"The name \"{ repo }\" is not valid. The repo name can only contain word characters, digits and hyphens (\"-\").", result.Exception.Message);
    }


    [Fact]
    public void Execute_ValidRepoNonExisting_CreatesRepo()
    {
        // Given
        var args = new string[]{_ValidRepo};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(_ValidRepoPath));
        Assert.Equal($"[INFO] Created repository \"{ _ValidRepoPath }\".\n", _CaptureWriter.ToString());
        
        // Finally
        _DeleteDirectory(_ValidRepoPath);
    }

    [Fact]
    public void Execute_ValidRepoExisting_ThrowsRepoAlreadyExistsException()
    {
        // Given
        new GitInitBareCommand(_ValidRepoPath).Start();
        var args = new string[]{_ValidRepo};
        
        
        // When
        var result = App().RunAndCatch<RepoAlreadyExistsException>(args);

        // Then
        Assert.IsType<RepoAlreadyExistsException>(result.Exception);
        Assert.Equal($"The repository \"{ _ValidRepoPath }\" already exists.", result.Exception.Message);
        
        // Finally
        _DeleteDirectory(_ValidRepoPath);
    }

    [Fact]
    public void Execute_ValidRepoValidGroupNonExisting_ThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[]{_ValidRepo, $"--group={ _ValidGroup }"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
        Assert.Equal($"The group \"{ _ValidGroup }\" does not exist.", result.Exception.Message);
    }

    [Fact]
    public void Execute_ValidRepoNonExistingValidGroupExisting_CreatesRepoInGroup()
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
        Assert.Equal($"[INFO] Created repository \"{ repoPath }\".\n", _CaptureWriter.ToString());

        // Finally
        _DeleteDirectory(_ValidGroup);
    }
    
    [Fact]
    public void Execute_ValidRepoExistingValidGroupExisting_ThrowsRepoAlreadyExistsException()
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
        Assert.Equal($"The repository \"{ repoPath }\" already exists.", result.Exception.Message);
        
        // Finally
        _DeleteDirectory(_ValidGroup);
    }
} 