# SysDocs Example Documents

This directory contains complete example project documentation for testing SysDocs' capabilities.

## Projects

### ADNS Project (Autonomous Drone Navigation System)

A comprehensive systems engineering documentation set for a fictional aerospace project. Follows INCOSE SE Handbook v4 and IEEE standards.

**Location**: `examples/adns-project/`

**Documents**: 8 complete SE documents (more in progress)

---

## Document Naming Convention

Files follow a standardized naming scheme:

```
[NN]_[TYPE]_[Project]_[OptionalDetail].md
```

- **NN**: Two-digit sequence (00-99) for ordering
- **TYPE**: Document type abbreviation (SEMP, SRS, STP, etc.)
- **Project**: Short project identifier (ADNS)
- **OptionalDetail**: Optional descriptive suffix

### Examples:
- `00_PROJECT_INDEX.md` - Master document index
- `01_SEMP_ADNS.md` - Systems Engineering Management Plan
- `12_SRS_ADNS.md` - System Requirements Specification
- `31_STP_ADNS.md` - System Test Plan
- `40_FMEA_ADNS.md` - Failure Modes & Effects Analysis

---

## Current Document Set (ADNS Project)

### Phase 1: Project Planning (00-09)
- ✅ `00_PROJECT_INDEX.md` - Complete document index with traceability
- ✅ `01_SEMP_ADNS.md` - Systems Engineering Management Plan (~600 lines)

### Phase 2: Requirements (10-19)
- ✅ `10_StRS_ADNS.md` - Stakeholder Requirements Specification (~450 lines)
- ✅ `11_ConOps_ADNS.md` - Concept of Operations (~700 lines)
- ✅ `12_SRS_ADNS.md` - System Requirements Specification (~400 lines)
- ✅ `13_RTM_ADNS.md` - Requirements Traceability Matrix (~70 lines)
- ✅ `13_RTM_ADNS.csv` - RTM (CSV format)
- ✅ `13_RTM_ADNS.json` - RTM (JSON format)

### Phase 3: Architecture & Design (20-29)
- ⏳ `20_SAD_ADNS.md` - System Architecture Description (TODO)
- ✅ `21_SDD_ADNS_Software.tex` - Software Design Document (~350 lines, LaTeX)
- ✅ `22_HDD_ADNS_Hardware.txt` - Hardware Design Document (~500 lines, plain text)
- ✅ `23_ICD_ADNS_Sensors.txt` - Interface Control Document - Sensors (~400 lines, plain text)

### Phase 4: Verification & Validation (30-39)
- ⏳ `30_VVP_ADNS.md` - Verification & Validation Plan (TODO)
- ✅ `31_STP_ADNS.md` - System Test Plan (~800 lines)
- ⏳ `32_SIT_ADNS.md` - System Integration Test Procedures (TODO)
- ⏳ `33_ATP_ADNS.md` - Acceptance Test Procedures (TODO)

### Phase 5: Safety & Risk (40-49)
- ✅ `40_FMEA_ADNS.md` - Failure Modes & Effects Analysis (~650 lines)
- ⏳ `41_FTA_ADNS.md` - Fault Tree Analysis (TODO)
- ⏳ `42_SafetyCase_ADNS.md` - Safety Case Report (TODO)

### Data Exports & Auxiliary Files
- ✅ `requirements_export.csv` - Requirements database (15 requirements)
- ✅ `risk_register.csv` - Risk management data (10 risks)
- ✅ `test_results.csv` - Test execution results (15 tests)
- ✅ `project_metadata.xml` - Project metadata (XML format)
- ✅ `90_Glossary_ADNS.csv` - Glossary of terms (23 entries)
- ✅ `images/architecture_diagram.svg` - System architecture diagram (SVG)
- ✅ `FORMAT_TEST_GUIDE.md` - File format reference and complexity analysis
- ✅ `TEST_PLAN.md` - Comprehensive test plan with test cases

