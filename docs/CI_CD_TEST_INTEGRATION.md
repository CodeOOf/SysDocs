# CI/CD Test Integration

This document describes how the automated tests are integrated into the CI/CD pipeline.

## Test Categories

Tests are organized by category for targeted execution:

- **`FileFormat`**: Tests FR-01 (multi-format import: Markdown, LaTeX, plain text, CSV, JSON, XML)
- **`Determinism`**: Tests FR-06 and NFR-01 (deterministic, reproducible output)
- **`CrossPlatform`**: Tests FR-14 (cross-platform consistency)
- **`Integration`**: Integration tests
- **`Unit`**: Unit tests

## CI Pipeline (`ci.yml`)

### Build & Test Job

Runs on: Linux, Windows, macOS

```yaml
- name: Test FR-01 Multi-Format Import
  run: dotnet test --filter "Category=FileFormat"

- name: Test FR-06/NFR-01 Determinism
  run: dotnet test --filter "Category=Determinism"
```

### FR-14 Cross-Platform Tests Job

Runs on: Linux, Windows, macOS (matrix)

- Executes all tests tagged with `Category=CrossPlatform`
- Generates platform-specific hash files for comparison
- Each platform processes `examples/expected-results/` PDFs

### Cross-Platform Hash Verification Job

Runs on: Linux (after all platform tests complete)

- Downloads hash files from all platforms
- Compares hashes across Linux, Windows, macOS
- Fails if any platform produces different output
- Verifies FR-14 compliance

### Generate Expected Results Job

Runs on: Linux (alpha branch only, if needed)

- Checks if `examples/expected-results/` PDFs exist
- Generates them if missing (when implementation is ready)
- Commits generated files back to repository

## Release Pipeline (`release.yml`)

### Build Linux Job

- Runs FR-01 format tests
- Runs FR-06/NFR-01 determinism tests
- Must pass before Docker image is built

### Verify Docker Cross-Platform Job

Runs on: Linux, Windows, macOS (matrix)

1. Pulls the released Docker image
2. Creates test input file
3. Runs Docker container to generate PDF
4. Computes SHA256 hash of output
5. Uploads hash for comparison

### Verify FR-14 & NFR-01 Compliance Job

Runs on: Linux (after all platform tests)

- Downloads all hash files
- Compares hashes from Linux, Windows, macOS
- **Blocks release** if hashes don't match
- Verifies:
  - ✅ FR-14: Cross-platform determinism
  - ✅ NFR-01: 100% reproducible output
  - ✅ FR-06: Deterministic output
  - ✅ FR-07: Identical content across platforms

## Test Fixtures

### Source: `examples/adns-project/`

Test input files:
- Markdown documents (8 files)
- LaTeX documents (1 file)
- Plain text documents (2 files)
- CSV files (5 files)
- JSON files (1 file)
- XML files (1 file)
- SVG images (1 file)

### Expected Results: `examples/expected-results/`

Baseline PDFs for byte-for-byte verification:

```
expected-results/
├── individual/
│   ├── 01_markdown.pdf    # From 12_SRS_ADNS.md
│   ├── 02_latex.pdf       # From 21_SDD_ADNS_Software.tex
│   ├── 03_plaintext.pdf   # From 22_HDD_ADNS_Hardware.txt
│   ├── 04_csv.pdf         # From requirements_export.csv
│   ├── 05_json.pdf        # From 13_RTM_ADNS.json
│   ├── 06_xml.pdf         # From project_metadata.xml
│   └── 07_mixed.pdf       # From multiple inputs
└── folder/
    └── complete.pdf       # From entire adns-project/ folder
```

## Test Execution Flow

### Pull Request

1. **Build** on all platforms (Linux, Windows, macOS)
2. **Unit tests** on all platforms
3. **FR-01 tests** (file format import)
4. **FR-06/NFR-01 tests** (determinism)
5. **FR-14 tests** (cross-platform) on all platforms
6. **Hash comparison** across platforms

### Push to `alpha`

