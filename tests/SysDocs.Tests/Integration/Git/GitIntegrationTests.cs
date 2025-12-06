using FluentAssertions;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Integration.Git;

public class GitIntegrationTests
{
    [Fact(Skip = "Pending LibGit2Sharp integration")]
    [RequirementTest("FR-14", TestType.Integration, Description = "Extract Git metadata")]
    [TestCategory(TestCategories.Integration)]
    public void GitIntegration_ShouldExtractCommitInformation()
    {
        // Arrange - Running in actual git repository
        var repoPath = GetRepositoryRoot();

        // Act
        // TODO: Extract git metadata using LibGit2Sharp
        
        // Assert
        // Should extract commit hash, author, date, branch
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-14", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    public void GitIntegration_ShouldTrackDocumentLineage()
    {
        // Arrange
        var filePath = "README.md";

        // Act
        // TODO: Get git history for file
        
        // Assert
        // Should show all commits that modified the file
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-14", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    public void GitIntegration_ShouldHandleNonGitDirectories()
    {
        // Arrange
        var nonGitPath = Path.GetTempPath();

        // Act
        // TODO: Attempt to get git info from non-git directory
        
        // Assert
        // Should gracefully handle missing git repo
    }

    private static string GetRepositoryRoot()
    {
        var dir = Directory.GetCurrentDirectory();
        while (dir != null && !Directory.Exists(Path.Combine(dir, ".git")))
        {
            dir = Directory.GetParent(dir)?.FullName;
        }
        return dir ?? throw new InvalidOperationException("Not in a git repository");
    }
}
