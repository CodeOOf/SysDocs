using SysDocs.Core.Model;

namespace SysDocs.Core.Exporters;

/// <summary>
/// Interface for document exporters.
/// </summary>
public interface IDocumentExporter
{
    /// <summary>
    /// Exports a document to the specified output path.
    /// </summary>
    Task ExportAsync(Document document, string outputPath, CancellationToken cancellationToken = default);
}

/// <summary>
/// PDF exporter using QuestPDF for deterministic output.
/// </summary>
public class PdfExporter : IDocumentExporter
{
    public async Task ExportAsync(Document document, string outputPath, CancellationToken cancellationToken = default)
    {
        // TODO: Implement using QuestPDF
        await Task.Run(() =>
        {
            // Placeholder - will implement deterministic PDF generation
            var directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            File.WriteAllText(outputPath, $"PDF Export Placeholder for: {document.Title}");
        }, cancellationToken);
    }
}