All PR checks, plus:
- **Generate expected results** (if missing)
- **Docker image build** and push

### Release Tag

All checks, plus:
- **Build Linux artifacts**
- **Build Docker image**
- **Docker cross-platform verification** (FR-14, NFR-01)
  - Run Docker on Linux, Windows, macOS
  - Compare output hashes
  - **Block release if hashes differ**
- **Create GitHub release** (only if all verifications pass)

## Test Verification Method

### Byte-for-Byte Comparison

All tests use SHA256 hash comparison for verification:

```csharp
var generatedHash = ComputeSha256(outputFile);
var expectedHash = ComputeSha256(expectedFile);
generatedHash.Should().Be(expectedHash);
```

This ensures:
- No metadata differences
- No timestamp variations
- No floating-point rounding differences
- No platform-specific variations

### Why SHA256?

- Cryptographically strong (collision-resistant)
- Fast computation
- Standard in CI/CD environments
- Any single byte difference produces completely different hash

## Running Tests Locally

### All Tests

```bash
dotnet test tests/SysDocs.Tests/SysDocs.Tests.csproj
```

### By Category

```bash
# FR-01: Multi-format import
dotnet test --filter "Category=FileFormat"

# FR-06/NFR-01: Determinism
dotnet test --filter "Category=Determinism"

# FR-14: Cross-platform
dotnet test --filter "Category=CrossPlatform"
```

### By Requirement

```bash
# All tests for FR-01
dotnet test --filter "RequirementId=FR-01"

# All tests for NFR-01
dotnet test --filter "RequirementId=NFR-01"
```

### Specific Test

```bash
dotnet test --filter "FullyQualifiedName~TC01_Should_Import_Markdown_Format"
```

## Generating Expected Results

### First Time Setup

```bash
# Build SysDocs
dotnet build src/SysDocs.sln --configuration Release

# Generate expected results (when implementation is ready)
# TODO: Implement generation script
# make generate-expected-results

# Or manually:
# dotnet run --project src/SysDocs.Cli --configuration Release -- \
#   --input examples/adns-project/12_SRS_ADNS.md \
#   --output examples/expected-results/individual/01_markdown.pdf
```

### After Changes

If output format changes intentionally:

```bash
# Regenerate expected results
make regenerate-expected-results

# Commit with explanation
git add examples/expected-results/
git commit -m "test: Update expected results - improved table rendering"
```

## Test Status

| Test Suite | Status | Notes |
|------------|--------|-------|
| **FR-01 Multi-Format** | ⏳ Pending | Tests created, awaiting implementation |
| **FR-06 Determinism** | ⏳ Pending | Tests created, awaiting implementation |
| **FR-14 Cross-Platform** | ⏳ Pending | Tests created, awaiting implementation |
| **NFR-01 Reproducible** | ⏳ Pending | Tests created, awaiting implementation |

Tests are marked with `[Fact(Skip = "Pending implementation")]` until SysDocs PDF generation is implemented.

## Integration with Traceability

All tests use `[RequirementTest]` attribute for automatic traceability:

```csharp
[Fact]
[RequirementTest("FR-01", TestType.Integration, Description = "Markdown format import")]
[TestCategory(TestCategories.FileFormat)]
public async Task TC01_Should_Import_Markdown_Format() { }
```

The traceability report generator (`tests/SysDocs.Tests/Tools/TraceabilityReportGenerator.cs`) scans all tests and generates `reports/TEST_TRACEABILITY.md` showing requirement coverage.

## See Also

- `examples/TEST_PLAN.md` - Detailed test plan with test cases
- `examples/README.md` - ADNS project documentation
- `tests/SysDocs.Tests/Integration/Formats/FR01_MultiFormatImportTests.cs` - FR-01 test implementation
- `tests/SysDocs.Tests/Integration/Determinism/FR06_NFR01_DeterministicOutputTests.cs` - Determinism tests
- `tests/SysDocs.Tests/Integration/CrossPlatform/FR14_CrossPlatformDeterminismTests.cs` - Cross-platform tests
