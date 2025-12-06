# Test Case Index - Manifest-Based Assembly

This document provides an index of test cases for manifest-based document assembly.

## Test Case Summary

| ID | Name | Requirements | Status | Location |
|----|------|--------------|--------|----------|
| TC-11 | Multi-File SEMP Assembly | FR-16, FR-17, FR-18 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC11 |
| TC-12 | Single-File Section Extraction | FR-16, FR-17 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC12 |
| TC-13 | Manifest Determinism | FR-19, NFR-01 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC13 |
| TC-14 | Section Extraction Patterns | FR-17 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC14 |
| TC-15 | Multi-File Ordering | FR-18 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC15 |
| TC-16 | Section Remapping | FR-16, FR-18 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC16 |
| TC-17 | Cross-Platform Determinism | FR-14, FR-19 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC17 |
| TC-18 | Invalid Manifest Handling | FR-16 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC18 |
| TC-19 | Missing Section Handling | FR-17 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC19 |
| TC-20 | Template Configuration | FR-16 | Defined (Skip) | FR16_ManifestBasedAssemblyTests.cs::TC20 |

## Test Execution

### Local Execution

```bash
# Run all manifest tests
dotnet test --filter "Category=Manifest"

# Or use Makefile
make test-manifest
```

### CI/CD Execution

Manifest tests are automatically executed in:
- **ci.yml**: Build job on all platforms (Linux, Windows, macOS)
- **release.yml**: Build-linux job (must pass before Docker image creation)

## Test Input Files

### SkyNet Repository Example
- `examples/skynet-repo/README.md` - Sections 1.1-1.10
- `examples/skynet-repo/project_description.md` - Sections 2.1-2.8
- `examples/skynet-repo/docs/engineering_process.md` - Sections 3.1-3.4
- `examples/skynet-repo/requirements/stakeholder_requirements.md` - Sections 4.1-4.4
- `examples/skynet-repo/sysdocs.manifest.json` - Advanced manifest

### ADNS Project Example
- `examples/adns-project/*.md` - Individual SE documents
- `examples/adns-project/sysdocs.manifest.json` - Simple manifest

## Expected Results

Expected baseline PDFs will be stored in:
- `examples/expected-results/manifests/01_SEMP_SkyNet.pdf`
- `examples/expected-results/manifests/10_StRS_SkyNet.pdf`

These will be generated once PDF generation is implemented.

## Verification Methods

All tests use **SHA256 hash comparison** for byte-for-byte verification:

```csharp
var expectedHash = await ComputeSha256HashAsync(expectedOutputPath);
var actualHash = await ComputeSha256HashAsync(actualOutputPath);
actualHash.Should().Be(expectedHash);
```

This ensures:
- ✅ Determinism (repeated runs produce identical output)
- ✅ Cross-platform consistency (same hash on Linux/Windows/macOS)
- ✅ Traceability (expected results committed to git)

## Test Categories

Manifest tests are categorized with multiple traits:

```csharp
[TestCategory(TestCategories.Manifest)]        // Primary category
[TestCategory(TestCategories.Determinism)]     // Also tests determinism
[TestCategory(TestCategories.CrossPlatform)]   // Some tests are cross-platform
```

This enables flexible test filtering:
```bash
dotnet test --filter "Category=Manifest"
dotnet test --filter "Category=Manifest&Category=Determinism"
dotnet test --filter "Category=CrossPlatform"
```

## Implementation Status

**Current State**: All tests defined with `[Fact(Skip = "Pending SysDocs PDF generation implementation")]`

**Next Steps**:
1. Implement manifest processing classes
2. Implement PDF generation
3. Generate expected results
4. Remove `Skip` attributes
5. Verify all tests pass

---

**Related Documentation**:
- [MANIFEST_BASED_ASSEMBLY.md](MANIFEST_BASED_ASSEMBLY.md) - Feature documentation
- [MANIFEST_IMPLEMENTATION_SUMMARY.md](MANIFEST_IMPLEMENTATION_SUMMARY.md) - Implementation summary
- [TEST_PLAN.md](../examples/TEST_PLAN.md) - Overall test plan
- [CI_CD_TEST_INTEGRATION.md](CI_CD_TEST_INTEGRATION.md) - CI/CD integration guide
