using FluentAssertions;
using SysDocs.Core.Model;
using SysDocs.Tests.Attributes;
using Xunit;

namespace SysDocs.Tests.Unit.Model;

public class DocumentTests
{
    [Fact]
    [RequirementTest("FR-01", TestType.Unit, Description = "Document model supports multiple input formats")]
    [RequirementTest("FR-15", TestType.Unit, Description = "Document has stable identifiers")]
    [TestCategory(TestCategories.Unit)]
    public void Document_ShouldHaveStableIdentifier()
    {
        // Arrange & Act
        var doc = new Document 
        { 
            Title = "Test Document",
            Metadata = new DocumentMetadata { Version = "1.0" }
        };

        // Assert
        doc.Id.Should().NotBeEmpty();
        doc.Title.Should().Be("Test Document");
        doc.Metadata.Version.Should().Be("1.0");
    }

    [Fact]
    [RequirementTest("FR-01", TestType.Unit)]
    [RequirementTest("FR-14", TestType.Unit, Description = "Document supports metadata")]
    [TestCategory(TestCategories.Unit)]
    public void Document_ShouldSupportMetadata()
    {
        // Arrange & Act
        var doc = new Document
        {
            Title = "Requirements Spec",
            Metadata = new DocumentMetadata
            {
                Author = "John Doe",
                Version = "1.0",
                CustomProperties = new Dictionary<string, string>
                {
                    ["Department"] = "Engineering",
                    ["Classification"] = "Internal"
                }
            }
        };

        // Assert
        doc.Metadata.Author.Should().Be("John Doe");
        doc.Metadata.CustomProperties.Should().ContainKey("Department");
        doc.Metadata.CustomProperties["Department"].Should().Be("Engineering");
        doc.Metadata.CustomProperties.Should().HaveCount(2);
    }

    [Fact]
    [RequirementTest("FR-01", TestType.Unit)]
    [TestCategory(TestCategories.Unit)]
    public void Document_ShouldSupportHierarchicalSections()
    {
        // Arrange
        var doc = new Document { Title = "System Design" };
        var section1 = new Section { Title = "Introduction", Level = 1 };
        var section2 = new Section { Title = "Architecture", Level = 1 };
        var subsection = new Section { Title = "Component Design", Level = 2 };

        // Act
        doc.Sections.Add(section1);
        doc.Sections.Add(section2);
        section2.Subsections.Add(subsection);

        // Assert
        doc.Sections.Should().HaveCount(2);
        doc.Sections[1].Subsections.Should().HaveCount(1);
        doc.Sections[1].Subsections[0].Title.Should().Be("Component Design");
    }

    [Fact]
    [RequirementTest("FR-01", TestType.Unit)]
    [RequirementTest("FR-10", TestType.Unit, Description = "Tables supported in document model")]
    [TestCategory(TestCategories.Unit)]
    public void Section_ShouldSupportMixedContentTypes()
    {
        // Arrange
        var section = new Section { Title = "Test Section", Level = 1 };

        // Act
        section.Content.Add(new Paragraph 
        { 
            Text = "This is a paragraph." 
        });
        
        section.Content.Add(new Image 
        { 
            Path = "/images/diagram.png",
            AltText = "System Diagram" 
        });
        
        section.Content.Add(new Table 
        { 
            Rows = new List<TableRow>
            {
                new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Content = "ID" },
                        new TableCell { Content = "Requirement" },
                        new TableCell { Content = "Status" }
                    }
                },
                new TableRow
                {
                    Cells = new List<TableCell>
                    {
                        new TableCell { Content = "FR-01" },
                        new TableCell { Content = "Import documents" },
                        new TableCell { Content = "Implemented" }
                    }
                }
            }
        });

        // Assert
        section.Content.Should().HaveCount(3);
        section.Content.OfType<Paragraph>().Should().HaveCount(1);
        section.Content.OfType<Image>().Should().HaveCount(1);
        section.Content.OfType<Table>().Should().HaveCount(1);
    }

    [Fact]
    [RequirementTest("FR-14", TestType.Unit, Description = "Git metadata tracking")]
    [TestCategory(TestCategories.Unit)]
    public void Document_ShouldSupportGitMetadata()
    {
        // Arrange & Act
        var doc = new Document
        {
            Title = "Test",
            Metadata = new DocumentMetadata
            {
                GitCommit = "abc123def456",
                GitBranch = "main",
                Author = "John Doe"
            }
        };

        // Assert
        doc.Metadata.Should().NotBeNull();
        doc.Metadata.GitCommit.Should().Be("abc123def456");
        doc.Metadata.GitBranch.Should().Be("main");
    }
}
