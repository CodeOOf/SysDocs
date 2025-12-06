namespace SysDocs.Core.Model;

/// <summary>
/// Represents the internal document object model.
/// All imported documents are converted to this unified structure.
/// </summary>
public class Document
{
    /// <summary>
    /// Unique identifier for the document.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Document title.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Document metadata.
    /// </summary>
    public DocumentMetadata Metadata { get; set; } = new();
    
    /// <summary>
    /// Root sections of the document.
    /// </summary>
    public List<Section> Sections { get; set; } = new();
}

/// <summary>
/// Document metadata for traceability and qualification.
/// </summary>
public class DocumentMetadata
{
    public string Author { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public string Version { get; set; } = "1.0.0";
    public string GitCommit { get; set; } = string.Empty;
    public string GitBranch { get; set; } = string.Empty;
    public Dictionary<string, string> CustomProperties { get; set; } = new();
}

/// <summary>
/// Represents a document section (hierarchical).
/// </summary>
public class Section
{
    public string Title { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public List<Block> Content { get; set; } = new();
    public List<Section> Subsections { get; set; } = new();
}

/// <summary>
/// Base class for content blocks.
/// </summary>
public abstract class Block
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

/// <summary>
/// Paragraph block.
/// </summary>
public class Paragraph : Block
{
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// Image block.
/// </summary>
public class Image : Block
{
    public string Path { get; set; } = string.Empty;
    public string AltText { get; set; } = string.Empty;
    public byte[]? Data { get; set; }
}

/// <summary>
/// Table block.
/// </summary>
public class Table : Block
{
    public List<TableRow> Rows { get; set; } = new();
}

/// <summary>
/// Table row.
/// </summary>
public class TableRow
{
    public List<TableCell> Cells { get; set; } = new();
}

/// <summary>
/// Table cell.
/// </summary>
public class TableCell
{
    public string Content { get; set; } = string.Empty;
}
