using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Integration.Formats;

/// <summary>
/// Tests for FR-01: Import documents from multiple formats
/// Uses the examples/adns-project/ test fixture with byte-for-byte verification
/// against expected results in examples/expected-results/
/// </summary>
public class FR01_MultiFormatImportTests : IDisposable
{
    private readonly string _testProjectPath;
    private readonly string _expectedResultsPath;
    private readonly string _tempOutputPath;

    public FR01_MultiFormatImportTests()
    {
        // Paths to test fixtures
        var solutionRoot = FindSolutionRoot();
        _testProjectPath = Path.Combine(solutionRoot, "examples", "adns-project");
        _expectedResultsPath = Path.Combine(solutionRoot, "examples", "expected-results");
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

    [Fact]
    [RequirementTest("FR-01", TestType.Integration, Description = "Markdown format import")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC01_Should_Import_Markdown_Format()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var outputFile = Path.Combine(_tempOutputPath, "01_markdown.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "01_markdown.pdf");

        // Skip if expected result doesn't exist yet
        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}. Run: make generate-expected-results");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash, 
            "Generated PDF must be byte-for-byte identical to expected result (FR-06, NFR-01)");
    }

    [Fact]
    [RequirementTest("FR-01", TestType.Integration, Description = "LaTeX format import with equations")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC02_Should_Import_LaTeX_Format()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "21_SDD_ADNS_Software.tex");
        var outputFile = Path.Combine(_tempOutputPath, "02_latex.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "02_latex.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from LaTeX");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "LaTeX import must produce deterministic output with equations rendered correctly");
    }

    [Fact]
    [RequirementTest("FR-01", TestType.Integration, Description = "Plain text format import")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC03_Should_Import_PlainText_Format()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "22_HDD_ADNS_Hardware.txt");
        var outputFile = Path.Combine(_tempOutputPath, "03_plaintext.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "03_plaintext.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from plain text");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "Plain text import must preserve ASCII diagrams and structure");
    }

    [Fact(Skip = "CSV import not yet implemented")]
    [RequirementTest("FR-01", TestType.Integration, Description = "CSV format import")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC04_Should_Import_CSV_Format()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "requirements_export.csv");
        var outputFile = Path.Combine(_tempOutputPath, "04_csv.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "04_csv.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from CSV");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "CSV import must convert tabular data to PDF tables");
    }

    [Fact(Skip = "JSON import not yet implemented")]
    [RequirementTest("FR-01", TestType.Integration, Description = "JSON format import")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC05_Should_Import_JSON_Format()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "13_RTM_ADNS.json");
        var outputFile = Path.Combine(_tempOutputPath, "05_json.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "05_json.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from JSON");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "JSON import must structure data appropriately in PDF");
    }

    [Fact(Skip = "XML import not yet implemented")]
    [RequirementTest("FR-01", TestType.Integration, Description = "XML format import")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC06_Should_Import_XML_Format()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "project_metadata.xml");
        var outputFile = Path.Combine(_tempOutputPath, "06_xml.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "06_xml.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from XML");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "XML import must preserve hierarchical structure");
    }

    [Fact(Skip = "Mixed format import not yet implemented")]
    [RequirementTest("FR-01", TestType.Integration, Description = "Multiple input formats in single PDF")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC07_Should_Import_Mixed_Formats()
    {
        // Arrange
        var inputFiles = new[]
        {
            Path.Combine(_testProjectPath, "12_SRS_ADNS.md"),
            Path.Combine(_testProjectPath, "requirements_export.csv")
        };
        var outputFile = Path.Combine(_tempOutputPath, "07_mixed.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "07_mixed.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFiles, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from multiple formats");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "Mixed format import must handle multiple input types correctly");
    }

    [Fact(Skip = "Folder input not yet implemented")]
    [RequirementTest("FR-01", TestType.Integration, Description = "Entire folder as input")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC08_Should_Import_Entire_Folder()
    {
        // Arrange
        var inputFolder = _testProjectPath;
        var outputFile = Path.Combine(_tempOutputPath, "complete.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "folder", "complete.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfFromFolderAsync(inputFolder, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue("PDF should be generated from entire folder");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            "Folder import must process all files and produce deterministic output");
        
        // Additional verification: comprehensive document expected
        var fileInfo = new FileInfo(outputFile);
        fileInfo.Length.Should().BeGreaterThan(100_000, 
            "Complete project PDF should be substantial (50+ pages expected)");
    }

    #region Helper Methods

    private async Task GeneratePdfAsync(string inputFile, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call
        // For now, this is a placeholder that will be implemented
        throw new NotImplementedException(
            "SysDocs PDF generation not yet implemented. " +
            "This test verifies the contract and expected behavior.");
    }

    private async Task GeneratePdfAsync(string[] inputFiles, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call for multiple inputs
        throw new NotImplementedException(
            "SysDocs multi-file PDF generation not yet implemented.");
    }

    private async Task GeneratePdfFromFolderAsync(string inputFolder, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call for folder input
        throw new NotImplementedException(
            "SysDocs folder-based PDF generation not yet implemented.");
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

/// <summary>
/// Custom exception to skip tests when expected results aren't available yet
/// </summary>
public class SkipException : Exception
{
    public SkipException(string message) : base(message) { }
}
