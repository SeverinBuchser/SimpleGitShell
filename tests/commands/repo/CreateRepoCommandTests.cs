using Server.GitShell.Commands.Repo;
using Server.GitShell.Lib.Exceptions.Group;
using Server.GitShell.Lib.Exceptions.Repo;
using Server.GitShell.Lib.Utils.Git;
using Spectre.Console.Cli;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Repo;

[Collection("File System Sequential")]
public class CreateRepoCommandTests : FileSystemCommandTests
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
        var args = new string[]{""};
        
        // When
        var result = App().RunAndCatch<EmptyRepoNameException>(args);

        // Then
        Assert.IsType<EmptyRepoNameException>(result.Exception);
    }

    [Theory]
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

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void Run_InvalidGroup_ThrowsGroupNameNotValidException(string group)
    {
        // Given
        var args = new string[]{"repo", $"--group={ group }"};
        
        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }


    [Fact]
    public void Run_ValidRepoNonExisting_CreatesRepo()
    {
        // Given
        var args = new string[]{"repo"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists("repo.git"));
        _DeleteDirectory("repo.git");
    }

    [Fact]
    public void Run_ExistingRepo_PromptsUserForConfirmation()
    {
        // Given
        new GitInitBareCommand("repo.git").Start();
        _SetInput("abort");
        var args = new string[]{"repo"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", _CaptureWriter.ToString());
        
        // Finally
        _DeleteDirectory("repo.git");
    }

    [Fact]
    public void Run_ExistingRepoAbort_DoesNotOverrideRepo()
    {
        // Given
        new GitInitBareCommand("repo.git").Start();
        var subDirPath = Path.Combine("repo.git", "subdir");
        _CreateDirectory(subDirPath);
        _SetInput("abort");
        var args = new string[]{"repo"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("repo.git");
    }

    [Fact]
    public void Run_ExistingRepoConfirm_OverridesRepo()
    {
        // Given
        new GitInitBareCommand("repo.git").Start();
        var subDirPath = Path.Combine("repo.git", "subdir");
        _CreateDirectory(subDirPath);
        _SetInput(Path.Combine(".", "repo.git"));
        var args = new string[]{"repo"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("repo.git");
    }

    [Fact]
    public void Run_NonExistingGroup_ThrowsGroupDoesNotExistException()
    {
        // Given
        var args = new string[]{"repo", $"--group=group"};
        
        // When
        var result = App().RunAndCatch<GroupDoesNotExistException>(args);

        // Then
        Assert.IsType<GroupDoesNotExistException>(result.Exception);
    }

    [Fact]
    public void Run_ExistingGroup_CreatesRepoInGroup()
    {
        // Given
        _CreateDirectory("group");
        var args = new string[]{"repo", $"--group=group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(Path.Combine("group", "repo.git")));
    }

    [Fact]
    public void Run_ExistingRepoInGroup_PromptsUserForConfirmation()
    {
        // Given
        _CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        new GitInitBareCommand(repoPath).Start();
        _CreateNonEmptyDirectory(repoPath, "subdir");
        _SetInput("abort");
        var args = new string[]{"repo", $"--group=group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.Contains("confirm", _CaptureWriter.ToString());
        
        // Finally
        _DeleteDirectory("group");
    }

    [Fact]
    public void Run_ExistingRepoInGroupAbort_DoesNotOverrideRepo()
    {
        // Given
        _CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        new GitInitBareCommand(repoPath).Start();

        var subDirPath = Path.Combine(repoPath, "subdir");
        _CreateDirectory(subDirPath);

        _SetInput("abort");
        var args = new string[]{"repo", $"--group=group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.True(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("group");
    }

    [Fact]
    public void Run_ExistingRepoInGroupConfirm_OverridesRepo()
    {
        // Given
        _CreateDirectory("group");
        var repoPath = Path.Combine("group", "repo.git");
        new GitInitBareCommand(repoPath).Start();

        var subDirPath = Path.Combine(repoPath, "subdir");
        _CreateDirectory(subDirPath);
        _SetInput(repoPath);
        var args = new string[]{"repo", $"--group=group"};
        
        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        Assert.False(Directory.Exists(subDirPath));
        
        // Finally
        _DeleteDirectory("group");
    }
} 