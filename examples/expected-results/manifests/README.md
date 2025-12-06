# Expected Results - Manifest-Based Assembly

This directory contains expected baseline PDFs for manifest-based document assembly tests.

## Test Cases

### TC-11: SkyNet SEMP - Multi-File Assembly
**Input**: `skynet-repo/sysdocs.manifest.json`  
**Sources**:
- `skynet-repo/README.md` (sections 1.1-1.10)
- `skynet-repo/project_description.md` (sections 2.1-2.8)
- `skynet-repo/docs/engineering_process.md` (sections 3.1-3.4)

**Expected Output**: `01_SEMP_SkyNet.pdf`

### TC-12: SkyNet StRS - Single File with Section Extraction
**Input**: `skynet-repo/sysdocs.manifest.json`  
**Sources**:
- `skynet-repo/requirements/stakeholder_requirements.md` (sections 4.1-4.4)

**Expected Output**: `10_StRS_SkyNet.pdf`

### TC-13: ADNS Project - Manifest-Based Assembly
**Input**: `adns-project/sysdocs.manifest.json`  
**Sources**: All files in `adns-project/` as specified in manifest

**Expected Outputs**: All PDFs specified in manifest (01_SEMP_ADNS.pdf, 10_StRS_ADNS.pdf, etc.)

## Generation

These baseline PDFs will be generated once SysDocs PDF generation is implemented:

```bash
# Generate expected results for manifest tests
make generate-manifest-expected-results
```

## Verification

All manifest-based assembly must produce:
- ✅ Byte-for-byte identical output across runs (determinism)
- ✅ Cross-platform identical output (Windows, Linux, macOS)
- ✅ Correct section extraction and ordering
- ✅ Proper section renumbering per manifest
- ✅ Template application (cover pages, TOC, watermarks)

---

**Test Class**: `tests/SysDocs.Tests/Integration/Manifests/FR16_ManifestBasedAssemblyTests.cs`
