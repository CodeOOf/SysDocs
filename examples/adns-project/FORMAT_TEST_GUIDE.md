# Example Documents - File Format Reference

This document demonstrates all input formats required by FR-01.

## Supported Input Formats (FR-01)

| Format | Extension | Purpose | Example File |
|--------|-----------|---------|--------------|
| **Markdown** | `.md` | Primary documentation | `*.md` (all documents) |
| **LaTeX** | `.tex` | Technical documents with equations | `21_SDD_ADNS_Software.tex` |
| **Plain Text** | `.txt` | Simple structured docs | `22_HDD_ADNS_Hardware.txt`, `23_ICD_ADNS_Sensors.txt` |
| **CSV** | `.csv` | Tabular data | `13_RTM_ADNS.csv`, `requirements_export.csv`, etc. |
| **JSON** | `.json` | Structured data | `13_RTM_ADNS.json` |
| **XML** | `.xml` | Hierarchical data | `project_metadata.xml` |
| **SVG** | `.svg` | Vector graphics | `images/architecture_diagram.svg` |

## File Inventory

### Markdown Documents (.md)
- `00_PROJECT_INDEX.md` - Document index
- `01_SEMP_ADNS.md` - Systems Engineering Management Plan (~600 lines)
- `10_StRS_ADNS.md` - Stakeholder Requirements (~450 lines)
- `11_ConOps_ADNS.md` - Concept of Operations (~700 lines)
- `12_SRS_ADNS.md` - System Requirements (~400 lines)
- `13_RTM_ADNS.md` - Requirements Traceability Matrix
- `31_STP_ADNS.md` - System Test Plan (~800 lines)
- `40_FMEA_ADNS.md` - FMEA (~650 lines)

### LaTeX Documents (.tex)
- `21_SDD_ADNS_Software.tex` - Software Design Document with equations

### Plain Text Documents (.txt)
- `22_HDD_ADNS_Hardware.txt` - Hardware Design Document
- `23_ICD_ADNS_Sensors.txt` - Interface Control Document

### CSV Files (.csv)
- `13_RTM_ADNS.csv` - Requirements traceability
- `requirements_export.csv` - Requirements database
- `risk_register.csv` - Risk management data
- `test_results.csv` - Test execution results
- `90_Glossary_ADNS.csv` - Technical glossary

### JSON Files (.json)
- `13_RTM_ADNS.json` - Requirements traceability (structured)

### XML Files (.xml)
- `project_metadata.xml` - Project metadata

### Images (.svg)
- `images/architecture_diagram.svg` - System architecture diagram

## Testing FR-01 Compliance

**Note**: Actual test cases are defined in the test code (see `tests/` directory), not as command-line examples. This document provides reference information about available test artifacts.

### Test Artifacts Available

The following files are available for automated testing:

1. **Markdown files** (.md): 8 documents covering all SE phases
2. **LaTeX files** (.tex): 1 document with mathematical equations
3. **Plain text files** (.txt): 2 documents with ASCII diagrams
4. **CSV files** (.csv): 5 data files with tabular content
5. **JSON files** (.json): 1 structured data file
6. **XML files** (.xml): 1 hierarchical data file
7. **SVG files** (.svg): 1 vector graphics file

### Expected Test Outcomes

All format import tests should verify:
1. ✅ Successfully parse the input format
2. ✅ Convert to internal unified model
3. ✅ Generate valid PDF output
4. ✅ Produce deterministic output (FR-06)
5. ✅ Include all content from source files

### FR-01 Verification Matrix

| Format | Test File | Status | Notes |
|--------|-----------|--------|-------|
| Markdown (.md) | `12_SRS_ADNS.md` | ✅ Ready | Primary format |
| LaTeX (.tex) | `21_SDD_ADNS_Software.tex` | ✅ Ready | Equations, tables |
| Plain Text (.txt) | `22_HDD_ADNS_Hardware.txt` | ✅ Ready | ASCII diagrams |
| Plain Text (.txt) | `23_ICD_ADNS_Sensors.txt` | ✅ Ready | Technical specs |
| CSV (.csv) | `requirements_export.csv` | ✅ Ready | 15 requirements |
| CSV (.csv) | `test_results.csv` | ✅ Ready | 15 test cases |
| JSON (.json) | `13_RTM_ADNS.json` | ✅ Ready | Structured traceability |
| XML (.xml) | `project_metadata.xml` | ✅ Ready | Project data |
| SVG (.svg) | `architecture_diagram.svg` | ✅ Ready | System diagram |

## File Complexity Analysis

| File | Format | Lines | Tables | Equations | Diagrams | Complexity |
|------|--------|-------|--------|-----------|----------|------------|
| `01_SEMP_ADNS.md` | Markdown | ~600 | 15 | 0 | 3 ASCII | High |
| `11_ConOps_ADNS.md` | Markdown | ~700 | 18 | 0 | 5 ASCII | High |
| `21_SDD_ADNS_Software.tex` | LaTeX | ~300 | 3 | 8 | 0 | Medium |
| `22_HDD_ADNS_Hardware.txt` | Text | ~500 | 4 | 0 | 2 ASCII | High |
| `31_STP_ADNS.md` | Markdown | ~800 | 22 | 0 | 2 ASCII | Very High |
| `40_FMEA_ADNS.md` | Markdown | ~650 | 14 | 0 | 0 | High |
| `requirements_export.csv` | CSV | 16 | 1 | 0 | 0 | Low |
| `13_RTM_ADNS.json` | JSON | ~80 | 0 | 0 | 0 | Medium |

## Content Diversity

### Text Formatting
- ✅ Headers (H1-H6)
- ✅ Bold, italic, inline code
- ✅ Ordered lists, unordered lists, nested lists
- ✅ Tables (simple and complex)
- ✅ Code blocks
- ✅ Blockquotes
- ✅ Horizontal rules
- ✅ Special characters (✅ ⏳ ❌)

### Mathematical Content
- ✅ LaTeX equations (inline and display)
- ✅ Matrices
- ✅ Greek symbols
- ✅ Subscripts/superscripts

### Structured Data
- ✅ CSV tables
- ✅ JSON objects and arrays
- ✅ XML hierarchy

### Graphics
- ✅ SVG vector graphics
- ✅ ASCII art diagrams

## Determinism Testing (FR-06)

**Test Requirement**: Each input format must produce byte-for-byte identical output when processed multiple times with the same inputs.

**Test Approach**: Automated tests should:
1. Process the same input file(s) multiple times
2. Compute SHA256 hash of each output PDF
3. Verify all hashes are identical
4. Test with each file format: Markdown, LaTeX, CSV, JSON, XML, plain text
5. Test with mixed input formats

**Expected Result**: All hash comparisons must show **identical** SHA256 hashes to verify FR-06 compliance.
