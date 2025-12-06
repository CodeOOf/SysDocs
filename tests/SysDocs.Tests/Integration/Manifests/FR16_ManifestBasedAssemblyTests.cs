using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Integration.Manifests;

/// <summary>
/// Tests for FR-16, FR-17, FR-18, FR-19: Manifest-based document assembly.
/// Verifies that SysDocs can compose formal SE documents from distributed markdown files
/// across a git repository structure using a manifest file for mapping.
/// </summary>
[TestCategory(TestCategories.Manifest)]
[TestCategory(TestCategories.Determinism)]
[TestCategory(TestCategories.CrossPlatform)]
public class FR16_ManifestBasedAssemblyTests
{
    private const string ExamplesPath = "examples";
    private const string SkyNetRepoPath = "examples/skynet-repo";
    private const string AdnsProjectPath = "examples/adns-project";
    private const string ExpectedResultsPath = "examples/expected-results/manifests";

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-16", "Manifest file support for document assembly")]
    [RequirementTest("FR-17", "Section extraction from markdown headings")]
    [RequirementTest("FR-18", "Multi-file document composition")]
    public async Task TC11_SkyNetSEMP_AssembledFromMultipleMarkdownFiles()
    {
        // Arrange
        var manifestPath = Path.Combine(SkyNetRepoPath, "sysdocs.manifest.json");
        var expectedOutputPath = Path.Combine(ExpectedResultsPath, "01_SEMP_SkyNet.pdf");
        var actualOutputPath = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}", "01_SEMP_SkyNet.pdf");

        // Verify manifest exists
        File.Exists(manifestPath).Should().BeTrue($"Manifest file should exist at {manifestPath}");

        // Verify source files exist
        File.Exists(Path.Combine(SkyNetRepoPath, "README.md")).Should().BeTrue();
        File.Exists(Path.Combine(SkyNetRepoPath, "project_description.md")).Should().BeTrue();
        File.Exists(Path.Combine(SkyNetRepoPath, "docs/engineering_process.md")).Should().BeTrue();

        // Act
        // TODO: Call SysDocs with manifest input
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: Path.GetDirectoryName(actualOutputPath)
        // );

        // Assert
        File.Exists(actualOutputPath).Should().BeTrue("PDF should be generated");

        // Verify determinism: Output should match expected baseline
        var expectedHash = await ComputeSha256HashAsync(expectedOutputPath);
        var actualHash = await ComputeSha256HashAsync(actualOutputPath);

        actualHash.Should().Be(expectedHash,
            "Manifest-based assembly should produce byte-for-byte identical output (FR-19)");

        // Verify PDF structure (requires PDF parsing library)
        // - Should contain sections from README.md (1.1-1.10)
        // - Should contain sections from project_description.md (2.1-2.8)
        // - Should contain sections from docs/engineering_process.md (3.1-3.4)
        // - Sections should be renumbered according to manifest
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-16", "Manifest file support for document assembly")]
    [RequirementTest("FR-17", "Section extraction from markdown headings")]
    public async Task TC12_SkyNetStRS_ExtractedFromSingleFile()
    {
        // Arrange
        var manifestPath = Path.Combine(SkyNetRepoPath, "sysdocs.manifest.json");
        var expectedOutputPath = Path.Combine(ExpectedResultsPath, "10_StRS_SkyNet.pdf");
        var actualOutputPath = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}", "10_StRS_SkyNet.pdf");

        // Verify source file exists
        File.Exists(Path.Combine(SkyNetRepoPath, "requirements/stakeholder_requirements.md")).Should().BeTrue();

        // Act
        // TODO: Call SysDocs with manifest input
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: Path.GetDirectoryName(actualOutputPath)
        // );

        // Assert
        File.Exists(actualOutputPath).Should().BeTrue("PDF should be generated");

        var expectedHash = await ComputeSha256HashAsync(expectedOutputPath);
        var actualHash = await ComputeSha256HashAsync(actualOutputPath);

