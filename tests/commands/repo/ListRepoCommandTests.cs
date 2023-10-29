using SimpleGitShell.Commands.Repo;
using SimpleGitShell.Library.Exceptions.Group;
using Spectre.Console.Testing;
using Tests.SimpleGitShell.Utils;

namespace Tests.SimpleGitShell.Commands.Repo;

[Collection("File System Sequential")]
public class ListRepoCommandTests : FileSystemTests
{
    private static CommandAppTester App()
    {
        var app = new CommandAppTester();
        app.SetDefaultCommand<ListRepoCommand>();
        return app;
    }

    [Theory]
    [InlineData("$")]
    [InlineData("#")]
    [InlineData("\\")]
    [InlineData("(")]
    [InlineData("`")]
    [InlineData("_")]
    public void RunInvalidBaseGroupThrowsGroupNameNotValidException(string baseGroup)
    {
        // Given
        var args = new string[] { $"--base-group={baseGroup}" };

        // When
        var result = App().RunAndCatch<GroupNameNotValidException>(args);

        // Then
        Assert.IsType<GroupNameNotValidException>(result.Exception);
    }

    [Fact]
    public void RunReposInRootOnlyListsReposInRoot()
    {
        // Given
        /*
            root:
                - git-shell-commands
                - .ssh
                - .config
                - repo1.git <- list
                - repo2.git <- list
                - repo3.git <- list
                - repo4.git <- list
                - group1
                - group2
                - group3
                - group4
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
        CreateDirectory("git-shell-commands");
        CreateDirectory(".ssh");
        CreateDirectory(".config");
        foreach (var repo in repos1)
        {
            CreateDirectory(repo);
        }

        foreach (var group in groups1)
        {
            CreateDirectory(group);
        }

        foreach (var repo in repos2)
        {
            CreateDirectory(Path.Combine("group", repo));
        }

        foreach (var group in groups2)
        {
            CreateDirectory(Path.Combine("group", group));
        }

        // When
        var result = App().Run();

        // Then
        Assert.Equal(0, result.ExitCode);
        var output = CaptureWriter.ToString();
        Assert.DoesNotContain("git-shell-commands", output);
        Assert.DoesNotContain(".ssh", output);
        Assert.DoesNotContain(".config", output);
        foreach (var repo in repos1)
        {
            Assert.Contains(repo, output);
        }

        foreach (var group in groups1)
        {
            Assert.DoesNotContain(group, output);
        }

        foreach (var repo in repos2)
        {
            Assert.DoesNotContain(repo, output);
        }

        foreach (var group in groups2)
        {
            Assert.DoesNotContain(group, output);
        }

        // Finally
        DeleteDirectory("git-shell-commands");
        DeleteDirectory(".ssh");
        DeleteDirectory(".config");
        foreach (var repo in repos1)
        {
            DeleteDirectory(repo);
        }

        foreach (var group in groups1)
        {
            DeleteDirectory(group);
        }

        DeleteDirectory("group");
    }

    [Fact]
    public void RunReposInBaseGroupOnlyListsReposInBaseGroup()
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
                - group:
                    - repo5.git <- list
                    - repo6.git <- list
                    - repo7.git <- list
                    - repo8.git <- list
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
        CreateDirectory("git-shell-commands");
        CreateDirectory(".ssh");
        CreateDirectory(".config");
        foreach (var repo in repos1)
        {
            CreateDirectory(repo);
        }

        foreach (var group in groups1)
        {
            CreateDirectory(group);
        }

        foreach (var repo in repos2)
        {
            CreateDirectory(Path.Combine("group", repo));
        }

        foreach (var group in groups2)
        {
            CreateDirectory(Path.Combine("group", group));
        }

        var args = new string[] { $"--base-group=group" };

        // When
        var result = App().Run(args);

        // Then
        Assert.Equal(0, result.ExitCode);
        var output = CaptureWriter.ToString();
        Assert.DoesNotContain("git-shell-commands", output);
        Assert.DoesNotContain(".ssh", output);
        Assert.DoesNotContain(".config", output);
        foreach (var repo in repos1)
        {
            Assert.DoesNotContain(repo, output);
        }

        foreach (var group in groups1)
        {
            Assert.DoesNotContain(group, output);
        }

        foreach (var repo in repos2)
        {
            Assert.Contains(repo, output);
        }

        foreach (var group in groups2)
        {
            Assert.DoesNotContain(group, output);
        }

        // Finally
        DeleteDirectory("git-shell-commands");
        DeleteDirectory(".ssh");
        DeleteDirectory(".config");
        foreach (var repo in repos1)
        {
            DeleteDirectory(repo);
        }

        foreach (var group in groups1)
        {
            DeleteDirectory(group);
        }

        DeleteDirectory("group");
    }
}
