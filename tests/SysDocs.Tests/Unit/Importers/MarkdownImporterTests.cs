using FluentAssertions;
using SysDocs.Core.Importers;
using SysDocs.Core.Model;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Unit.Importers;

public class MarkdownImporterTests
{
    private readonly IDocumentImporter _importer;

    public MarkdownImporterTests()
    {
        // TODO: Initialize actual MarkdownImporter once implemented
        _importer = null!;
    }

    [Fact(Skip = "Pending MarkdownImporter implementation")]
    [RequirementTest("FR-01", TestType.Unit, Description = "Import Markdown documents")]
    [RequirementTest("FR-03", TestType.Unit, Description = "Markdown parsing with Markdig")]
    [TestCategory(TestCategories.Unit)]
    public async Task MarkdownImporter_ShouldParseBasicMarkdown()
    {
        // Arrange
        var markdown = """
            # Test Document
            
            This is a paragraph.
            
            ## Section 1
            
            Content here.
            """;

        // Act
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, markdown);
        var document = await _importer.ImportAsync(tempFile);

        // Assert
        document.Should().NotBeNull();
        document.Title.Should().Be("Test Document");
        document.Sections.Should().HaveCount(1);
        document.Sections[0].Title.Should().Be("Section 1");
    }

    [Fact(Skip = "Pending MarkdownImporter implementation")]
    [RequirementTest("FR-10", TestType.Unit, Description = "Markdown tables")]
    [TestCategory(TestCategories.Unit)]
    public async Task MarkdownImporter_ShouldParseTables()
    {
        // Arrange
        var markdown = """
            # Document
            
            | ID | Name | Status |
            |---|---|---|
            | 1 | Item A | Active |
            | 2 | Item B | Pending |
            """;

        // Act
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, markdown);
        var document = await _importer.ImportAsync(tempFile);

        // Assert
        var table = document.Sections[0].Content.OfType<Table>().FirstOrDefault();
        table.Should().NotBeNull();
        table!.Rows.Should().HaveCount(3); // Header row + 2 data rows
    }

    [Fact(Skip = "Pending MarkdownImporter implementation")]
    [RequirementTest("FR-01", TestType.Unit)]
    [RequirementTest("FR-10", TestType.Unit, Description = "Markdown images")]
    [TestCategory(TestCategories.Unit)]
    public async Task MarkdownImporter_ShouldParseImages()
    {
        // Arrange
        var markdown = """
            # Document
            
            ![Architecture Diagram](./diagrams/architecture.png)
            """;

        // Act
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, markdown);
        var document = await _importer.ImportAsync(tempFile);

        // Assert
        var image = document.Sections[0].Content.OfType<Image>().FirstOrDefault();
        image.Should().NotBeNull();
        image!.Path.Should().Contain("architecture.png");
        image.AltText.Should().Be("Architecture Diagram");
    }

    [Fact(Skip = "Pending MarkdownImporter implementation")]
    [RequirementTest("FR-01", TestType.Unit)]
    [TestCategory(TestCategories.Unit)]
    public async Task MarkdownImporter_ShouldHandleNestedLists()
    {
        // Arrange
        var markdown = """
            # Document
            
            - Item 1
              - Subitem 1.1
              - Subitem 1.2
            - Item 2
            """;

        // Act
        var tempFile = Path.GetTempFileName();
        File.WriteAllText(tempFile, markdown);
        var document = await _importer.ImportAsync(tempFile);

        // Assert
        document.Sections[0].Content.Should().NotBeEmpty();
        // TODO: Verify list structure once model supports lists
    }
}
