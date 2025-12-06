# Requirements Verification Matrix

**📖 Navigation**: [⬅️ Back: Branch Strategy](../project/BRANCH_STRATEGY.md) | [🏠 README](README.md) | [➡️ Next: Requirements Spec](../project/REQUIREMENTS.md) | [🗺️ Docs Navigation](docs/DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This is the **CENTRAL REQUIREMENTS MATRIX** showing each requirement's implementation, testing, and deviation status.  
> For detailed test data, see: [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md)

## How to Use This Document

This matrix provides a complete view of each requirement:
- **Requirement**: What needs to be implemented (from REQUIREMENTS.md)
- **Implementation**: Where/how it's implemented (from TRACEABILITY.md)
- **Tests**: Whether automated tests exist (✅ = has tests, ⚠️ = manual only, ❌ = no tests)
- **Deviations**: Any approved deviations affecting this requirement

**Quick Navigation:**
- [Functional Requirements](#functional-requirements)
- [Non-Functional Requirements](#non-functional-requirements)
- [Constraints](#constraints)
- [Build Requirements](#build-requirements)
- [Summary Statistics](#summary-statistics)

---

## Functional Requirements

### FR-01: Import from Markdown, Word, LaTeX, PDF
**Category**: Input Handling  
**Implementation**: `src/SysDocs.Core/Importers/`  
**Libraries**: MarkDig (MD), DocX (Word), iText7 (PDF)  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-01)  
**Deviations**: None  
**Verification**: 
- Unit tests for each format parser
- Integration tests for full import pipeline
- Manual review of imported document structure

---

### FR-02: Import images (SVG, PNG, JPEG, TIFF)
**Category**: Input Handling  
**Implementation**: `src/SysDocs.Core/Importers/ImageImporter.cs`  
**Libraries**: ImageSharp for cross-platform image processing  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-02)  
**Deviations**: None  
**Verification**:
- Unit tests for each image format
- Cross-platform rendering validation
- Image embedding tests in PDFs

---

### FR-03: Unified internal model conversion
**Category**: Input Handling  
**Implementation**: `src/SysDocs.Core/Model/`  
**Architecture**: Document Object Model (DOM) structure  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-03)  
**Deviations**: None  
**Verification**:
- Unit tests for document model structure
- Conversion tests from all input formats
- Structure preservation validation

---

### FR-04: PDF output generation
**Category**: Output  
**Implementation**: `src/SysDocs.Core/Exporters/PdfExporter.cs`  
**Libraries**: QuestPDF for deterministic PDF generation  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-04)  
**Deviations**: None  
**Verification**:
- Unit tests for PDF generation
- Integration tests for full pipeline
- Cross-platform output validation

---

### FR-05: Custom templates support
**Category**: Output  
**Implementation**: `src/SysDocs.Core/Templates/`  
**Features**: Watermarks, title pages, headers/footers, branding  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-05)  
**Deviations**: None  
**Verification**:
- Template loading tests
- Template application tests
- Visual review of rendered outputs

---

### FR-06: Deterministic PDF output
**Category**: Output  
**Implementation**: Deterministic rendering libraries, normalized timestamps, sorted collections  
**Scope**: **Output determinism** - Tool produces identical PDFs given same inputs across platforms  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-06)  
**Deviations**: None  
**Verification**:
- Cross-platform output hash comparison (Windows, Linux, macOS)
- Same input → same PDF hash on all platforms
- CI/CD automated verification across OS matrix

---

### FR-07: Identical output across environments
**Category**: Determinism & Qualification  
**Implementation**: Platform-independent rendering, content normalization  
**Scope**: **Output determinism** - Same content, layout, metadata on all platforms  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-07)  
**Deviations**: None  
**Verification**:
- Cross-platform PDF content comparison
- Metadata consistency validation
- CI/CD multi-OS output verification

---

### FR-08: Change and trace report generation
**Category**: Determinism & Qualification  
**Implementation**: `src/SysDocs.Core/Tracing/`  
**Features**: Git diff integration, change log generation  
**Tests**: ⚠️ Manual verification required  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-08)  
**Deviations**: None  
**Verification**:
- Manual review of change reports
- Git integration testing
- Diff accuracy validation

---