        actualHash.Should().Be(expectedHash,
            "Manifest-based assembly should be deterministic (FR-19)");
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-16", "Manifest file support for document assembly")]
    [RequirementTest("FR-19", "Deterministic manifest-based output")]
    [RequirementTest("FR-06", "Byte-for-byte identical output")]
    [RequirementTest("NFR-01", "100% reproducible output")]
    public async Task TC13_AdnsProject_ManifestBasedAssembly_Deterministic()
    {
        // Arrange
        var manifestPath = Path.Combine(AdnsProjectPath, "sysdocs.manifest.json");
        var outputDirectory = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}");
        Directory.CreateDirectory(outputDirectory);

        // Act - Run 1
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: outputDirectory
        // );
        var run1Hashes = await ComputeDirectoryHashesAsync(outputDirectory);

        // Clean output directory
        Directory.Delete(outputDirectory, recursive: true);
        Directory.CreateDirectory(outputDirectory);

        // Act - Run 2 (same inputs)
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: outputDirectory
        // );
        var run2Hashes = await ComputeDirectoryHashesAsync(outputDirectory);

        // Assert
        run1Hashes.Should().BeEquivalentTo(run2Hashes,
            "Repeated runs with manifest should produce identical output (FR-19, NFR-01)");
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-17", "Section extraction from markdown headings")]
    public async Task TC14_SectionExtraction_HeadingPatternMatching()
    {
        // Arrange
        var manifestPath = Path.Combine(SkyNetRepoPath, "sysdocs.manifest.json");
        var readmePath = Path.Combine(SkyNetRepoPath, "README.md");
        var outputPath = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}", "test_output.pdf");

        // Verify README contains expected heading patterns
        var readmeContent = await File.ReadAllTextAsync(readmePath);
        readmeContent.Should().Contain("## 1.1 Project Scope");
        readmeContent.Should().Contain("## 1.2 System Overview");
        readmeContent.Should().Contain("## 1.3 Stakeholders");

        // Act
        // TODO: Call SysDocs with manifest that extracts specific sections
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: Path.GetDirectoryName(outputPath)
        // );

        // Assert
        File.Exists(outputPath).Should().BeTrue("PDF should be generated with extracted sections");

        // Verify PDF contains only specified sections
        // - Should include: 1.1, 1.2, 1.3 (as per manifest)
        // - Should NOT include: other sections not in manifest
        // Requires PDF text extraction to verify
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-18", "Multi-file document composition")]
    public async Task TC15_MultiFileComposition_CorrectOrdering()
    {
        // Arrange
        var manifestPath = Path.Combine(SkyNetRepoPath, "sysdocs.manifest.json");
        var outputPath = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}", "01_SEMP_SkyNet.pdf");

        // Act
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: Path.GetDirectoryName(outputPath)
        // );

        // Assert
        File.Exists(outputPath).Should().BeTrue("PDF should be generated");

        // Verify section ordering in PDF:
        // 1. Sections from README.md (1.1-1.10) should appear first
        // 2. Sections from project_description.md (2.1-2.8) should appear second
        // 3. Sections from docs/engineering_process.md (3.1-3.4) should appear third
        // Requires PDF text extraction to verify ordering
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-16", "Manifest file support")]
    [RequirementTest("FR-18", "Multi-file document composition")]
    public async Task TC16_SectionRemapping_NumberingAdjustment()
    {
        // Arrange
        var manifestPath = Path.Combine(SkyNetRepoPath, "sysdocs.manifest.json");
        var outputPath = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}", "01_SEMP_SkyNet.pdf");

        // Manifest specifies section remapping:
        // - "1.1" from README.md → "1. Project Scope" in PDF
        // - "2.1" from project_description.md → "2. System Purpose" in PDF
        // - "3.1" from docs/engineering_process.md → "3. V-Model Lifecycle" in PDF

        // Act
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: Path.GetDirectoryName(outputPath)
        // );

        // Assert
        File.Exists(outputPath).Should().BeTrue("PDF should be generated");

        // Verify section numbering in PDF matches manifest sectionMapping
        // Requires PDF text extraction to verify section titles and numbers
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-14", "Cross-platform determinism")]
    [RequirementTest("FR-19", "Deterministic manifest-based output")]
    public async Task TC17_ManifestAssembly_CrossPlatformDeterminism()
    {
        // This test should be run on Linux, Windows, and macOS
        // Each platform should produce identical PDF hashes

        // Arrange
        var manifestPath = Path.Combine(SkyNetRepoPath, "sysdocs.manifest.json");
        var outputDirectory = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}");
        Directory.CreateDirectory(outputDirectory);

