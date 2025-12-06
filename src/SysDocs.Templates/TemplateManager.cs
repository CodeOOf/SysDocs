namespace SysDocs.Templates;

/// <summary>
/// Manages SE artifact templates for INCOSE and V-Model compliance.
/// </summary>
public class TemplateManager
{
    /// <summary>
    /// Gets available template names.
    /// </summary>
    public IEnumerable<string> GetAvailableTemplates()
    {
        return new[] { "default", "incose", "vmodel" };
    }
    
    /// <summary>
    /// Loads a template by name.
    /// </summary>
    public Template LoadTemplate(string templateName)
    {
        // TODO: Load from embedded resources
        return new Template
        {
            Name = templateName,
            Description = $"Template: {templateName}"
        };
    }
}

/// <summary>
/// Represents a document template.
/// </summary>
public class Template
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    // TODO: Add template properties (fonts, colors, layouts, etc.)
}
