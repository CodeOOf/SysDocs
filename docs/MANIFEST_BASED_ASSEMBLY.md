# Manifest-Based Document Assembly

**Requirements**: FR-16, FR-17, FR-18, FR-19  
**Version**: 1.0.0  
**Status**: Specification Complete, Implementation Pending

---

## Overview

SysDocs supports **manifest-based document assembly**, enabling complex SE documents to be composed from distributed markdown files across a git repository structure. This feature bridges the gap between modern git repository documentation practices and formal SE documentation requirements.

## Use Cases

### Traditional Approach (adns-project)
```
01_SEMP_ADNS.md         → 01_SEMP_ADNS.pdf
12_SRS_ADNS.md          → 12_SRS_ADNS.pdf
```
- One source file → One PDF document
- Simple, direct mapping
- Traditional SE documentation structure

### Modern Git-Repo Approach (skynet-repo)
```
README.md (sections 1.1-1.10)           ┐
project_description.md (sections 2.1-2.8) ├→ 01_SEMP_SkyNet.pdf
docs/engineering_process.md (3.1-3.4)   ┘
```
- Multiple source files → One PDF document
- Documentation lives in repository structure (README, docs/, etc.)
- Formal SE document assembled from distributed sources

## Manifest File Format

### Basic Structure

```json
{
  "$schema": "https://sysdocs.example.com/schemas/manifest/v1.0.0",
  "version": "1.0.0",
  "project": {
    "name": "Project Name",
    "shortName": "PROJ",
    "version": "1.0.0",
    "date": "2025-12-06"
  },
  "documents": [
    {
      "outputName": "01_SEMP_Project.pdf",
      "title": "Systems Engineering Management Plan",
      "standard": "ISO/IEC 15288",
      "type": "management",
      "sources": [...]
    }
  ],
  "templates": {...},
  "metadata": {...}
}
```

### Document Definition

```json
{
  "outputName": "01_SEMP_SkyNet.pdf",
  "title": "Systems Engineering Management Plan",
  "standard": "ISO/IEC 15288",
  "type": "management",
  "sources": [
    {
      "file": "README.md",
      "type": "markdown",
      "sections": ["1.1", "1.2", "1.3"],
      "sectionMapping": {
        "1.1": "1. Project Scope",
        "1.2": "1. Project Overview",
        "1.3": "1. Stakeholders"
      }
    }
  ]
}
```

### Source File Types

- **Single file, all content**: `"sections": "all"`
- **Specific sections**: `"sections": ["1.1", "1.2", "1.3"]`
- **Multiple files**: Array of source objects
- **Mixed formats**: Different file types in same document

## Section Extraction

### Heading Pattern Matching

The manifest defines how to extract sections:

```json
{
  "sectionExtraction": {
    "enabled": true,
    "strategy": "heading-based",
    "headingPattern": "^#{1,6}\\s+(\\d+\\.\\d+)\\s+(.+)$"
  }
}
```

**Example Source Markdown**:
```markdown
## 1.1 Project Scope

The project scope includes...

## 1.2 System Overview

The system provides...
```

**Extraction**: Pattern matches `## 1.1 Project Scope` and extracts that section

### Section Remapping

Original section numbers can be remapped in the output:

```json
{
  "sectionMapping": {
    "1.1": "1. Project Scope",    // 1.1 becomes Section 1
    "1.2": "2. System Overview"    // 1.2 becomes Section 2
  }
}
```

## Multi-File Composition

### Assembly Process

1. **Parse Manifest**: Read document definitions
2. **Extract Sections**: For each source file, extract specified sections
3. **Compose Document**: Merge sections in specified order
4. **Renumber**: Apply section remapping if specified
5. **Render PDF**: Generate final document

### Example: SEMP from 3 Files

**Manifest**:
```json
{
  "sources": [
    {"file": "README.md", "sections": ["1.1", "1.2", "1.3"]},
    {"file": "project_description.md", "sections": ["2.1", "2.2"]},
    {"file": "docs/engineering_process.md", "sections": ["3.1", "3.2"]}
  ]
}
```

**Result**: Single SEMP PDF containing sections from all three files in order

## Determinism (FR-19)

Manifest-based assembly maintains the same determinism guarantees as direct file processing:

- ✅ **Repeated runs**: Same manifest + same sources → identical PDF hash
- ✅ **Cross-platform**: Linux/Windows/macOS produce identical output
- ✅ **Section order**: Deterministic section ordering
- ✅ **Content**: No timestamps, no random elements

## Template Configuration

Manifests can specify document templates:

```json
{
  "templates": {
    "default": "standard-se-document",
    "coverPage": true,
    "toc": true,
    "headers": true,
    "footers": true,
    "watermark": "DRAFT"
  }
}
```

Applied to all documents unless overridden per-document.

## Example Projects

### Example 1: ADNS Project (Simple Manifest)

**Location**: `examples/adns-project/sysdocs.manifest.json`

**Pattern**: One-to-one mapping (each SE doc is one file)

```json
{
  "documents": [
    {
      "outputName": "01_SEMP_ADNS.pdf",
      "sources": [{"file": "01_SEMP_ADNS.md", "sections": "all"}]
    }
  ]
}
```

### Example 2: SkyNet Repository (Advanced Manifest)

**Location**: `examples/skynet-repo/sysdocs.manifest.json`

**Pattern**: Multi-file composition with section extraction

```json
{
  "documents": [
    {
      "outputName": "01_SEMP_SkyNet.pdf",
      "sources": [
        {"file": "README.md", "sections": ["1.1", "1.2", "1.3"]},
        {"file": "project_description.md", "sections": ["2.1", "2.2"]},
        {"file": "docs/engineering_process.md", "sections": ["3.1"]}
      ]
    }
  ]
}
```

## Usage

### CLI Command (Planned)

```bash
# Generate from manifest
sysdocs --manifest examples/skynet-repo/sysdocs.manifest.json \
        --output output/

# Result: Generates all PDFs defined in manifest
# - 01_SEMP_SkyNet.pdf
# - 10_StRS_SkyNet.pdf
# - etc.
```

### API Usage (Planned)

```csharp
using SysDocs.Core.Manifest;

// Load manifest
var manifest = ManifestReader.Load("sysdocs.manifest.json");

// Process all documents
foreach (var doc in manifest.Documents)
{
    var output = await DocumentComposer.ComposeAsync(doc);
    await PdfGenerator.GenerateAsync(output, doc.OutputName);
}
```

## Testing

### Test Coverage

- **TC-11**: Multi-file SEMP assembly (README + project_description + docs)
- **TC-12**: Single-file with section extraction (StRS from requirements/)
- **TC-13**: Determinism verification (repeated runs)
- **TC-14**: Section extraction accuracy
- **TC-15**: Multi-file composition ordering
- **TC-16**: Section remapping verification
- **TC-17**: Cross-platform determinism
- **TC-18**: Invalid manifest error handling
- **TC-19**: Missing section error handling
- **TC-20**: Template configuration application

### Test Class

`tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`

### Test Execution

```bash
# Run all manifest tests
dotnet test --filter "Category=Manifest"

# Run in CI/CD
# Automatically executed in build and release pipelines
```

## Benefits

### For Modern Software Projects

- ✅ **Documentation in Repository**: README.md, docs/ folder patterns
- ✅ **Developer-Friendly**: Write docs where developers expect them
- ✅ **Git Workflow**: Documentation reviewed in pull requests
- ✅ **Living Documentation**: Docs evolve with code

### For SE Compliance

- ✅ **Formal Documents**: Generate required SE artifacts
- ✅ **Standards Compliance**: Meet INCOSE, ISO, IEEE requirements
- ✅ **Traceability**: Section-level mapping to sources
- ✅ **Audit Trail**: Manifest version-controlled in git

### For Organizations

- ✅ **Best of Both Worlds**: Modern practices + formal compliance
- ✅ **Reduce Duplication**: One set of docs, multiple outputs
- ✅ **Maintain Quality**: Automated generation, consistent format
- ✅ **Tool Qualification**: Deterministic output supports qualification

## Limitations

- Section extraction requires consistent heading patterns
- Markdown must use `#` headings (not underline-style)
- Section numbers must match manifest expectations
- Complex nested structures may need manual adjustment

## Future Enhancements

- **Conditional sections**: Include/exclude based on configuration
- **Variable substitution**: Template variables in sources
- **Cross-references**: Automatic link updates between documents
- **Multi-format sources**: Mix markdown, LaTeX, Word in one document
- **Section inheritance**: Reuse sections across multiple documents

---

**Related Documentation**:
- [REQUIREMENTS.md](REQUIREMENTS.md) - FR-16, FR-17, FR-18, FR-19
- [REQUIREMENTS_MATRIX.md](REQUIREMENTS_MATRIX.md) - Implementation status
- [examples/skynet-repo/README_TEST.md](../examples/skynet-repo/README_TEST.md) - Example usage
- [CI_CD_TEST_INTEGRATION.md](CI_CD_TEST_INTEGRATION.md) - CI/CD integration