### FR-09: Tool qualification support
**Category**: Determinism & Qualification  
**Implementation**: `src/SysDocs.Core/Qualification/`  
**Features**: Logging, validation, audit trails  
**Tests**: ⚠️ Manual verification required  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-09)  
**Deviations**: None  
**Verification**:
- Audit log review
- Validation step checks
- Qualification package completeness

---

### FR-10: INCOSE SE Handbook alignment
**Category**: Systems Engineering Support  
**Requirement**: Outputs shall align with INCOSE SE Handbook processes and artifacts  
**Implementation Solution**: Templates and machine-parsable definitions  
**Artifact Definitions**: [docs/SE_ARTIFACT_DEFINITIONS.md](../docs/SE_ARTIFACT_DEFINITIONS.md)  
**Machine-Parsable Schema**: [src/SysDocs.Core/Model/SeArtifactDefinitions.json](../src/SysDocs.Core/Model/SeArtifactDefinitions.json)  
**Approach**: Built-in templates for SE artifacts mapped to INCOSE processes  
**Tests**: ⚠️ Manual verification required  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-10)  
**Deviations**: None  
**Verification**:
- Manual review against INCOSE SE Handbook (5th Edition)
- Template content validation against INCOSE process groups
- SE artifact completeness check using machine-parsable definitions
- Compliance with ISO/IEC/IEEE 15288 lifecycle processes

---

### FR-11: V-Model phase support
**Category**: Systems Engineering Support  
**Requirement**: Generate SE documentation aligned with V-Model phases  
**Implementation Solution**: V-Model templates with phase-based artifact generation  
**Supported Phases**: 8 phases defined (Stakeholder Req → System Req → Architecture → Detailed Design → Unit Verification → Integration Verification → System Verification → Validation)  
**Artifact Definitions**: [docs/SE_ARTIFACT_DEFINITIONS.md](../docs/SE_ARTIFACT_DEFINITIONS.md)  
**Machine-Parsable Schema**: [src/SysDocs.Core/Model/SeArtifactDefinitions.json](../src/SysDocs.Core/Model/SeArtifactDefinitions.json)  
**Templates**: `src/SysDocs.Templates/VModel/`  
**Cross-Cutting Artifacts**: Traceability Matrix, V&V Matrix, Change Management  
**Tests**: ⚠️ Manual verification required  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-11)  
**Deviations**: None  
**Verification**:
- Manual review against V-Model methodology
- All 8 phases represented in machine-parsable definitions
- Template completeness validation for each phase
- Traceability link validation between phases
- Cross-cutting artifact generation verification

---

### FR-12: Git integration (branches, tags, folders)
**Category**: Repository Integration  
**Implementation**: LibGit2Sharp  
**Libraries**: Cross-platform Git library for .NET  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-12)  
**Deviations**: None  
**Verification**:
- Git operations unit tests
- Branch/tag access tests
- Repository structure validation

---

### FR-13: Version integrity maintenance
**Category**: Repository Integration  
**Implementation**: `src/SysDocs.Core/Versioning/`  
**Approach**: Hash-based verification  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-13)  
**Deviations**: None  
**Verification**:
- Hash calculation tests
- Version tracking validation
- Integrity check tests

---

### FR-14: Cross-platform (Win, Linux, macOS)
**Category**: Portability  
**Requirement**: Tool shall run identically on Windows, Linux, and macOS  
**Implementation Solution**: Cross-platform capability achieved via .NET 10 (LTS platform)  
**Runtime**: .NET 10 SDK - executes identically on Windows, Linux, macOS  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-14)  
**Deviations**: None  
**Note**: Implementation leverages FR-15 (LTS platform requirement) to achieve cross-platform capability  
**Verification**:
- CI/CD tests on all platforms (Windows, Linux, macOS)
- Platform-specific behavior checks
- Runtime compatibility validation

---

