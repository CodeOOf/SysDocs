# ADNS Project - Test Plan

This document describes how the ADNS project example files are used for testing SysDocs.

## Test Strategy

The ADNS project serves **two distinct test purposes**:

### 1. Individual File Format Tests (FR-01)
Verify that SysDocs can correctly import each file format independently.

### 2. Folder-Level Integration Test
Verify that SysDocs can process an entire project folder and generate a comprehensive document.

---

## Test Cases

### TC-01: Markdown Format Import
**Requirement**: FR-01 (Markdown import)  
**Input**: `examples/adns-project/12_SRS_ADNS.md`  
**Expected Output**: `examples/expected-results/individual/01_markdown.pdf`  
**Verification**:
- PDF generated successfully
- Contains all sections from markdown
- Tables rendered correctly
- Headers, lists, code blocks formatted properly
- Deterministic output (same hash on repeated runs)

### TC-02: LaTeX Format Import
**Requirement**: FR-01 (LaTeX import)  
**Input**: `examples/adns-project/21_SDD_ADNS_Software.tex`  
**Expected Output**: `examples/expected-results/individual/02_latex.pdf`  
**Verification**:
- PDF generated successfully
- Mathematical equations rendered correctly
- LaTeX tables converted properly
- Special symbols (Greek, math operators) displayed
- Deterministic output

### TC-03: Plain Text Format Import
**Requirement**: FR-01 (Plain text import)  
**Input**: `examples/adns-project/22_HDD_ADNS_Hardware.txt`  
**Expected Output**: `examples/expected-results/individual/03_plaintext.pdf`  
**Verification**:
- PDF generated successfully
- ASCII diagrams preserved or converted appropriately
- Text structure maintained (sections, indentation)
- Tables in text format converted to PDF tables
- Deterministic output

### TC-04: CSV Format Import
**Requirement**: FR-01 (CSV import, if supported)  
**Input**: `examples/adns-project/requirements_export.csv`  
**Expected Output**: `examples/expected-results/individual/04_csv.pdf`  
**Verification**:
- CSV parsed correctly
- Table generated in PDF
- All rows and columns present
- Headers formatted distinctly
- Deterministic output

### TC-05: JSON Format Import
**Requirement**: FR-01 (JSON import, if supported)  
**Input**: `examples/adns-project/13_RTM_ADNS.json`  
**Expected Output**: `examples/expected-results/individual/05_json.pdf`  
**Verification**:
- JSON structure parsed correctly
- Data presented in readable format
- Nested structures handled appropriately
- Deterministic output

### TC-06: XML Format Import
**Requirement**: FR-01 (XML import, if supported)  
**Input**: `examples/adns-project/project_metadata.xml`  
**Expected Output**: `examples/expected-results/individual/06_xml.pdf`  
**Verification**:
- XML hierarchy preserved
- Elements and attributes displayed correctly
- Readable formatting applied
- Deterministic output

### TC-07: Mixed Format Import
**Requirement**: FR-01 (Multiple input formats)  
**Input**: `examples/adns-project/12_SRS_ADNS.md` + `examples/adns-project/requirements_export.csv`  
**Expected Output**: `examples/expected-results/individual/07_mixed.pdf`  
**Verification**:
- Both inputs processed correctly
- Content from both files present
- Appropriate ordering/separation between files
- Format-specific rendering for each input
- Deterministic output

### TC-08: Folder Input (Integration Test)
**Requirement**: Tool capability to process entire folders  
**Input**: `examples/adns-project/` (entire folder)  
**Expected Output**: `examples/expected-results/folder/complete.pdf`  
**Verification**:
- All eligible files in folder processed
- Files ordered logically (by filename, by document type, etc.)
- Mixed formats handled correctly in single PDF
- Table of contents generated (if applicable)
- All content from all files present
- Expected page count met (comprehensive document)
- Deterministic output

### TC-09: Determinism - Repeated Runs (FR-06, NFR-01)
**Requirement**: FR-06 (Deterministic output), NFR-01 (Reproducible builds)  
**Input**: Any file (e.g., `12_SRS_ADNS.md`)  
**Test Procedure**:
1. Generate PDF from input file (run 1)
2. Generate PDF from same input file (run 2)
3. Compute SHA256 hash of both outputs
4. Compare hashes

**Expected Result**: Hashes are identical (byte-for-byte identical PDFs)

### TC-10: Cross-Platform Determinism (FR-14)
**Requirement**: FR-14 (Cross-platform deterministic output)  
**Input**: Any file (e.g., `12_SRS_ADNS.md`)  
**Test Procedure**:
1. Generate PDF on Linux (native)
2. Generate PDF on Windows (via Docker)
3. Generate PDF on macOS (via Docker)
4. Compute SHA256 hash of all three outputs
5. Compare hashes

**Expected Result**: All three hashes are identical

---

## Test Execution

### Manual Testing

```bash
# Individual format tests
sysdocs --input examples/adns-project/12_SRS_ADNS.md \
        --output test_output/01_markdown.pdf

sysdocs --input examples/adns-project/21_SDD_ADNS_Software.tex \
        --output test_output/02_latex.pdf

sysdocs --input examples/adns-project/22_HDD_ADNS_Hardware.txt \
        --output test_output/03_plaintext.pdf

# Folder test
sysdocs --input examples/adns-project/ \
        --output test_output/complete.pdf

# Compare with expected results
diff test_output/01_markdown.pdf expected-results/individual/01_markdown.pdf
# or
sha256sum test_output/01_markdown.pdf expected-results/individual/01_markdown.pdf
```

### Automated Testing

The test suite uses **byte-for-byte comparison** (SHA256 hashes) between generated PDFs and expected result PDFs.