**Note**: Expected test results are stored in `examples/expected-results/` (outside of `adns-project/` since the entire project folder is used as test input).

---

## Using with SysDocs

### Primary Use Case: Folder-Level Input

The **primary test case** is to process the entire ADNS project folder:

```bash
# Generate comprehensive PDF from entire project folder
sysdocs --input examples/adns-project/ \
        --output output/ADNS_Complete.pdf
```

**This tests**:
- Folder traversal and file discovery
- Multi-file processing
- Mixed format handling (Markdown, LaTeX, plain text, CSV, JSON, XML)
- Document ordering and assembly
- Comprehensive documentation generation

### Secondary Use Case: Individual Format Tests

Individual file tests verify specific format import capabilities (FR-01):

```bash
# Test Markdown format
sysdocs --input examples/adns-project/12_SRS_ADNS.md \
        --output output/test_markdown.pdf

# Test LaTeX format
sysdocs --input examples/adns-project/21_SDD_ADNS_Software.tex \
        --output output/test_latex.pdf

# Test plain text format
sysdocs --input examples/adns-project/22_HDD_ADNS_Hardware.txt \
        --output output/test_plaintext.pdf

# Test CSV format
sysdocs --input examples/adns-project/requirements_export.csv \
        --output output/test_csv.pdf

# Test mixed formats
sysdocs --input examples/adns-project/12_SRS_ADNS.md \
        --input examples/adns-project/requirements_export.csv \
        --output output/test_mixed.pdf
```

### Test Complete Project

```bash
# All documents in sequence
sysdocs --input examples/adns-project/*.md \
        --output output/ADNS_Complete_Documentation.pdf
```

### Test Determinism (FR-06, NFR-01)

```bash
# Generate output
sysdocs --input examples/adns-project/12_SRS_ADNS.md --output test_output.pdf

# Compare against expected result (byte-for-byte)
sha256sum test_output.pdf examples/expected-results/individual/01_markdown.pdf

# Or test repeated generation
sysdocs --input examples/adns-project/12_SRS_ADNS.md --output run1.pdf
sysdocs --input examples/adns-project/12_SRS_ADNS.md --output run2.pdf
sha256sum run1.pdf run2.pdf  # Should be identical
```

### Test Cross-Platform (FR-14)

```bash
# Docker on Linux
docker run sysdocs:latest \
  --input /workspace/examples/adns-project/12_SRS_ADNS.md \
  --output /workspace/linux.pdf

# Docker on Windows
docker run sysdocs:latest \
  --input /workspace/examples/adns-project/12_SRS_ADNS.md \
  --output /workspace/windows.pdf

# Docker on macOS
docker run sysdocs:latest \
  --input /workspace/examples/adns-project/12_SRS_ADNS.md \
  --output /workspace/macos.pdf

# All should match the expected result (byte-for-byte)
sha256sum linux.pdf windows.pdf macos.pdf \
          examples/expected-results/individual/01_markdown.pdf
# All four hashes should be identical
```

### Automated Testing

See `TEST_PLAN.md` in this directory for comprehensive test specification.

Tests use **byte-for-byte comparison** (SHA256 hashes) of generated PDFs against expected results in `expected-results/`.
---

## File Formats & Testing Artifacts

### Supported Input Formats (FR-01 Compliance)

The ADNS project includes test artifacts in all formats required by FR-01:

| Format | Extension | Status | Example Files | Purpose |
|--------|-----------|--------|---------------|----------|
| **Markdown** | `.md` | ✅ Complete | 8 documents | Primary documentation format |
| **LaTeX** | `.tex` | ✅ Complete | `21_SDD_ADNS_Software.tex` | Technical docs with equations |
| **Plain Text** | `.txt` | ✅ Complete | `22_HDD_*.txt`, `23_ICD_*.txt` | Simple structured documents |
| **CSV** | `.csv` | ✅ Complete | 5 data files | Tabular data, requirements, risks |
| **JSON** | `.json` | ✅ Complete | `13_RTM_ADNS.json` | Structured data interchange |
| **XML** | `.xml` | ✅ Complete | `project_metadata.xml` | Hierarchical project data |
| **SVG** | `.svg` | ✅ Complete | `architecture_diagram.svg` | Vector graphics, diagrams |

