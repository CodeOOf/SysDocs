using FluentAssertions;
using SysDocs.Tests.Attributes;
using SysDocs.Templates;
using Xunit;

namespace SysDocs.Tests.Unit.Templates;

public class TemplateManagerTests
{
    [Fact(Skip = "Pending TemplateManager implementation")]
    [RequirementTest("FR-05", TestType.Unit, Description = "Load templates")]
    [RequirementTest("FR-06", TestType.Unit, Description = "INCOSE compliance")]
    [TestCategory(TestCategories.Unit)]
    public void TemplateManager_ShouldLoadIncoseTemplate()
    {
        // Arrange
        var manager = new TemplateManager();

        // Act
        var template = manager.LoadTemplate("INCOSE");

        // Assert
        template.Should().NotBeNull();
        template.Name.Should().Be("INCOSE");
    }

    [Fact(Skip = "Pending TemplateManager implementation")]
    [RequirementTest("FR-05", TestType.Unit)]
    [RequirementTest("FR-06", TestType.Unit, Description = "V-Model compliance")]
    [TestCategory(TestCategories.Unit)]
    public void TemplateManager_ShouldLoadVModelTemplate()
    {
        // Arrange
        var manager = new TemplateManager();

        // Act
        var template = manager.LoadTemplate("V-Model");

        // Assert
        template.Should().NotBeNull();
        template.Name.Should().Be("V-Model");
    }

    [Fact(Skip = "Pending TemplateManager implementation")]
    [RequirementTest("FR-05", TestType.Unit)]
    [TestCategory(TestCategories.Unit)]
    public void TemplateManager_ShouldListAvailableTemplates()
    {
        // Arrange
        var manager = new TemplateManager();

        // Act
        // TODO: Implement ListTemplates method
        // var templates = manager.ListTemplates();

        // Assert
        // templates.Should().Contain("INCOSE");
        // templates.Should().Contain("V-Model");
        // templates.Should().Contain("Default");
    }

    [Fact(Skip = "Pending TemplateManager implementation")]
    [RequirementTest("FR-05", TestType.Unit)]
    [TestCategory(TestCategories.Unit)]
    public void TemplateManager_ShouldThrowOnInvalidTemplate()
    {
        // Arrange
        var manager = new TemplateManager();

        // Act & Assert
        var act = () => manager.LoadTemplate("NonExistent");
        act.Should().Throw<ArgumentException>();
    }
}
