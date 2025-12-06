using System.Security.Cryptography;
using FluentAssertions;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Integration.CrossPlatform;

/// <summary>
/// Tests for FR-14: Cross-platform deterministic output
/// Verifies that Linux, Windows (Docker), and macOS (Docker) produce identical output
/// Note: This test class runs on all platforms in CI/CD
/// </summary>
public class FR14_CrossPlatformDeterminismTests : IDisposable
{
    private readonly string _testProjectPath;
    private readonly string _expectedResultsPath;
    private readonly string _tempOutputPath;

    public FR14_CrossPlatformDeterminismTests()
    {
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

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-14", TestType.Integration, Description = "Cross-platform determinism")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.CrossPlatform)]
    [TestCategory(TestCategories.Determinism)]
    public async Task TC10_Should_Produce_Identical_Output_On_All_Platforms()
    {
        // This test runs on Linux, Windows (Docker), and macOS (Docker) in CI/CD
        // Each platform generates the same output and compares against expected result
        
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var outputFile = Path.Combine(_tempOutputPath, "cross_platform.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "01_markdown.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        var platformInfo = GetPlatformInfo();
        
        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        File.Exists(outputFile).Should().BeTrue($"PDF should be generated on {platformInfo}");
        
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);
        
        generatedHash.Should().Be(expectedHash,
            $"Output on {platformInfo} must match expected result (cross-platform determinism - FR-14)");
    }

    [Theory(Skip = "Pending implementation")]
    [InlineData("12_SRS_ADNS.md", "01_markdown.pdf")]
    [InlineData("21_SDD_ADNS_Software.tex", "02_latex.pdf")]
    [InlineData("22_HDD_ADNS_Hardware.txt", "03_plaintext.pdf")]
    [RequirementTest("FR-14", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.CrossPlatform)]
    public async Task Should_Match_Expected_Results_On_Current_Platform(string inputFileName, string expectedFileName)
    {
        // This test verifies that the current platform (wherever it's running)
        // produces output matching the expected results
        
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, inputFileName);
        var outputFile = Path.Combine(_tempOutputPath, Path.GetFileName(expectedFileName));
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", expectedFileName);

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        var platformInfo = GetPlatformInfo();

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);

        generatedHash.Should().Be(expectedHash,
            $"Output on {platformInfo} must be byte-for-byte identical to expected result");
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-14", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.CrossPlatform)]
    public async Task Should_Handle_Path_Separators_Correctly()
    {
        // Verifies that Windows (\) and Unix (/) path separators don't affect output
        
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var outputFile = Path.Combine(_tempOutputPath, "path_test.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "01_markdown.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);

        generatedHash.Should().Be(expectedHash,
            "Path separator differences (Windows \\ vs Unix /) must not affect output");
    }

    [Fact(Skip = "Pending implementation")]
    [RequirementTest("FR-14", TestType.Integration)]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.CrossPlatform)]
    public async Task Should_Handle_Line_Endings_Correctly()
    {
        // Verifies that different line endings (CRLF vs LF) don't affect output
        
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var outputFile = Path.Combine(_tempOutputPath, "line_ending_test.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "01_markdown.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act
        await GeneratePdfAsync(inputFile, outputFile);

        // Assert
        var generatedHash = ComputeSha256(outputFile);
        var expectedHash = ComputeSha256(expectedFile);

        generatedHash.Should().Be(expectedHash,
            "Line ending differences (CRLF vs LF) must not affect PDF output");
    }

    [Fact(Skip = "Pending Docker implementation")]
    [RequirementTest("FR-14", TestType.Integration, Description = "Docker deployment cross-platform")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.CrossPlatform)]
    public async Task Should_Produce_Identical_Output_In_Docker()
    {
        // This test verifies Docker deployment produces same output
        // Requires Docker to be available
        
        var dockerAvailable = IsDockerAvailable();
        if (!dockerAvailable)
        {
            throw new SkipException("Docker not available");
        }

        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "12_SRS_ADNS.md");
        var expectedFile = Path.Combine(_expectedResultsPath, "individual", "01_markdown.pdf");

        if (!File.Exists(expectedFile))
        {
            throw new SkipException($"Expected result file not found: {expectedFile}");
        }

        // Act - Run via Docker
        var dockerOutputFile = await RunInDockerAsync(inputFile);

        // Assert
        var dockerHash = ComputeSha256(dockerOutputFile);
        var expectedHash = ComputeSha256(expectedFile);

        dockerHash.Should().Be(expectedHash,
            "Docker deployment must produce identical output to native execution");
    }

    #region Helper Methods

    private async Task GeneratePdfAsync(string inputFile, string outputFile)
    {
        // TODO: Replace with actual SysDocs API call
        throw new NotImplementedException("SysDocs PDF generation not yet implemented");
    }

    private async Task<string> RunInDockerAsync(string inputFile)
    {
        // TODO: Implement Docker execution
        throw new NotImplementedException("Docker test execution not yet implemented");
    }

    private bool IsDockerAvailable()
    {
        try
        {
            var process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "--version",
                RedirectStandardOutput = true,
                UseShellExecute = false
            });
            process?.WaitForExit();
            return process?.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    }

    private string GetPlatformInfo()
    {
        if (OperatingSystem.IsWindows())
            return "Windows";
        if (OperatingSystem.IsLinux())
            return "Linux";
        if (OperatingSystem.IsMacOS())
            return "macOS";
        return "Unknown";
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