### File Format Details

| Format | Purpose | Example Files | Use Case |
|--------|---------|---------------|----------|
| **Markdown (.md)** | Primary documentation | All narrative docs | Human-readable technical docs |
| **LaTeX (.tex)** | Mathematical/technical docs | Software Design Document | Equations, algorithms, formal specs |
| **Plain Text (.txt)** | Simple structured docs | Hardware Design, Interface Control | Legacy system compatibility |
| **CSV (.csv)** | Tabular data | RTM, requirements, risks, test results, glossary | Data import/export, spreadsheet integration |
| **JSON (.json)** | Structured data | RTM data | API integration, tooling |
| **XML (.xml)** | Hierarchical data | Project metadata | Legacy systems, DOORS integration |
| **SVG (.svg)** | Vector graphics | Architecture diagrams | Scalable technical illustrations |

### Markdown Features Tested

| Feature | Coverage | Example Location |
|---------|----------|------------------|
| **Headers** (H1-H6) | ✅ Extensive | All documents |
| **Tables** (simple & complex) | ✅ Extensive | All documents (200+ tables) |
| **Lists** (ordered, unordered, nested) | ✅ Extensive | All documents |
| **Code blocks** | ✅ Moderate | ConOps, STP (scenarios) |
| **Bold, italic, inline code** | ✅ Extensive | All documents |
| **Horizontal rules** | ✅ Moderate | Section separators |
| **ASCII diagrams** | ✅ Good | SEMP, SRS, ConOps |
| **Special characters** | ✅ Good | ✅ ⏳ ❌ (status indicators) |
| **Long documents** | ✅ Excellent | ConOps (~700 lines), STP (~800 lines) ||
| **Lists** (ordered, unordered, nested) | ✅ Extensive | All documents |
| **Code blocks** | ✅ Moderate | ConOps, STP (scenarios) |
| **Bold, italic, inline code** | ✅ Extensive | All documents |
| **Horizontal rules** | ✅ Moderate | Section separators |
| **ASCII diagrams** | ✅ Good | SEMP, SRS, ConOps |
| **Special characters** | ✅ Good | ✅ ⏳ ❌ (status indicators) |
| **Long documents** | ✅ Excellent | ConOps (~700 lines), STP (~800 lines) |

---

## File Complexity Analysis

Test artifacts vary in complexity to ensure comprehensive testing:

| File | Format | Lines | Tables | Equations | Diagrams | Complexity |
|------|--------|-------|--------|-----------|----------|------------|
| `01_SEMP_ADNS.md` | Markdown | ~600 | 15 | 0 | 3 ASCII | High |
| `11_ConOps_ADNS.md` | Markdown | ~700 | 18 | 0 | 5 ASCII | High |
| `12_SRS_ADNS.md` | Markdown | ~400 | 16 | 0 | 2 ASCII | Medium |
| `21_SDD_ADNS_Software.tex` | LaTeX | ~350 | 3 | 8 | 0 | Medium |
| `22_HDD_ADNS_Hardware.txt` | Text | ~500 | 4 | 0 | 2 ASCII | High |
| `23_ICD_ADNS_Sensors.txt` | Text | ~400 | 3 | 0 | 0 | Medium |
| `31_STP_ADNS.md` | Markdown | ~800 | 22 | 0 | 2 ASCII | Very High |
| `40_FMEA_ADNS.md` | Markdown | ~650 | 14 | 0 | 0 | High |
| `requirements_export.csv` | CSV | 16 | 1 | 0 | 0 | Low |
| `test_results.csv` | CSV | 16 | 1 | 0 | 0 | Low |
| `13_RTM_ADNS.json` | JSON | ~80 | 0 | 0 | 0 | Medium |
| `project_metadata.xml` | XML | ~60 | 0 | 0 | 0 | Low |