Test files structure in `tests/` directory:

- `tests/test_formats.py` - TC-01 through TC-07 (individual format tests)
- `tests/test_folder_input.py` - TC-08 (entire folder as input)
- `tests/test_determinism.py` - TC-09 (repeated generation)
- `tests/test_cross_platform.py` - TC-10 (Linux, Windows, macOS)

Key verification method:

```python
# tests/test_formats.py
import pytest
import hashlib
from pathlib import Path

EXAMPLES_DIR = Path("examples/adns-project")
EXPECTED_DIR = Path("examples/expected-results/individual")

def compute_hash(filepath):
    """Compute SHA256 hash of file"""
    sha256 = hashlib.sha256()
    with open(filepath, 'rb') as f:
        sha256.update(f.read())
    return sha256.hexdigest()

def test_markdown_format(tmp_path, sysdocs_cli):
    """TC-01: Markdown Format Import"""
    input_file = EXAMPLES_DIR / "12_SRS_ADNS.md"
    output_file = tmp_path / "output.pdf"
    expected_file = EXPECTED_DIR / "01_markdown.pdf"
    
    # Generate PDF
    sysdocs_cli.generate(input=input_file, output=output_file)
    
    # Verify file exists
    # Verify byte-for-byte match (determinism)
    assert compute_hash(output_file) == compute_hash(expected_file)
    # This verifies FR-06 (determinism) and FR-14 (cross-platform consistency)
    # Verify determinism (hash matches expected)
    assert compute_hash(output_file) == compute_hash(expected_file)

def test_latex_format(tmp_path, sysdocs_cli):
    """TC-02: LaTeX Format Import"""
    input_file = EXAMPLES_DIR / "21_SDD_ADNS_Software.tex"
    output_file = tmp_path / "output.pdf"
    expected_file = EXPECTED_DIR / "02_latex.pdf"
    
    sysdocs_cli.generate(input=input_file, output=output_file)
    assert output_file.exists()
    assert compute_hash(output_file) == compute_hash(expected_file)

# ... more tests for other formats

def test_folder_input(tmp_path, sysdocs_cli):
    """TC-08: Folder Input"""
    input_dir = EXAMPLES_DIR
    output_file = tmp_path / "complete.pdf"
    expected_file = Path("examples/expected-results/folder/complete.pdf")
    
    sysdocs_cli.generate(input=input_dir, output=output_file)
    assert output_file.exists()
    assert compute_hash(output_file) == compute_hash(expected_file)
```

---

## Test Data Coverage

### File Formats (FR-01)
- ✅ Markdown (.md) - 8 files
- ✅ LaTeX (.tex) - 1 file (with equations)
- ✅ Plain Text (.txt) - 2 files (with ASCII diagrams)
- ✅ CSV (.csv) - 5 files
- ✅ JSON (.json) - 1 file
- ✅ XML (.xml) - 1 file
- ✅ SVG (.svg) - 1 file (images)

### Content Types
- ✅ Management documents (SEMP)
- ✅ Requirements documents (StRS, SRS)
- ✅ Design documents (SDD, HDD, ICD)
- ✅ Test documents (STP)
- ✅ Safety documents (FMEA)
- ✅ Operations documents (ConOps)
- ✅ Traceability matrices (RTM)
- ✅ Data files (requirements, risks, test results)

### Document Complexity
- ✅ Short documents (~60 lines)
- ✅ Medium documents (~400 lines)
- ✅ Long documents (~800 lines)
- ✅ Simple tables
- ✅ Complex wide tables
- ✅ Mathematical equations (LaTeX)
- ✅ ASCII diagrams
- ✅ SVG vector graphics
- ✅ Nested lists
- ✅ Code blocks
- ✅ Special characters

### SE Standards Coverage
- ✅ INCOSE SE Handbook v4
- ✅ ISO/IEC 15288 (Systems engineering)
- ✅ ISO/IEC 29148 (Requirements)
- ✅ IEEE 1362 (ConOps)
- ✅ IEEE 829 (Test documentation)
- ✅ IEEE 1016 (Design documentation)
- ✅ MIL-STD-1629 (FMEA)

---

## Success Criteria

### Individual Format Tests (TC-01 through TC-07)
- ✅ All formats generate valid PDF output
- ✅ Content from input files present in output
- ✅ Format-specific features rendered correctly (equations, tables, diagrams)
- ✅ Deterministic output (identical hash on repeated runs)

### Folder Test (TC-08)
- ✅ All files in folder processed
- ✅ Comprehensive PDF generated (50+ pages expected)
- ✅ Logical ordering of documents
- ✅ Deterministic output

### Determinism Tests (TC-09, TC-10)
- ✅ Same input produces identical output (byte-for-byte)
- ✅ Cross-platform output identical (Linux, Windows, macOS via Docker)

---

## Maintenance

### When to Update Test Data
1. New file format support added → Add new test file
2. Enhanced rendering capabilities → Regenerate expected results
3. Bug fixes affecting output → Regenerate expected results
4. New SE document types needed → Add to ADNS project

### Regenerating Expected Results

```bash
# Regenerate all expected results
./scripts/regenerate_expected_results.sh

# Or manually for specific test
sysdocs --input examples/adns-project/12_SRS_ADNS.md \
        --output examples/adns-project/expected-results/individual/01_markdown.pdf
```

**Important**: Document the reason for regenerating expected results in the commit message.

### Version Control
- Source files (.md, .tex, .txt, .csv, .json, .xml) → Commit to git
- Expected result PDFs → Either commit with git-lfs or generate in CI/CD
- Checksums (SHA256 hashes) → Commit to git (`expected-results/checksums.txt`)
