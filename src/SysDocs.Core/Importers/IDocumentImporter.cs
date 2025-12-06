using SysDocs.Core.Model;

namespace SysDocs.Core.Importers;

/// <summary>
/// Interface for document importers.
/// </summary>
public interface IDocumentImporter
{
    /// <summary>
    /// Determines if this importer supports the given file.
    /// </summary>
    bool CanImport(string filePath);
    
    /// <summary>
    /// Imports a document and converts it to the internal model.
    /// </summary>
    Task<Document> ImportAsync(string filePath, CancellationToken cancellationToken = default);
}

/// <summary>
/// Markdown importer implementation.
/// </summary>
public class MarkdownImporter : IDocumentImporter
{
    public bool CanImport(string filePath)
    {
        return filePath.EndsWith(".md", StringComparison.OrdinalIgnoreCase) ||
               filePath.EndsWith(".markdown", StringComparison.OrdinalIgnoreCase);
    }
    
    public async Task<Document> ImportAsync(string filePath, CancellationToken cancellationToken = default)
    {
        // TODO: Implement using Markdig
        var content = await File.ReadAllTextAsync(filePath, cancellationToken);
        
        return new Document
        {
            Title = Path.GetFileNameWithoutExtension(filePath),
            Metadata = new DocumentMetadata
            {
                CreatedAt = File.GetCreationTime(filePath),
                ModifiedAt = File.GetLastWriteTime(filePath)
            }
        };
    }
}
