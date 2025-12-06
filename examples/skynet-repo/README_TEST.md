# SkyNet Repository - Advanced Markdown Structure Example

This example demonstrates **manifest-based document assembly** where SE documents are composed from distributed markdown files across a git repository structure.

## Project Structure

```
skynet-repo/
├── README.md                          # Section 1.1-1.10 of SEMP
├── project_description.md             # Section 2.x of SEMP
├── sysdocs.manifest.json              # Document assembly manifest
├── docs/
│   ├── engineering_process.md         # Section 3.x of SEMP
│   └── index.md                       # Document index
├── requirements/
│   ├── stakeholder_requirements.md    # Complete StRS document
│   └── system_requirements.md         # Complete SyRS document
├── design/
│   └── architecture.md                # Architecture documentation
└── verification/
    └── test_strategy.md               # Verification planning
```

## Concept: Distributed Documentation

Unlike the `adns-project` example where each SE document is a single file (e.g., `01_SEMP_ADNS.md`), this example follows a **git repository documentation pattern** where:

1. **README.md** contains high-level project information that maps to SEMP Section 1
2. **project_description.md** contains detailed descriptions that map to SEMP Section 2
3. **docs/engineering_process.md** contains process details that map to SEMP Section 3
4. Multiple markdown files compose a single formal SE document

## Manifest-Based Mapping

The `sysdocs.manifest.json` file defines:

### Document Assembly Rules

```json
{
  "outputName": "01_SEMP_SkyNet.pdf",
  "sources": [
    {
      "file": "README.md",
      "sections": ["1.1", "1.2", "1.3"],
      "sectionMapping": {
        "1.1": "1. Project Scope",
        "1.2": "1. Project Overview"
      }
    },
    {
      "file": "project_description.md",
      "sections": ["2.1", "2.2"],
      "sectionMapping": {
        "2.1": "2. System Purpose"
      }
    }
  ]
}
```

### Section Extraction

The manifest supports extracting specific heading sections from markdown:

- **Pattern**: `## 1.1 Project Scope` → Extract as "1. Project Scope" in PDF
- **Heading-based**: Sections identified by `## X.Y SectionName` patterns
- **Remapping**: Original section numbers can be remapped in output

## Use Cases

This pattern is ideal for:

1. **Living Documentation**: Documentation lives in repository README files
2. **Git Workflow**: Documentation evolves with code, reviewed in PRs
3. **Developer-Friendly**: Engineers write docs in familiar repository structure
4. **Formal Output**: SysDocs assembles formal SE documents for audits/reviews

## Test Coverage

This example tests:

- **FR-16**: Manifest-based document assembly
- **FR-17**: Section extraction from markdown headings
- **FR-18**: Multi-file document composition
- **FR-06/NFR-01**: Deterministic output (byte-for-byte identical)
- **FR-14**: Cross-platform determinism

## Comparison with adns-project

| Aspect | adns-project | skynet-repo |
|--------|-------------|-------------|
| **Structure** | One SE doc = one file | One SE doc = multiple files |
| **Pattern** | Formal SE naming | Git repo documentation |
| **Sections** | Monolithic documents | Distributed across files |
| **Use Case** | Traditional SE projects | Modern software projects |
| **Complexity** | Simple, direct | Advanced, requires manifest |

## Expected Output

Running SysDocs on this project with the manifest should produce:

- `01_SEMP_SkyNet.pdf` - Assembled from README.md + project_description.md + docs/engineering_process.md
- `10_StRS_SkyNet.pdf` - Assembled from requirements/stakeholder_requirements.md

Both outputs must be:
- ✅ Deterministic (byte-for-byte identical across runs)
- ✅ Cross-platform identical (Windows, Linux, macOS)
- ✅ Properly formatted with correct section numbers
- ✅ Include all extracted sections in correct order

---

**Related Test**: `tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`