        var platform = Environment.OSVersion.Platform.ToString();
        var hashFilePath = Path.Combine(outputDirectory, $"manifest-hashes-{platform}.json");

        // Act
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: outputDirectory
        // );

        var hashes = await ComputeDirectoryHashesAsync(outputDirectory);

        // Save hashes for cross-platform comparison
        var hashesJson = System.Text.Json.JsonSerializer.Serialize(hashes, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true
        });
        await File.WriteAllTextAsync(hashFilePath, hashesJson);

        // Assert
        hashes.Should().NotBeEmpty("Should generate PDF files");

        // Cross-platform comparison happens in CI/CD
        // The cross-platform-hash-verification job compares hashes across platforms
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-16", "Manifest validation")]
    public async Task TC18_InvalidManifest_ProperErrorHandling()
    {
        // Arrange
        var invalidManifestPath = Path.Combine(Path.GetTempPath(), $"invalid_manifest_{Guid.NewGuid()}.json");
        await File.WriteAllTextAsync(invalidManifestPath, "{ invalid json }");

        // Act & Assert
        // var exception = await Assert.ThrowsAsync<InvalidManifestException>(async () =>
        // {
        //     await SysDocsRunner.RunAsync(
        //         manifestPath: invalidManifestPath,
        //         outputDirectory: Path.GetTempPath()
        //     );
        // });

        // exception.Message.Should().Contain("Invalid JSON");
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-17", "Section extraction error handling")]
    public async Task TC19_MissingSectionInSourceFile_ProperErrorHandling()
    {
        // Arrange
        // Create manifest that references non-existent section
        var manifestPath = Path.Combine(Path.GetTempPath(), $"manifest_{Guid.NewGuid()}.json");
        var manifestContent = @"{
            ""documents"": [{
                ""outputName"": ""test.pdf"",
                ""sources"": [{
                    ""file"": ""README.md"",
                    ""sections"": [""99.99""]
                }]
            }]
        }";
        await File.WriteAllTextAsync(manifestPath, manifestContent);

        // Act & Assert
        // var exception = await Assert.ThrowsAsync<SectionNotFoundException>(async () =>
        // {
        //     await SysDocsRunner.RunAsync(
        //         manifestPath: manifestPath,
        //         outputDirectory: Path.GetTempPath()
        //     );
        // });

        // exception.Message.Should().Contain("Section 99.99 not found");
    }

    [Fact(Skip = "Pending SysDocs PDF generation implementation")]
    [RequirementTest("FR-16", "Manifest template support")]
    public async Task TC20_ManifestTemplateConfiguration_AppliedCorrectly()
    {
        // Arrange
        var manifestPath = Path.Combine(AdnsProjectPath, "sysdocs.manifest.json");
        var outputPath = Path.Combine(Path.GetTempPath(), $"sysdocs_test_{Guid.NewGuid()}", "01_SEMP_ADNS.pdf");

        // Manifest specifies:
        // - coverPage: true
        // - toc: true
        // - watermark: "DRAFT"

        // Act
        // await SysDocsRunner.RunAsync(
        //     manifestPath: manifestPath,
        //     outputDirectory: Path.GetDirectoryName(outputPath)
        // );

        // Assert
        File.Exists(outputPath).Should().BeTrue("PDF should be generated");

        // Verify PDF contains:
        // - Cover page
        // - Table of contents
        // - "DRAFT" watermark on pages
        // Requires PDF parsing to verify template application
    }

    #region Helper Methods

    private static async Task<string> ComputeSha256HashAsync(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var sha256 = SHA256.Create();
        var hashBytes = await sha256.ComputeHashAsync(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }

    private static async Task<Dictionary<string, string>> ComputeDirectoryHashesAsync(string directoryPath)
    {
        var hashes = new Dictionary<string, string>();
        var files = Directory.GetFiles(directoryPath, "*.pdf", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = Path.GetRelativePath(directoryPath, file);
            var hash = await ComputeSha256HashAsync(file);
            hashes[relativePath] = hash;
        }

        return hashes;
    }

    #endregion
}
