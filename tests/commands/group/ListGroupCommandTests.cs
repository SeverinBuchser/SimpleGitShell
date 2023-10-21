using Server.GitShell.Commands.Group;
using Server.GitShell.Lib.Exceptions.Group;
using Spectre.Console.Testing;
using Tests.Server.GitShell.Utils;

namespace Tests.Server.GitShell.Commands.Group;

[Collection("File System Sequential")]
public class ListGroupCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<ListGroupCommand>();
        return app;
    }

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void Run_InvalidBaseGroup_ThrowsGroupNameNotValidException(string group)
    {
        // Given
        var args = new string[]{$"--base-group={ group }"};
        
        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Fact]
    public void Execute_GroupsInRoot_OnlyListsGroupsInRoot()
    {
        // Given
        /*
            root:
                - git-shell-commands
                - .ssh
                - .config
                - repo1.git
                - repo2.git
                - repo3.git
                - repo4.git
                - group1 <- list
                - group2 <- list
                - group3 <- list
                - group4 <- list
                - group:
                    - repo5.git
                    - repo6.git
                    - repo7.git
                    - repo8.git
                    - group5
                    - group6
                    - group7
                    - group8
        */
        var repos1 = new string[] {
            "repo1.git", "repo2.git", "repo3.git", "repo4.git"
        };
        var repos2 = new string[] {
            "repo5.git", "repo6.git", "repo7.git", "repo8.git"
        };
        var groups1 = new string[] {
            "group1", "group2", "group3", "group4"
        };
        var groups2 = new string[] {
            "group5", "group6", "group7", "group8"
        };
        _CreateDirectory("git-shell-commands");
        _CreateDirectory(".ssh");
        _CreateDirectory(".config");
        foreach (var repo in repos1) _CreateDirectory(repo);
        foreach (var group in groups1) _CreateDirectory(group);
        foreach (var repo in repos2) _CreateDirectory(Path.Combine("group", repo));
        foreach (var group in groups2) _CreateDirectory(Path.Combine("group", group));

        // When
        var result = App().Run();

        // Then
        Assert.Equal(0, result.ExitCode);
        var output = _CaptureWriter.ToString();
        Assert.DoesNotContain("git-shell-commands", output);
        Assert.DoesNotContain(".ssh", output);
        Assert.DoesNotContain(".config", output);
        foreach (var repo in repos1) Assert.DoesNotContain(repo, output);
        foreach (var group in groups1) Assert.Contains(group, output);
        foreach (var repo in repos2) Assert.DoesNotContain(repo, output);
        foreach (var group in groups2) Assert.DoesNotContain(group, output);

        // Finally
        _DeleteDirectory("git-shell-commands");
        _DeleteDirectory(".ssh");
        _DeleteDirectory(".config");
        foreach (var repo in repos1) _DeleteDirectory(repo);
        foreach (var group in groups1) _DeleteDirectory(group);
        _DeleteDirectory("group");
    }



    [Fact]
    public void Execute_GroupsInBaseGroup_OnlyListsGroupsInBaseGroup()
    {
        // Given
        /*
            root:
                - git-shell-commands
                - .ssh
                - .config
                - repo1.git
                - repo2.git
                - repo3.git
                - repo4.git
                - group1
                - group2
                - group3
                - group4
                - basegroup:
                    - repo5.git
                    - repo6.git
                    - repo7.git
                    - repo8.git
                    - group5 <- list
                    - group6 <- list
                    - group7 <- list
                    - group8 <- list
        */
        var repos1 = new string[] {
            "repo1.git", "repo2.git", "repo3.git", "repo4.git"
        };
        var repos2 = new string[] {
            "repo5.git", "repo6.git", "repo7.git", "repo8.git"
        };
        var groups1 = new string[] {
            "group1", "group2", "group3", "group4"
        };
        var groups2 = new string[] {
            "group5", "group6", "group7", "group8"
        };
        _CreateDirectory("git-shell-commands");
        _CreateDirectory(".ssh");
        _CreateDirectory(".config");
        foreach (var repo in repos1) _CreateDirectory(repo);
        foreach (var group in groups1) _CreateDirectory(group);
        foreach (var repo in repos2) _CreateDirectory(Path.Combine("basegroup", repo));
        foreach (var group in groups2) _CreateDirectory(Path.Combine("basegroup", group));
        var args = new string[]{$"--base-group=basegroup"};

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var output = _CaptureWriter.ToString();
        Assert.DoesNotContain("git-shell-commands", output);
        Assert.DoesNotContain(".ssh", output);
        Assert.DoesNotContain(".config", output);
        foreach (var repo in repos1) Assert.DoesNotContain(repo, output);
        foreach (var group in groups1) Assert.DoesNotContain(group, output);
        foreach (var repo in repos2) Assert.DoesNotContain(repo, output);
        foreach (var group in groups2) Assert.Contains(group, output);

        // Finally
        _DeleteDirectory("git-shell-commands");
        _DeleteDirectory(".ssh");
        _DeleteDirectory(".config");
        foreach (var repo in repos1) _DeleteDirectory(repo);
        foreach (var group in groups1) _DeleteDirectory(group);
        _DeleteDirectory("basegroup");
    }
} 