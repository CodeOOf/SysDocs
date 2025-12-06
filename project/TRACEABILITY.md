# Requirements Traceability Matrix

**📖 Navigation**: [⬅️ Back: Requirements](../project/REQUIREMENTS.md) | [🏠 README](README.md) | [➡️ Next: V&V Strategy](../project/VERIFICATION_VALIDATION.md) | [🗺️ Docs Navigation](docs/DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This is a **HUMAN-MAINTAINED GUIDE** explaining traceability strategy.  
> For current test coverage data, see auto-generated: [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md)  
> For complete V&V status per requirement, see: [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)

This document tracks how each requirement from `REQUIREMENTS.md` is fulfilled in the SysDocs implementation.

## Automated Test Traceability

**For detailed requirement-to-test mapping, see:** [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md)

The project uses automated tests tagged with requirement IDs to ensure traceability. Each test is marked with `[RequirementTest("FR-XX")]` attributes that map to requirements in this document.

**Generate latest traceability report:**
```bash
dotnet run --project tests/SysDocs.Tests -- --traceability
```

**Run tests for specific requirement:**
```bash
dotnet test --filter "RequirementId=FR-01"
```

**Current Test Coverage:** See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) for detailed requirement-to-test mapping

---

## 1. Functional Requirements

### 1.1 Input Handling

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| FR-01 | Import from Markdown, Word, LaTeX, PDF | `src/SysDocs.Core/Importers/` | Libraries: MarkDig (MD), DocX (Word), iText7 (PDF) |
| FR-02 | Import images (SVG, PNG, JPEG, TIFF) | `src/SysDocs.Core/Importers/ImageImporter.cs` | ImageSharp for cross-platform image processing |
| FR-03 | Unified internal model conversion | `src/SysDocs.Core/Model/` | Document Object Model (DOM) structure |

### 1.2 Output

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| FR-04 | PDF output generation | `src/SysDocs.Core/Exporters/PdfExporter.cs` | QuestPDF for deterministic PDF generation |
| FR-05 | Custom templates support | `src/SysDocs.Core/Templates/` | Template engine with branding support |
| FR-06 | Deterministic PDF output | Nix build system + Docker | Ensured through fixed dependencies and environment |

### 1.3 Determinism & Qualification

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| FR-07 | Identical output across environments | Nix flake + Docker container | Reproducible builds with pinned dependencies |
| FR-08 | Change and trace report generation | `src/SysDocs.Core/Tracing/` | Git diff integration, change log generation |
| FR-09 | Tool qualification support | `src/SysDocs.Core/Qualification/` | Logging, validation, audit trails |

### 1.4 Systems Engineering Support

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| FR-10 | INCOSE SE Handbook alignment | Templates and documentation | Built-in templates for SE artifacts |
| FR-11 | V-Model phase support | `src/SysDocs.Templates/VModel/` | Stakeholder req, system req, V&V matrices |

### 1.5 Repository Integration

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| FR-12 | Git integration (branches, tags, folders) | LibGit2Sharp | Cross-platform Git library for .NET |
| FR-13 | Version integrity maintenance | `src/SysDocs.Core/Versioning/` | Hash-based verification |

### 1.6 Portability

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| FR-14 | Cross-platform execution (Win, Linux, macOS) | .NET 10 LTS platform | Cross-platform capability achieved through .NET runtime (see FR-15) |
| FR-15 | LTS platform implementation | .NET 10 LTS platform | Compiler/runtime LTS support enables long-term stability and FR-14 cross-platform capability |

---

## 2. Non-Functional Requirements

### Reliability

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| NFR-01 | 100% reproducible output | Nix + Docker + deterministic libraries | Fixed timestamps, sorted collections, pinned deps |
| NFR-02 | Nondeterministic content normalization | Rendering engine | Timestamp standardization, metadata cleanup |

### Performance

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| NFR-03 | Acceptable performance for large docs (>250 pages) | Optimization phase | Streaming processing, parallel rendering |

### Usability

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| NFR-04 | Clear CLI and API interfaces | System.CommandLine | Docker run commands for automation |

### Security

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| NFR-05 | Air-gap compatible (no network required) | Docker container with embedded dependencies | All dependencies bundled in container |
| NFR-06 | Secure Git credentials handling | Credential helpers | Environment variables, no hardcoded secrets |

---

## 3. Constraints

| ID | Requirement | Implementation | Notes |
|----|-------------|----------------|-------|
| C-01 | MIT License with compatible dependencies | MIT License + automated dependency license scanning | Tool licensed under MIT; all dependencies verified MIT-compatible to preserve license integrity |
| C-02 | Platform-independent behavior | .NET 10 + embedded fonts | Font embedding, cross-platform libs only |
| C-03 | Deterministic rendering libraries | QuestPDF, ImageSharp, embedded resources | Libraries chosen for reproducibility |

---

## Verification Approach

Each requirement is verified through:

1. **Unit Tests**: Core functionality (`tests/SysDocs.Tests/`)
2. **Integration Tests**: End-to-end workflows (`tests/SysDocs.Tests/Integration/`)
3. **Determinism Tests**: Identical output verification across runs
4. **Manual Review**: Template rendering, document quality
5. **Cross-Platform CI**: GitHub Actions testing on Windows, Linux, macOS

