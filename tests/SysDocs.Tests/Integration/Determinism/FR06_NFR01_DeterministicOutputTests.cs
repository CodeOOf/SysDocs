using System.Security.Cryptography;
using FluentAssertions;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Integration.Determinism;

/// <summary>
/// Tests for FR-06 and NFR-01: Deterministic output
/// Verifies that same input produces byte-for-byte identical output
/// </summary>
public class FR06_NFR01_DeterministicOutputTests : IDisposable
{
    private readonly string _testProjectPath;
    private readonly string _tempOutputPath;

    public FR06_NFR01_DeterministicOutputTests()
    {
        var solutionRoot = FindSolutionRoot();
        _testProjectPath = Path.Combine(solutionRoot, "examples", "adns-project");
        _tempOutputPath = Path.Combine(Path.GetTempPath(), $"sysdocs-test-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempOutputPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempOutputPath))
        {
            Directory.Delete(_tempOutputPath, recursive: true);
        }
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-06", TestType.Integration, Description = "Deterministic output")]
    [RequirementTest("NFR-01", TestType.Integration, Description = "Reproducible builds")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.Determinism)]
    public async Task TC09_Should_Produce_Identical_Output_On_Repeated_Runs()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var outputFile1 = Path.Combine(_tempOutputPath, "run1.pdf");
        var outputFile2 = Path.Combine(_tempOutputPath, "run2.pdf");
        var outputFile3 = Path.Combine(_tempOutputPath, "run3.pdf");

        // Act - Generate three times
        await GeneratePdfAsync(inputFile, outputFile1);
        await GeneratePdfAsync(inputFile, outputFile2);
        await GeneratePdfAsync(inputFile, outputFile3);

        // Assert - All three outputs must be byte-for-byte identical
        var hash1 = ComputeSha256(outputFile1);
        var hash2 = ComputeSha256(outputFile2);
        var hash3 = ComputeSha256(outputFile3);

        hash2.Should().Be(hash1, "Second run must produce identical output");
        hash3.Should().Be(hash1, "Third run must produce identical output");

        // Additional verification: file sizes should be identical
        var size1 = new FileInfo(outputFile1).Length;
        var size2 = new FileInfo(outputFile2).Length;
        var size3 = new FileInfo(outputFile3).Length;

        size2.Should().Be(size1, "File sizes must be identical");
        size3.Should().Be(size1, "File sizes must be identical");
    }

    [Theory(Skip = "Pending implementation")]
    [InlineData("12_SRS_ADNS.md")]
    [InlineData("21_SDD_ADNS_Software.tex")]
    [InlineData("22_HDD_ADNS_Hardware.txt")]
    [RequirementTest("FR-06", TestType.Integration)]
    [RequirementTest("NFR-01", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.Determinism)]
    public async Task Should_Be_Deterministic_For_All_Formats(string inputFileName)
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, inputFileName);
        var outputFile1 = Path.Combine(_tempOutputPath, $"{inputFileName}_run1.pdf");
        var outputFile2 = Path.Combine(_tempOutputPath, $"{inputFileName}_run2.pdf");

        // Act
        await GeneratePdfAsync(inputFile, outputFile1);
        await GeneratePdfAsync(inputFile, outputFile2);

        // Assert
        var hash1 = ComputeSha256(outputFile1);
        var hash2 = ComputeSha256(outputFile2);

        hash2.Should().Be(hash1, 
            $"Determinism must hold for {Path.GetExtension(inputFileName)} format");
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-06", TestType.Integration)]
    [RequirementTest("NFR-01", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.Determinism)]
    public async Task Should_Be_Deterministic_For_Mixed_Inputs()
    {
        // Arrange
        var inputFiles = new[]
        {
            Path.Combine(_testProjectPath, "12_SRS_ADNS.md"),
            Path.Combine(_testProjectPath, "requirements_export.csv")
        };
        var outputFile1 = Path.Combine(_tempOutputPath, "mixed_run1.pdf");
        var outputFile2 = Path.Combine(_tempOutputPath, "mixed_run2.pdf");

        // Act
        await GeneratePdfAsync(inputFiles, outputFile1);
        await GeneratePdfAsync(inputFiles, outputFile2);

        // Assert
        var hash1 = ComputeSha256(outputFile1);
        var hash2 = ComputeSha256(outputFile2);

        hash2.Should().Be(hash1, "Mixed format inputs must also produce deterministic output");
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-06", TestType.Integration)]
    [RequirementTest("NFR-01", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.Determinism)]
    public async Task Should_Be_Deterministic_For_Folder_Input()
    {
        // Arrange
        var inputFolder = _testProjectPath;
        var outputFile1 = Path.Combine(_tempOutputPath, "folder_run1.pdf");
        var outputFile2 = Path.Combine(_tempOutputPath, "folder_run2.pdf");

        // Act
        await GeneratePdfFromFolderAsync(inputFolder, outputFile1);
        await GeneratePdfFromFolderAsync(inputFolder, outputFile2);

        // Assert
        var hash1 = ComputeSha256(outputFile1);
        var hash2 = ComputeSha256(outputFile2);

        hash2.Should().Be(hash1, "Folder input must produce deterministic output");
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("NFR-01", TestType.Integration, Description = "No timestamp/metadata variation")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.Determinism)]
    public async Task Should_Not_Include_Timestamps_Or_Variable_Metadata()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var outputFile1 = Path.Combine(_tempOutputPath, "no_timestamp1.pdf");
        
        // Act - Generate, wait, generate again
        await GeneratePdfAsync(inputFile, outputFile1);
        await Task.Delay(2000); // Wait 2 seconds
        var outputFile2 = Path.Combine(_tempOutputPath, "no_timestamp2.pdf");
        await GeneratePdfAsync(inputFile, outputFile2);

        // Assert - Despite time passing, output must be identical
        var hash1 = ComputeSha256(outputFile1);
        var hash2 = ComputeSha256(outputFile2);

        hash2.Should().Be(hash1, 
            "Output must not include timestamps or any time-dependent metadata (NFR-01)");
    }

    #region Helper Methods

    private async Task GeneratePdfAsync(string inputFile, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call
        throw new NotImplementedException("SysDocs PDF generation not yet implemented");
    }

    private async Task GeneratePdfAsync(string[] inputFiles, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call
        throw new NotImplementedException("SysDocs multi-file PDF generation not yet implemented");
    }

    private async Task GeneratePdfFromFolderAsync(string inputFolder, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call
        throw new NotImplementedException("SysDocs folder-based PDF generation not yet implemented");
    }

    private string ComputeSha256(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
    }

    private string FindSolutionRoot()
    {
        var currentDir = Directory.GetCurrentDirectory();
        while (currentDir != null)
        {
            if (File.Exists(Path.Combine(currentDir, "SysDocs.sln")) ||
                File.Exists(Path.Combine(currentDir, "global.json")))
            {
                return currentDir;
            }
            currentDir = Directory.GetParent(currentDir)?.FullName;
        }
        throw new InvalidOperationException("Could not find solution root directory");
    }

    #endregion
}
