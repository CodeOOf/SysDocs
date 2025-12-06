using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Integration.SysML;

/// <summary>
/// Tests for FR-20 and FR-21: Import SysML v2 Requirements and Use Cases
/// Validates mapping to Stakeholder and System Requirements documentation
/// </summary>
public class FR20_FR21_SysMLRequirementsUseCasesTests : IDisposable
{
    private readonly string _testProjectPath;
    private readonly string _expectedResultsPath;
    private readonly string _tempOutputPath;

    public FR20_FR21_SysMLRequirementsUseCasesTests()
    {
        var solutionRoot = FindSolutionRoot();
        _testProjectPath = Path.Combine(solutionRoot, "examples", "sysml-v2-project");
        _expectedResultsPath = Path.Combine(solutionRoot, "examples", "expected-results", "sysml");
        _tempOutputPath = Path.Combine(Path.GetTempPath(), $"sysdocs-test-sysml-{Guid.NewGuid()}");
        Directory.CreateDirectory(_tempOutputPath);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempOutputPath))
        {
            Directory.Delete(_tempOutputPath, recursive: true);
        }
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-20", TestType.Integration, Description = "Import SysML v2 Requirements as Stakeholder Requirements")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    public async Task TC01_Should_Import_SysMLv2_Requirements_As_Stakeholder_Requirements()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "stakeholder_requirements.sysml");
        var outputFile = Path.Combine(_tempOutputPath, "stakeholder_requirements.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "stakeholder_requirements.pdf");

        // Act
        var result = await ImportSysMLRequirements(inputFile, outputFile, DocumentType.StakeholderRequirements);

        // Assert
        result.Success.Should().BeTrue("SysML v2 requirements import should succeed");
        File.Exists(outputFile).Should().BeTrue("Output PDF should be created");
        
        // Verify deterministic output
        var actualHash = ComputeFileHash(outputFile);
        var expectedHash = ComputeFileHash(expectedFile);
        actualHash.Should().Be(expectedHash, "Output should be byte-for-byte identical to expected result");
        
        // Verify requirement extraction
        result.ExtractedRequirements.Should().NotBeEmpty("Should extract requirements from SysML v2");
        result.ExtractedRequirements.Should().AllSatisfy(req => 
        {
            req.Id.Should().NotBeNullOrEmpty("Each requirement should have an ID");
            req.Text.Should().NotBeNullOrEmpty("Each requirement should have text");
            req.Type.Should().Be("StakeholderRequirement");
        });
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-20", TestType.Integration, Description = "Import SysML v2 Requirements as System Requirements")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    public async Task TC02_Should_Import_SysMLv2_Requirements_As_System_Requirements()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "system_requirements.sysml");
        var outputFile = Path.Combine(_tempOutputPath, "system_requirements.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "system_requirements.pdf");

        // Act
        var result = await ImportSysMLRequirements(inputFile, outputFile, DocumentType.SystemRequirements);

        // Assert
        result.Success.Should().BeTrue("SysML v2 requirements import should succeed");
        File.Exists(outputFile).Should().BeTrue("Output PDF should be created");
        
        var actualHash = ComputeFileHash(outputFile);
        var expectedHash = ComputeFileHash(expectedFile);
        actualHash.Should().Be(expectedHash, "Output should be byte-for-byte identical");
        
        // Verify system requirements have correct attributes
        result.ExtractedRequirements.Should().AllSatisfy(req =>
        {
            req.Type.Should().Be("SystemRequirement");
            req.VerificationMethod.Should().NotBeNullOrEmpty("System requirements should specify verification method");
        });
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-21", TestType.Integration, Description = "Import SysML v2 Use Cases as Stakeholder Requirements")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    public async Task TC03_Should_Import_SysMLv2_UseCases_As_Stakeholder_Requirements()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "stakeholder_use_cases.sysml");
        var outputFile = Path.Combine(_tempOutputPath, "stakeholder_use_cases.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "stakeholder_use_cases.pdf");

        // Act
        var result = await ImportSysMLUseCases(inputFile, outputFile, DocumentType.StakeholderRequirements);

        // Assert
        result.Success.Should().BeTrue("SysML v2 use case import should succeed");
        File.Exists(outputFile).Should().BeTrue("Output PDF should be created");
        
        var actualHash = ComputeFileHash(outputFile);
        var expectedHash = ComputeFileHash(expectedFile);
        actualHash.Should().Be(expectedHash, "Output should be byte-for-byte identical");
        
        // Verify use case extraction
        result.ExtractedUseCases.Should().NotBeEmpty("Should extract use cases from SysML v2");
        result.ExtractedUseCases.Should().AllSatisfy(uc =>
        {
            uc.Name.Should().NotBeNullOrEmpty("Each use case should have a name");
            uc.Actors.Should().NotBeEmpty("Each use case should have actors");
            uc.Preconditions.Should().NotBeNull("Each use case should define preconditions");
            uc.Postconditions.Should().NotBeNull("Each use case should define postconditions");
        });
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-21", TestType.Integration, Description = "Import SysML v2 Use Cases as System Requirements")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    public async Task TC04_Should_Import_SysMLv2_UseCases_As_System_Requirements()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "system_use_cases.sysml");
        var outputFile = Path.Combine(_tempOutputPath, "system_use_cases.pdf");
        var expectedFile = Path.Combine(_expectedResultsPath, "system_use_cases.pdf");

        // Act
        var result = await ImportSysMLUseCases(inputFile, outputFile, DocumentType.SystemRequirements);

        // Assert
        result.Success.Should().BeTrue("SysML v2 use case import should succeed");
        File.Exists(outputFile).Should().BeTrue("Output PDF should be created");
        
        var actualHash = ComputeFileHash(outputFile);
        var expectedHash = ComputeFileHash(expectedFile);
        actualHash.Should().Be(expectedHash, "Output should be byte-for-byte identical");
        
        // Verify system-level use case attributes
        result.ExtractedUseCases.Should().AllSatisfy(uc =>
        {
            uc.SystemBoundary.Should().NotBeNullOrEmpty("System use cases should define system boundary");
            uc.SystemFunctions.Should().NotBeEmpty("System use cases should reference system functions");
        });
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-24", TestType.Integration, Description = "Preserve traceability links from SysML v2")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    [TestCategory(TestCategories.Traceability)]
    public async Task TC05_Should_Preserve_SysMLv2_Traceability_Links()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "requirements_with_traces.sysml");
        var outputFile = Path.Combine(_tempOutputPath, "requirements_traced.pdf");

        // Act
        var result = await ImportSysMLRequirements(inputFile, outputFile, DocumentType.SystemRequirements);

        // Assert
        result.Success.Should().BeTrue("Import with traceability should succeed");
        
        // Verify traceability preservation
        result.TraceLinks.Should().NotBeEmpty("Should preserve traceability links");
        result.TraceLinks.Should().AllSatisfy(trace =>
        {
            trace.SourceId.Should().NotBeNullOrEmpty("Trace link should have source ID");
            trace.TargetId.Should().NotBeNullOrEmpty("Trace link should have target ID");
            trace.RelationType.Should().BeOneOf("satisfies", "refines", "derives", "verifies");
        });
        
        // Verify bidirectional traceability
        var requirementIds = result.ExtractedRequirements.Select(r => r.Id).ToHashSet();
        result.TraceLinks.Should().AllSatisfy(trace =>
        {
            requirementIds.Should().Contain(trace.SourceId, "Source ID should reference an extracted requirement");
        });
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-25", TestType.Integration, Description = "Support multiple SysML v2 file formats")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    [TestCategory(TestCategories.FileFormat)]
    public async Task TC06_Should_Support_Multiple_SysMLv2_File_Formats()
    {
        // Arrange - Test .sysml format
        var sysmlFile = Path.Combine(_testProjectPath, "requirements.sysml");
        var sysmlOutput = Path.Combine(_tempOutputPath, "from_sysml.pdf");
        
        // Arrange - Test .kerml format
        var kermlFile = Path.Combine(_testProjectPath, "requirements.kerml");
        var kermlOutput = Path.Combine(_tempOutputPath, "from_kerml.pdf");
        
        // Arrange - Test .json format (SysML v2 API)
        var jsonFile = Path.Combine(_testProjectPath, "requirements.json");
        var jsonOutput = Path.Combine(_tempOutputPath, "from_json.pdf");

        // Act
        var sysmlResult = await ImportSysMLRequirements(sysmlFile, sysmlOutput, DocumentType.SystemRequirements);
        var kermlResult = await ImportSysMLRequirements(kermlFile, kermlOutput, DocumentType.SystemRequirements);
        var jsonResult = await ImportSysMLRequirements(jsonFile, jsonOutput, DocumentType.SystemRequirements);

        // Assert - All formats should be supported
        sysmlResult.Success.Should().BeTrue("Should support .sysml format");
        kermlResult.Success.Should().BeTrue("Should support .kerml format");
        jsonResult.Success.Should().BeTrue("Should support .json format");
        
        // Verify all outputs are created
        File.Exists(sysmlOutput).Should().BeTrue();
        File.Exists(kermlOutput).Should().BeTrue();
        File.Exists(jsonOutput).Should().BeTrue();
        
        // Verify same content from different formats produces deterministic output
        var sysmlHash = ComputeFileHash(sysmlOutput);
        var kermlHash = ComputeFileHash(kermlOutput);
        var jsonHash = ComputeFileHash(jsonOutput);
        
        sysmlHash.Should().Be(kermlHash, "Equivalent content from .sysml and .kerml should produce identical output");
        sysmlHash.Should().Be(jsonHash, "Equivalent content from .sysml and .json should produce identical output");
    }

    [Fact(Skip = "Pending SysML v2 importer implementation")]
    [RequirementTest("FR-20,FR-24,NFR-01", TestType.Integration, Description = "SysML v2 import produces deterministic output")]
    [TestCategory(TestCategories.Integration)]
    [TestCategory(TestCategories.SysML)]
    [TestCategory(TestCategories.Determinism)]
    public async Task TC07_SysMLv2_Import_Should_Be_Deterministic()
    {
        // Arrange
        var inputFile = Path.Combine(_testProjectPath, "system_requirements.sysml");
        var output1 = Path.Combine(_tempOutputPath, "determinism_test_1.pdf");
        var output2 = Path.Combine(_tempOutputPath, "determinism_test_2.pdf");

        // Act - Import twice
        var result1 = await ImportSysMLRequirements(inputFile, output1, DocumentType.SystemRequirements);
        await Task.Delay(100); // Small delay to ensure different execution context
        var result2 = await ImportSysMLRequirements(inputFile, output2, DocumentType.SystemRequirements);

        // Assert
        result1.Success.Should().BeTrue();
        result2.Success.Should().BeTrue();
        
        var hash1 = ComputeFileHash(output1);
        var hash2 = ComputeFileHash(output2);
        
        hash1.Should().Be(hash2, "Multiple imports of same SysML v2 file should produce identical output");
    }

    #region Helper Methods

    private string FindSolutionRoot()
    {
        var directory = Directory.GetCurrentDirectory();
        while (directory != null)
        {
            if (File.Exists(Path.Combine(directory, "SysDocs.sln")))
                return directory;
            directory = Directory.GetParent(directory)?.FullName;
        }
        throw new InvalidOperationException("Could not find solution root");
    }

    private string ComputeFileHash(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hash = sha256.ComputeHash(stream);
        return Convert.ToHexString(hash);
    }

    private async Task<SysMLImportResult> ImportSysMLRequirements(string inputFile, string outputFile, DocumentType docType)
    {
        // Placeholder for actual SysML v2 importer implementation
        await Task.CompletedTask;
        throw new NotImplementedException("SysML v2 importer not yet implemented");
    }

    private async Task<SysMLImportResult> ImportSysMLUseCases(string inputFile, string outputFile, DocumentType docType)
    {
        // Placeholder for actual SysML v2 use case importer implementation
        await Task.CompletedTask;
        throw new NotImplementedException("SysML v2 use case importer not yet implemented");
    }

    #endregion

    #region Supporting Types (will be moved to actual implementation)

    private enum DocumentType
    {
        StakeholderRequirements,
        SystemRequirements
    }

    private class SysMLImportResult
    {
        public bool Success { get; set; }
        public List<SysMLRequirement> ExtractedRequirements { get; set; } = new();
        public List<SysMLUseCase> ExtractedUseCases { get; set; } = new();
        public List<TraceLink> TraceLinks { get; set; } = new();
    }

    private class SysMLRequirement
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string VerificationMethod { get; set; } = string.Empty;
    }

    private class SysMLUseCase
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Actors { get; set; } = new();
        public string Preconditions { get; set; } = string.Empty;
        public string Postconditions { get; set; } = string.Empty;
        public string SystemBoundary { get; set; } = string.Empty;
        public List<string> SystemFunctions { get; set; } = new();
    }

    private class TraceLink
    {
        public string SourceId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public string RelationType { get; set; } = string.Empty;
    }

    #endregion
}
