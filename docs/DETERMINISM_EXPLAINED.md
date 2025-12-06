# Determinism in SysDocs

**📖 Navigation**: [🏠 README](../README.md) | [➡️ Next: Deterministic Builds](DETERMINISTIC_BUILDS.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This document explains the two types of determinism in SysDocs and how they are verified.

## Overview

SysDocs implements **two distinct types of determinism**:

1. **Build Determinism**: The tool binary builds identically
2. **Output Determinism**: The tool produces identical PDF files

These are **independent concepts** verified in different ways.

---

## 1. Build Determinism (BR-02)

**What**: The SysDocs tool binary itself builds byte-for-byte identically on different systems.

**Why**: Ensures the tool can be independently verified and rebuilt from source with cryptographic guarantees.

**Scope**: Linux distributions only (Fedora and Debian)

**Requirements**: 
- BR-02: Identical binary artifacts (same SHA256)
- C-04: Official builds use Nix for reproducibility

**CI/CD Verification**: `nix-cross-distro` job
```yaml
# Builds tool on Fedora and Debian
# Compares SHA256 hashes of resulting binaries
# ✅ Pass: Both hashes match
# ❌ Fail: Hashes differ
```

**Limitation**: Windows and macOS not verified (see [DEV-003](../DEVIATIONS.md#dev-003))
- Reason: Complexity of cross-OS Nix builds
- Impact: Developers can use .NET SDK on any platform
- Acceptable: Tool qualification standards focus on Linux deployment

**Local Verification**:
```bash
# On Fedora
nix build
sha256sum result  # e.g., abc123...

# On Debian
nix build
sha256sum result  # Should also be abc123...
```

---

## 2. Output Determinism (FR-06, FR-07, NFR-01)

**What**: The SysDocs tool produces byte-for-byte identical PDF files when given the same inputs, regardless of platform.

**Why**: Critical for tool qualification - users need confidence that the same documentation will be produced everywhere.

**Scope**: All platforms (Windows 10+11, Linux, macOS)

**Requirements**:
- FR-06: Generated PDFs must be deterministic across all platforms
- FR-07: Identical text, images, ordering, metadata, layout
- NFR-01: 100% reproducible output across all platforms

**CI/CD Verification**: `output-determinism-check` job
```yaml
# On Windows, Linux, macOS:
#   1. Generate PDF from test input (run 1)
#   2. Generate PDF from same input (run 2)
#   3. Compare hashes (run 1 vs run 2)
#   4. Upload hash for cross-platform comparison
# ✅ Pass: All platforms produce same hash
# ❌ Fail: Any platform differs
```

**What This Means for Users**:
- Developer on Windows generates `requirement.pdf` → hash `xyz789`
- Reviewer on macOS generates same document → hash `xyz789` ✅
- CI/CD on Linux generates same document → hash `xyz789` ✅
- **Result**: Everyone can independently verify the documentation

**Implementation**:
- Deterministic rendering libraries (QuestPDF)
- Normalized timestamps (fixed epoch)
- Sorted collections (consistent ordering)
- Platform-independent fonts and resources
- No reliance on system-specific APIs

**Local Verification**:
```bash
# Generate PDF twice from same input
dotnet run --project src/SysDocs.Cli -- generate input.md --output out1.pdf
dotnet run --project src/SysDocs.Cli -- generate input.md --output out2.pdf

# Compare hashes
sha256sum out1.pdf out2.pdf
# Should be identical

# Share hash with colleague on different OS
# Their hash should match yours
```

---

## Comparison Table

| Aspect | Build Determinism (BR-02) | Output Determinism (FR-06, FR-07, NFR-01) |
|--------|---------------------------|-------------------------------------------|
| **What** | Tool binary builds identically | Tool produces identical PDFs |
| **Platforms** | Linux only (Fedora, Debian) | All (Windows, Linux, macOS) |
| **Verification** | Nix cross-distro hash comparison | PDF hash comparison across OS |
| **CI/CD Job** | `nix-cross-distro` | `output-determinism-check` |
| **User Impact** | Developers can verify tool source | Users can verify documentation |
| **Tool Qualification** | Source code traceability | Output artifact validation |
| **Developer Requirement** | Optional (can use .NET SDK) | Automatic (tool behavior) |

---

## CI/CD Verification Summary

### Build Determinism Checks

1. **nix-build** (Ubuntu + macOS)
   - Builds tool with Nix on different platforms
   - Calculates and records hashes
   - Informational only (not enforcement)

2. **nix-cross-distro** (Fedora + Debian)
   - Builds tool in Docker containers
   - Compares hashes across distros
   - **Enforced**: Job fails if hashes differ
   - Verifies BR-02

3. **determinism-check** (.NET on Linux)
   - Builds tool twice with .NET
   - Compares DLL hashes
   - Verifies .NET compiler determinism

### Output Determinism Checks

1. **output-determinism-check** (Windows + Linux + macOS)
   - Generates test PDF twice on each platform
   - Verifies same-platform determinism
   - Uploads hashes for cross-platform comparison
   - **Enforced**: Job fails if output differs within platform
   - Verifies FR-06, FR-07, NFR-01

2. **cross-platform-hash-verification**
   - Compares PDF hashes across all platforms
   - Ensures Windows hash == Linux hash == macOS hash
   - **Future**: Will be fully automated
   - **Current**: Manual verification via artifacts

---

## For Tool Qualification

### Aerospace/Automotive Standards (DO-330, ISO 26262)

**Build Determinism (BR-02)**:
- ✅ Source code can be independently rebuilt
- ✅ Binary artifacts have cryptographic proof
- ✅ Linux deployment target fully verified
- ⚠️ Cross-OS build verification not required (DEV-003)

**Output Determinism (FR-06, FR-07, NFR-01)**:
- ✅ Documentation artifacts are reproducible
- ✅ Cross-platform verification proves consistency
- ✅ Independent verification possible
- ✅ No platform-specific behavior

**Conclusion**: Both types of determinism support tool qualification:
- **BR-02**: Proves tool source is reliable (Linux-focused)
- **FR-06/FR-07/NFR-01**: Proves tool output is reliable (all platforms)

---

## Quick Reference

**"Is the tool itself deterministic?"**
→ Yes, on Linux distros (BR-02, verified via Nix)

**"Does the tool produce deterministic output?"**
→ Yes, on all platforms (FR-06, FR-07, NFR-01, verified via CI/CD)

**"Can I use Windows/macOS for development?"**
→ Yes! Output determinism is guaranteed regardless of platform

**"Do I need Nix on Windows?"**
→ No! Use .NET SDK. Nix is only for Linux build verification (BR-02)

**"Will my PDF match my colleague's PDF?"**
→ Yes! Same inputs → same PDF hash (FR-06, NFR-01)

**"How do I verify my output is deterministic?"**
→ Generate PDF twice, compare hashes. See "Local Verification" above.

---

## Related Documentation

- [REQUIREMENTS.md](../REQUIREMENTS.md) - Full requirements (FR-06, FR-07, NFR-01, BR-02)
- [REQUIREMENTS_MATRIX.md](../REQUIREMENTS_MATRIX.md) - V&V status of each requirement
- [DEVIATIONS.md](../DEVIATIONS.md) - DEV-003 explains build determinism scope
- [DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md) - How to use Nix for builds
- [BRANCH_STRATEGY.md](../BRANCH_STRATEGY.md) - CI/CD pipeline verification steps

---

**Last Updated**: December 6, 2025  
**Status**: Active - Two types of determinism fully defined and verified