### FR-15: LTS Platform Implementation
**Category**: Portability  
**Requirement**: Tool must be implemented using a Long-Term Support (LTS) platform version  
**Implementation Solution**: .NET 10 (LTS platform - released 2025, supported until 2028)  
**Rationale**: LTS platforms provide long-term compiler, runtime, and tooling support  
**Configuration**: Built and executed using .NET 10 SDK, targeting net10.0  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#fr-15)  
**Deviations**: ⚠️ **DEV-001** - Temporarily targeting net8.0 during .NET 10 ecosystem transition  
**Deviation Details**: See [DEVIATIONS.md#dev-001](DEVIATIONS.md#dev-001)  
**Note**: This requirement enables FR-14 (cross-platform capability via .NET's cross-platform runtime)  
**Verification**:
- Platform version checks (verify LTS version in use)
- Compiler and runtime LTS validation
- Long-term support commitment verification

---

### FR-16: Manifest File Support for Document Assembly
**Category**: Manifest-Based Assembly  
**Requirement**: Tool shall support manifest files (sysdocs.manifest.json) defining document assembly rules  
**Implementation**: Pending - `src/SysDocs.Core/Manifest/ManifestReader.cs`, `ManifestProcessor.cs`  
**Libraries**: System.Text.Json for manifest parsing  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`  
**Test Cases**: TC-11, TC-12, TC-13, TC-18, TC-20  
**Deviations**: None  
**Verification**:
- Unit tests for manifest JSON parsing and validation
- Integration tests for complete manifest-based assembly
- Test examples: adns-project/sysdocs.manifest.json, skynet-repo/sysdocs.manifest.json

---

### FR-17: Section Extraction from Markdown Headings
**Category**: Manifest-Based Assembly  
**Requirement**: Tool shall extract specific sections from markdown files based on heading patterns (e.g., ## 1.1 Project Scope)  
**Implementation**: Pending - `src/SysDocs.Core/Manifest/SectionExtractor.cs`  
**Pattern**: Regex-based heading matching with configurable patterns  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`  
**Test Cases**: TC-11, TC-12, TC-14, TC-19  
**Deviations**: None  
**Verification**:
- Unit tests for heading pattern matching
- Integration tests for section extraction with various heading formats
- Edge case testing: missing sections, malformed headings

---

### FR-18: Multi-File Document Composition
**Category**: Manifest-Based Assembly  
**Requirement**: Tool shall compose a single formal document from multiple source files, maintaining section order and renumbering  
**Implementation**: Pending - `src/SysDocs.Core/Manifest/DocumentComposer.cs`  
**Features**: Section ordering, renumbering, content merging  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`  
**Test Cases**: TC-11, TC-15, TC-16  
**Deviations**: None  
**Verification**:
- Integration tests for multi-file assembly
- Section ordering verification
- Section renumbering accuracy testing

---

### FR-19: Deterministic Manifest-Based Output
**Category**: Manifest-Based Assembly + Determinism  
**Requirement**: Manifest-based assembly shall produce deterministic output (byte-for-byte identical)  
**Implementation**: Pending - Extends FR-06 determinism to manifest-based workflows  
**Related**: FR-06 (Determinism), NFR-01 (Reproducibility), FR-14 (Cross-platform)  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`  
**Test Cases**: TC-13, TC-17  
**Deviations**: None  
**Verification**:
- Determinism tests (repeated runs with same manifest)
- Cross-platform determinism (Linux, Windows, macOS)
- Hash comparison with expected results

---

### FR-20: Import SysML v2 Models
**Category**: SysML v2 Integration  
**Requirement**: Import SysML v2 models, preserving traceability links and element relationships  
**Implementation**: Pending - `src/SysDocs.Core/Importers/SysML/`  
**Core Capability**: Parse SysML v2 models and extract all model elements with relationships  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/SysML/FR20_FR21_SysMLRequirementsUseCasesTests.cs`  
**Test Cases**: TC-01, TC-02, TC-05, TC-07  
**Deviations**: None  
**Verification**:
- Model import from all supported formats
- Element extraction validation
- Relationship preservation
- Traceability link extraction
- Cross-element references maintained

---

### FR-21: Support SysML v2 Diagram Types
**Category**: SysML v2 Integration  
**Requirement**: Support SysML v2 diagram types including Requirements, Use Case, BDD, IBD, Parametric, Activity, Sequence, State Machine, and Package diagrams  
**Implementation**: Pending - `src/SysDocs.Core/Importers/SysML/`  
**Diagram Types**: 9 core SysML v2 diagram types per OMG specification  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/SysML/FR22_FR23_SysMLDiagramsTests.cs`  
**Test Cases**: TC-01 through TC-09  
**Deviations**: None  
**Verification**:
- Requirements diagram import
- Use Case diagram import
- Block Definition diagram (BDD) import
- Internal Block diagram (IBD) import
- Parametric diagram import
- Activity diagram import
- Sequence diagram import
- State Machine diagram import
- Package diagram import

---

### FR-22: Support SysML v2 File Formats
**Category**: SysML v2 Integration + Input Handling  
**Requirement**: Support SysML v2 file formats including .sysml (textual), .kerml (KerML), .json (API format), and .sysmlv2  
**Implementation**: Pending - Multi-format parser infrastructure  
**Format Support**:  
- `.sysml` - SysML v2 textual syntax  
- `.kerml` - Kernel Modeling Language  
- `.json` - SysML v2 API JSON format  
- `.sysmlv2` - Alternative textual syntax  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/SysML/FR20_FR21_SysMLRequirementsUseCasesTests.cs`  
**Test Cases**: TC-06  
**Deviations**: None  
**Verification**:
- Parse tests for each format
- Equivalent content validation (same model, different formats)
- Format auto-detection capability

---

### FR-23: Render SysML v2 Diagrams
**Category**: SysML v2 Integration + Output  
**Requirement**: SysML v2 diagrams shall be rendered as embedded images (SVG, PNG, or PDF) in output documents  
**Implementation**: Pending - Diagram rendering engine  
**Output Formats**: SVG (preferred), PNG, PDF  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: `tests/SysDocs.Tests/Integration/SysML/FR22_FR23_SysMLDiagramsTests.cs`  
**Test Cases**: TC-08, TC-09  
**Deviations**: None  
**Verification**:
- SVG diagram generation
- PNG diagram generation (fallback)
- PDF diagram embedding
- Diagram quality validation
- Deterministic rendering

---

### FR-24: Extract SysML v2 Element Properties
**Category**: SysML v2 Integration  
**Requirement**: Extract and present SysML v2 model element properties (Requirements, Use Cases, Blocks, Interactions)  
**Implementation**: Pending - Element property extractors  
**Property Categories**: Requirements attributes, Use Case details, Block specifications, Interaction details  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: Multiple test classes  
**Test Cases**: FR20_FR21 TC-01 through TC-04, FR22_FR23 TC-03, TC-06  
**Deviations**: None  
**Verification**:
- Requirements: ID, text, verification method extraction
- Use Cases: actors, scenarios, preconditions/postconditions extraction
- Blocks: ports, properties, constraints, operations extraction
- Interactions: lifelines, messages, timing constraints extraction
- Property presentation in output documents

---

### FR-25: Deterministic SysML v2 Output
**Category**: SysML v2 Integration + Determinism  
**Requirement**: SysML v2 import shall produce deterministic output (byte-for-byte identical) across all supported file formats  
**Implementation**: Pending - Extends FR-06 determinism to SysML v2 workflows  
**Related**: FR-06 (Determinism), NFR-01 (Reproducibility), FR-14 (Cross-platform)  
**Tests**: ✅ Has test structure (skipped pending implementation)  
**Test Class**: Multiple test classes  
**Test Cases**: FR20_FR21 TC-07, FR22_FR23 TC-08  
**Deviations**: None  
**Verification**:
- Determinism tests (repeated imports with same model)
- Cross-format determinism (.sysml, .kerml, .json produce identical output)
- Cross-platform determinism (Linux, Windows, macOS)
- Hash comparison with expected results

---

## Non-Functional Requirements

### NFR-01: 100% reproducible output
**Category**: Reliability  
**Implementation**: Deterministic rendering + content normalization  
**Scope**: **Output determinism** - Byte-for-byte identical PDFs across all platforms  
**Platforms Verified**: Windows 10+11, Linux (Fedora, Debian), macOS  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-01)  
**Deviations**: None  
**Verification**:
- Cross-platform hash comparison (CI/CD)
- Same input → same output hash on Windows, Linux, macOS
- Multi-run determinism checks per platform

---

### NFR-02: Nondeterministic content normalization
**Category**: Reliability  
**Implementation**: Rendering engine  
**Approach**: Timestamp standardization, metadata cleanup  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-02)  
**Deviations**: None  
**Verification**:
- Timestamp normalization tests
- Metadata cleanup validation
- Content consistency checks

---

### NFR-03: Acceptable performance for large docs (>250 pages)
**Category**: Performance  
**Implementation**: Optimization phase  
**Approach**: Streaming processing, parallel rendering  
**Tests**: ⚠️ Performance benchmarks required  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-03)  
**Deviations**: None  
**Verification**:
- Performance benchmarking
- Large document tests (250+ pages)
- Memory usage profiling

---

### NFR-04: Clear CLI and API interfaces
**Category**: Usability  
**Implementation**: System.CommandLine  
**Approach**: Docker run commands for automation  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-04)  
**Deviations**: None  
**Verification**:
- CLI interface tests
- API usability checks
- Documentation review

---

### NFR-05: Air-gap compatible (no network required)
**Category**: Security  
**Implementation**: Docker container with embedded dependencies  
**Approach**: All dependencies bundled in container  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-05)  
**Deviations**: None  
**Verification**:
- Network isolation tests
- Dependency bundling validation
- Offline execution checks

---

### NFR-06: Secure Git credentials handling
**Category**: Security  
**Implementation**: Credential helpers  
**Approach**: Environment variables, no hardcoded secrets  
**Tests**: ⚠️ Security review required  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-06)  
**Deviations**: None  
**Verification**:
- Security audit of credential handling
- Environment variable validation
- No secrets in code/logs check

### NFR-07: Code Signing and Commit Verification
**Category**: Security  
**Requirement**: All commits must be GPG-signed and all release artifacts must be digitally signed  
**Implementation Solution**: 
- **Commit Signing**: GPG signatures for all commits on protected branches
- **Linux Releases**: GPG-signed release tarballs and checksums
- **Docker Images**: Cosign signatures for container images
- **NuGet Packages**: NuGet package signing for .NET packages
**GPG Key Management**: GitHub GPG keys for commit verification  
**Container Signing**: Sigstore Cosign for Docker image signatures  
**Signing Scripts**: `scripts/sign-release.sh`, `scripts/verify-signatures.sh`  
**Documentation**: [docs/CODE_SIGNING_GUIDE.md](../docs/CODE_SIGNING_GUIDE.md)  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#nfr-07)  
**Deviations**: DEV-004 (Signing infrastructure in development)  
**Verification**:
- GPG commit signature verification (git log --show-signature)
- Release artifact GPG signature verification (gpg --verify)
- Docker image signature verification (cosign verify)
- NuGet package signature verification (dotnet nuget verify)
- Automated tests in `NFR07_ShouldHaveSignedCommitsAndArtifacts()`
- CI/CD pipeline enforces signed commits on protected branches

---

## Constraints

### C-01: MIT License with Compatible Dependencies
**Category**: Legal  
**Requirement**: Tool must be MIT-licensed and only use dependencies with MIT-compatible licenses  
**Implementation**: MIT License file + dependency license validation  
**Repository**: Public on GitHub  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#c-01)  
**Deviations**: None  
**Verification**:
- License file presence check (MIT)
- License header validation in source files
- Automated dependency license compliance scanning (all dependencies must be MIT-compatible)
- Public repository confirmation

---

### C-02: Platform-independent behavior
**Category**: Technical  
**Implementation**: .NET 10 + embedded fonts  
**Approach**: Font embedding, cross-platform libs only  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#c-02)  
**Deviations**: None  
**Verification**:
- Cross-platform rendering tests
- Font embedding validation
- Behavior consistency checks

---

### C-03: Deterministic rendering libraries
**Category**: Technical  
**Implementation**: QuestPDF, ImageSharp, embedded resources  
**Approach**: Libraries chosen for reproducibility  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#c-03)  
**Deviations**: None  
**Verification**:
- License compliance check (all open-source)
- Determinism validation
- Library dependency audit

---

### C-04: Official builds must use Nix
**Category**: Build  
**Implementation**: Nix build system with flakes  
**Requirement**: Identical hash across Linux distributions  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#c-04)  
**Deviations**: ⚠️ **DEV-002** - Nix uses .NET 8 SDK (intentional for determinism); **DEV-003** - Cross-platform scope limited to Linux  
**Deviation Details**: See [DEVIATIONS.md#dev-002](DEVIATIONS.md#dev-002) and [DEVIATIONS.md#dev-003](DEVIATIONS.md#dev-003-cross-platform-build-scope-limited-to-linux-distributions)  
**Verification**:
- Hash comparison across Linux distros (Fedora + Debian)
- Build reproducibility checks on target platform
- Docker image determinism

---

## Build Requirements

### BR-01: Nix with flakes as primary build system
**Category**: Build  
**Implementation**: Nix flake configuration  
**Files**: `flake.nix`, `flake.lock`  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#br-01)  
**Deviations**: ⚠️ **DEV-002** - Nix uses .NET 8 SDK  
**Deviation Details**: See [DEVIATIONS.md#dev-002](DEVIATIONS.md#dev-002)  
**Verification**:
- Nix build success tests
- Flake evaluation validation
- Build reproducibility

---

### BR-02: Identical binary artifacts (same SHA256)
**Category**: Build  
**Implementation**: Nix deterministic build process across Linux distributions  
**Scope**: **Build determinism** - Tool binary itself builds identically on different Linux distros  
**Verification Method**: SHA256 hash comparison (Fedora vs Debian)  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#br-02)  
**Deviations**: See [DEV-003](DEVIATIONS.md#dev-003-cross-platform-build-scope-limited-to-linux-distributions) for cross-platform scope  
**Verification**:
- Cross-distro hash comparison (Fedora + Debian in CI/CD)
- Timestamp normalization check
- Byte-for-byte reproducibility of tool binary on Linux

**Note**: This verifies the **tool itself** builds identically. For **tool output** determinism, see FR-06, FR-07, NFR-01.

---

### BR-03: Docker images built with Nix
**Category**: Build  
**Implementation**: `dockerTools.buildLayeredImage`  
**Approach**: Deterministic Docker image creation  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#br-03)  
**Deviations**: None  
**Verification**:
- Docker image hash validation
- Layer determinism checks
- Image functionality tests

---

### BR-04: All dependencies pinned to specific versions
**Category**: Build  
**Implementation**: `flake.lock`, NuGet package versions  
**Files**: `Directory.Build.props`, `*.csproj`  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#br-04)  
**Deviations**: None  
**Verification**:
- Dependency pinning validation
- Lock file integrity checks
- Version consistency tests

---

### BR-05: Build timestamps normalized to 1980-01-01
**Category**: Build  
**Implementation**: Nix build configuration  
**Approach**: SOURCE_DATE_EPOCH environment variable  
**Tests**: ✅ Has automated tests  
**Test Details**: See [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md#br-05)  
**Deviations**: None  
**Verification**:
- Timestamp normalization check
- Build artifact inspection
- Reproducibility validation

---

## Summary Statistics

### Test Coverage Summary

Run this command to get current statistics:
```bash
dotnet run --project tests/SysDocs.Tests -- --traceability
```

Then check [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) for:
- Total requirements: 41 (FR-01 to FR-25, NFR-01 to NFR-07, C-01 to C-04, BR-01 to BR-05)
- Requirements with automated tests
- Requirements with manual verification only
- Requirements without tests

### Verification Status

| Status | Count | Description |
|--------|-------|-------------|
| ✅ Has automated tests | Check report | Requirements with unit/integration tests |
| ⚠️ Manual verification | Check report | Requirements needing human review |
| ❌ No tests | Check report | Requirements without any verification |

### Deviation Summary

| ID | Requirement | Status | Severity |
|----|-------------|--------|----------|
| DEV-001 | FR-15 | Active - Temporary | Low |
| DEV-002 | C-04, BR-01 | Active - Intentional | Low |

See [DEVIATIONS.md](../project/DEVIATIONS.md) for complete deviation details.

---

## How to Verify a Requirement

For any requirement ID (e.g., FR-01):

1. **Find in this matrix** - Locate the requirement section above
2. **Check test status** - Look for ✅/⚠️/❌ indicator
3. **Review test details** - Follow link to TEST_TRACEABILITY.md
4. **Run specific tests**:
   ```bash
   dotnet test --filter "RequirementId=FR-01"
   ```
5. **Check for deviations** - If ⚠️ appears, read deviation details
6. **Review implementation** - Check the listed implementation files

## Updating This Matrix

When requirements change:

1. **Update REQUIREMENTS.md** - The source of truth
2. **Update this matrix** - Implementation and verification approach
3. **Add/modify tests** - Ensure requirement is testable
4. **Regenerate report**:
   ```bash
   dotnet run --project tests/SysDocs.Tests -- --traceability
   ```
5. **Update DEVIATIONS.md** - If any deviations are needed

---

**Related Documents:**
- [REQUIREMENTS.md](../project/REQUIREMENTS.md) - Detailed requirement specifications
- [TRACEABILITY.md](../project/TRACEABILITY.md) - Implementation mapping strategy
- [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) - Auto-generated test data
- [DEVIATIONS.md](../project/DEVIATIONS.md) - Approved deviations with justification
- [VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md) - V&V strategy