**Key Testing Features:**

- **LaTeX Content**: Mathematical equations (EKF, A* algorithm), matrices, Greek symbols
- **Plain Text**: ASCII diagrams, structured tables, technical specifications
- **Varied Lengths**: Short (60 lines) to very long (800 lines)
- **Table Density**: From 0 to 22 tables per document
- **Content Types**: Management, requirements, design, test, safety, data

---

### SE Document Types Tested

| Document Type | Standard | Status |
|---------------|----------|--------|
| Management Plan (SEMP) | ISO/IEC 15288 | ✅ Complete |
| Stakeholder Requirements | ISO/IEC 29148 | ✅ Complete |
| Concept of Operations | IEEE 1362 | ✅ Complete |
| System Requirements | ISO/IEC 29148 | ✅ Complete |
| Test Plan | IEEE 829 | ✅ Complete |
| FMEA | MIL-STD-1629 | ✅ Complete |
| Architecture Description | ISO/IEC 42010 | ⏳ TODO |
| Design Documents | IEEE 1016 | ⏳ TODO |
| Traceability Matrix | - | ⏳ TODO |

---

## Document Complexity Levels

| Document | Lines | Tables | Complexity | Best For Testing |
|----------|-------|--------|------------|------------------|
| `00_PROJECT_INDEX.md` | ~200 | 8 | Medium | Table rendering, linking |
| `01_SEMP_ADNS.md` | ~600 | 15 | High | Large documents, varied content |
| `10_StRS_ADNS.md` | ~450 | 12 | Medium | Requirements tables, traceability |
| `11_ConOps_ADNS.md` | ~700 | 18 | High | Longest doc, complex scenarios |
| `12_SRS_ADNS.md` | ~400 | 16 | Medium | Technical requirements, standards |
| `31_STP_ADNS.md` | ~800 | 22 | High | Most tables, detailed test cases |
| `40_FMEA_ADNS.md` | ~650 | 14 | High | Wide tables, risk calculations |

---

## Standards Compliance

All documents demonstrate compliance with industry standards:

- **INCOSE SE Handbook v4** - Overall SE process
- **ISO/IEC 15288** - System lifecycle processes
- **ISO/IEC 29148** - Requirements engineering
- **IEEE 1362** - Concept of Operations
- **IEEE 829** - Software Test Documentation
- **IEEE 1016** - Software Design Descriptions
- **MIL-STD-1629** - FMEA procedures
- **DO-178C** - Airborne software (referenced)
- **ISO 26262** - Functional safety (referenced)

---

## Fictional Project Details

**ADNS** (Autonomous Drone Navigation System) is a completely fictional project created for testing purposes.

- **Domain**: Aerospace / Unmanned Systems
- **Timeline**: Jan 2025 - Jun 2026 (18 months)
- **Budget**: $2.4M USD
- **Team**: 12 engineers
- **Status**: Beta phase (Dec 2025)
- **Customer**: AeroTech Corp (fictional)
- **Regulator**: FAA (real agency, fictional engagement)

All technical content, names, dates, and requirements are invented for realistic SE documentation examples.

---

## Contributing

To add more example documents:

1. Follow the naming convention: `[NN]_[TYPE]_ADNS.md`
2. Use consistent formatting (headers, tables, lists)
3. Include diverse Markdown features
4. Reference other documents (traceability)
5. Update `00_PROJECT_INDEX.md` with the new document
6. Update this README with document details
7. Test rendering with SysDocs

---

## License

These are **example documents** for the SysDocs project. Content is entirely fictional and for testing/demonstration purposes only.
