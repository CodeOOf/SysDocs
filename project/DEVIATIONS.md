# Technical Deviations

**📖 Navigation**: [⬅️ Back: V&V Strategy](../project/VERIFICATION_VALIDATION.md) | [🏠 README](README.md) | [➡️ Next: Test Report](reports/TEST_TRACEABILITY.md) | [🗺️ Docs Navigation](docs/DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This document tracks intentional deviations from requirements with justification.  
> For requirements affected by each deviation, see: [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)

This document tracks intentional deviations from the requirements specification with justification and remediation plans.

---

## DEV-001: Target Framework - .NET 8 LTS with .NET 10 Runtime (TRANSITIONAL)

**Date**: December 6, 2025  
**Status**: ⚠️ Active - Temporary (Transitioning to .NET 10)  
**Severity**: Low  
**Affects Requirements**: FR-15 (LTS platform implementation)

### Description

**TRANSITIONAL STATE**: FR-15 requires using an LTS platform for compilers/runtime. The chosen implementation solution is .NET 10 (LTS platform, released December 2025).

The project currently **targets .NET 8.0 (LTS)** for compilation but **runs on .NET 10.0 SDK/runtime**. This is a temporary state during ecosystem transition.

### Reason

.NET 10 was recently released (December 2025) and the NuGet package ecosystem has not yet published compatible versions for most third-party libraries and even some Microsoft.Extensions.* packages. Attempting to target net10.0 directly results in package resolution failures.

.NET provides **forward compatibility** - assemblies compiled for .NET 8 run without modification on .NET 10 runtime. This is a core design principle of .NET and is guaranteed by Microsoft.

### Implementation Context

**Requirement**: FR-15 mandates LTS platform (compiler/runtime) for long-term support  
**Solution Chosen**: .NET 10 (LTS platform - 3 year support cycle)  
**Current State**: Transitional targeting during ecosystem maturation

### Migration Path

**Current State (Transitional)**: 
- Target Framework: `net8.0` (Previous LTS platform)
- Runtime: .NET 10.0 SDK  
- Package Versions: .NET 8 LTS compatible
- **Status**: Deviation from FR-15 implementation solution

**Target State (Once ecosystem ready - Est. Q2 2026)**:
- Target Framework: `net10.0` (Current LTS platform)
- Runtime: .NET 10.0 SDK  
- Package Versions: .NET 10 compatible
- **Status**: ✅ Fully compliant with FR-15 implementation

### Resolution Criteria

This deviation will be resolved when:
1. ✅ All critical NuGet dependencies support .NET 10
2. ✅ Project successfully compiles targeting `net10.0`
3. ✅ All tests pass on .NET 10 target
4. ✅ Nix flake updated to use `dotnet-sdk_10`

### Package Version Priority Order

1. **Primary**: Use packages targeting current .NET version (net10.0) - *Not yet available*
2. **Fallback 1**: Use packages from previous STS version (.NET 9) - *Limited availability*
3. **Fallback 2 (ACTIVE)**: Use packages from previous LTS version (.NET 8) - ✅ **Full ecosystem support**
4. **Last Resort**: Use packages from earlier LTS (.NET 6) if necessary

### Impact Assessment

✅ **Functional**: No impact - .NET 8 assemblies run identically on .NET 10  
✅ **Performance**: No impact - JIT compilation optimized for .NET 10  
✅ **Deterministic Output**: No impact - Build remains deterministic  
✅ **Cross-Platform**: No impact - All platforms supported  
✅ **Tool Qualification**: No impact - Dependencies are pinned and versioned  
✅ **Security**: No impact - .NET 10 runtime includes latest security patches

---

## DEV-002: Nix Uses .NET 8 SDK for Deterministic Builds

**Date**: December 6, 2025  
**Status**: Active - Intentional  
**Severity**: Low  
**Affects Requirements**: C-04, BR-01 (Build system requirements)

### Description

The Nix flake uses `dotnet-sdk_8` instead of `dotnet-sdk_10` for the official build system.

### Reason

1. **nixpkgs Availability**: .NET 10 is very new (December 2025) and may not yet be available in stable nixpkgs channels
2. **Determinism Priority**: Using .NET 8 SDK from pinned nixpkgs (24.05) ensures maximum reproducibility
3. **Package Ecosystem**: All NuGet packages are .NET 8 compatible (see DEV-001)
4. **Forward Compatibility**: .NET 8 SDK can build and run on .NET 10 runtime

### Strategy

**Current State**:
- Nix Build: Uses dotnet-sdk_8 from nixpkgs-24.05
- Target Framework: net8.0
- Runtime: .NET 8 or .NET 10 (forward compatible)

**Future State (when .NET 10 in stable nixpkgs)**:
- Nix Build: Uses dotnet-sdk_10
- Target Framework: net10.0
- Runtime: .NET 10

### Impact Assessment

✅ **Reproducibility**: Enhanced - stable SDK in stable nixpkgs  
✅ **Cross-Platform**: No impact - SDK available on all platforms  
✅ **Deterministic**: No impact - Nix ensures exact version used  
✅ **Functional**: No impact - Forward compatibility guaranteed

### Verification

```bash
# Build produces identical hashes regardless of .NET SDK version
nix build .#docker
sha256sum result  # Always the same for given source
```

### Remediation Plan

Monitor nixpkgs for .NET 10 availability:
```bash
# Check when .NET 10 is in nixpkgs
nix search nixpkgs dotnet-sdk_10
```

Update flake.nix when stable and test for identical reproducibility.

---
### Runtime Verification

```powershell
# Verify .NET 10 SDK installed
PS> dotnet --version
10.0.100

# Verify application runs on .NET 10 runtime
PS> dotnet run --project src/SysDocs.Cli
# (Uses .NET 10.0 runtime even though compiled for net8.0)
```

### Affected Components

**All projects compile to net8.0**:
- `SysDocs.Core.dll` - Targets net8.0, runs on .NET 10
- `SysDocs.Cli.exe` - Targets net8.0, runs on .NET 10  
- `SysDocs.Templates.dll` - Targets net8.0, runs on .NET 10
- `SysDocs.Tests.dll` - Targets net8.0, runs on .NET 10

### Remediation Plan

**Phase 1** (Q1 2026): Monitor NuGet for .NET 9 STS package releases  
**Phase 2** (Q3 2026): Upgrade to .NET 10-specific packages when available  
**Phase 3** (Ongoing): Establish automated package update process

### Testing

- ✅ All functionality tested on .NET 10 runtime
- ✅ Deterministic builds verified
- ✅ Cross-platform compatibility confirmed (Windows, Linux, macOS)
- ✅ Docker containerization working with .NET 10

### Compliance Statement

This deviation is acceptable per industry best practices:
- Microsoft guarantees forward compatibility across .NET versions
- Using LTS packages provides better stability than bleeding-edge
- All dependencies remain pinned for deterministic builds
- No functional degradation occurs

### Review Schedule

- **Monthly**: Check for updated package versions
- **Quarterly**: Assess upgrade feasibility
- **Annually**: Full dependency audit

---

## DEV-003: Build Determinism - Linux Only (Docker Deployment Model)

**Date**: December 6, 2025  
**Status**: Active - Intentional  
**Severity**: Low  
**Affects Requirements**: C-04 (build reproducibility), BR-02 (build determinism)

### Description

Official deterministic **builds** are verified **only across Linux distributions** (Fedora and Debian). The production deployment is a **Linux Docker image** that runs on all platforms via Docker.

**Deployment Model**:
- **Production**: Linux Docker image (built on Linux CI/CD)
- **Development**: Linux native, macOS native, or Windows via WSL2
- **No platform-specific artifacts**: Docker handles cross-platform execution

**Important**: This deviation applies to **build determinism** (the tool binary). **Output determinism** (PDF files generated by the tool) is verified across all platforms per FR-06, FR-07, and NFR-01.

### Two Types of Determinism

1. **Build Determinism (BR-02)**: The Linux Docker image builds identically
   - **Scope**: Linux distributions only (Fedora, Debian)
   - **Verification**: Nix builds produce same hash on different Linux distros
   - **Deployment**: Linux Docker image runs on all platforms via Docker
   - **Status**: ✅ Verified in CI/CD

2. **Output Determinism (FR-06, FR-07, NFR-01)**: The tool produces identical PDFs
   - **Scope**: All platforms (via Docker: Windows, Linux, macOS)
   - **Verification**: Same input → same PDF hash regardless of host platform
   - **Status**: ✅ Verified in CI/CD (`output-determinism-check` job)

### Reason

1. **Docker-First Deployment**: Production is Linux Docker image only
   - Docker provides consistent Linux runtime on all platforms
   - No need for platform-specific binaries
   - Simpler CI/CD and release process

2. **Linux Focus**: Primary deployment target is Linux containers
   - Build once on Linux, run anywhere via Docker
   - Eliminates platform-specific build variations
   - Consistent environment for all users

3. **Development Flexibility**:
   - Linux: Native .NET development
   - macOS: Native .NET development  
   - Windows: WSL2 provides Linux environment
   - All platforms can run via Docker for testing

### Impact Assessment

✅ **Production Deployment**: Linux Docker image on all platforms  
✅ **Build Reproducibility**: Full verification across Linux distros  
✅ **Tool Qualification**: Linux-to-Linux reproducibility verified  
✅ **CI/CD Efficiency**: Single build target (Linux)  
✅ **Developer Experience**: Flexible development platforms  
✅ **User Experience**: Consistent via Docker

### Strategy

**Build Determinism (Linux Docker Image)**:
- BR-02: Verified on Fedora and Debian in CI/CD via `nix-cross-distro` job
- Docker image: Built on Linux, deterministic hash verified
- Release: Single Docker image for all platforms
- **Status**: ✅ Fully verified

**Output Determinism (PDF Files)**:
- FR-06, FR-07, NFR-01: Verified via `output-determinism-check` job
- Docker on Windows: Same PDFs as Docker on Linux/macOS
- Development builds: Same PDFs as production Docker
- **Status**: ✅ Fully verified

**Developer Workflows**:
- **Linux Developers**: Native .NET SDK or Nix for builds
- **macOS Developers**: Native .NET SDK or Nix for builds
- **Windows Developers**: WSL2 + .NET SDK or Docker for testing
- **All Developers**: Can run production Docker image locally

### Verification

**Build Determinism** (Linux Docker Image):
```bash
# CI/CD verifies identical Docker image hash across Linux distros
# Fedora build hash == Debian build hash

# Example from GitHub Actions nix-cross-distro job:
# Fedora: docker load < result && docker images --digests
#   sysdocs:0.1.0  sha256:abc123...
# Debian: docker load < result && docker images --digests
#   sysdocs:0.1.0  sha256:abc123...  ✅ Match
```

**Output Determinism** (PDF Files via Docker):
```bash
# Docker on any platform produces identical PDFs
docker run sysdocs:0.1.0 --input test.md --output test.pdf

# Windows: SHA256(test.pdf) = xyz789...
# Linux:   SHA256(test.pdf) = xyz789...  ✅ Match
# macOS:   SHA256(test.pdf) = xyz789...  ✅ Match
```

### Compliance Justification

For tool qualification per DO-330, DO-178C, ISO 26262:
- **Requirement**: Reproducible builds on target platform
- **Target Platform**: Linux Docker image (production deployment)
- **Verification**: Linux-to-Linux reproducibility verified
- **Deployment**: Docker ensures consistent runtime on all platforms
- **Conclusion**: Docker deployment model satisfies cross-platform requirements

### Remediation Plan

**Current**: No remediation planned - Docker deployment model is intentional

This is the **production architecture choice**, not a temporary deviation.

### Review Schedule

- **Annually**: Reassess if cross-OS verification needed
- **On Tool Qualification**: Verify deviation acceptable to auditors

---

## DEV-004: Code Signing Infrastructure - Development Phase (TRANSITIONAL)

**Date**: December 6, 2025  
**Status**: ⚠️ Active - Temporary (Infrastructure Setup)  
**Severity**: Medium  
**Affects Requirements**: NFR-07 (GPG commit signing and artifact signing)

### Description

NFR-07 requires all commits to be GPG-signed and all release artifacts to be digitally signed. During the **development phase**, the signing infrastructure is being established:

**Current State**:
- GPG commit signing: **Optional** (developers configuring locally)
- Release artifact signing: **Manual** (not yet automated in CI/CD)
- Branch protection: **Not yet enabled** for signed commits
- Docker image signing: **Not yet implemented** (Cosign setup pending)

**Target State** (Before Beta Release):
- GPG commit signing: **Required** on `main` branch (enforced by GitHub)
- Release artifact signing: **Automated** in GitHub Actions
- All releases include: `.asc` signatures and signed `SHA256SUMS`
- Docker images signed with Cosign

### Reason

**Open Source Approach**:
- Using GPG (GNU Privacy Guard) for commit and artifact signing
- Free and appropriate for open-source projects
- No certificate purchase required (unlike Authenticode)
- Industry standard (Linux kernel, Python, Debian, etc.)

**Infrastructure Requirements**:
1. **Team GPG Key Setup**: Each developer needs to generate and configure GPG keys
2. **GitHub Integration**: GPG public keys must be added to GitHub accounts
3. **CI/CD Secrets**: Bot GPG key required for automated signing in GitHub Actions
4. **Branch Protection**: Must configure GitHub to require signed commits
5. **Cosign Setup**: Docker image signing with Sigstore Cosign

**Development Phase Flexibility**:
- Allowing time for team members to set up GPG keys
- Testing signing workflows before enforcement
- Building automation scripts before hard requirements

### Implementation Context

**Requirement**: NFR-07 mandates GPG-signed commits and signed release artifacts  
**Current State**: Infrastructure in development, signing is optional/manual  
**Target State**: Fully automated signing with branch protection enforcement

### Impact

**Development Phase**:
- ✅ No impact on development velocity (optional signing)
- ✅ Developers can work without GPG setup initially
- ⚠️ Commits show "Unverified" on GitHub until keys configured
- ⚠️ Manual artifact signing for any test releases

**Production Releases** (After Beta):
- ❌ **Must not release without signatures**
- ✅ Branch protection enforces signed commits
- ✅ Automated signing in GitHub Actions
- ✅ All artifacts include GPG signatures
- ✅ Users can verify authenticity

### Remediation Plan

**Phase 1: Developer Setup** (Completed by team members individually)
1. Generate GPG keys (`gpg --full-generate-key`)
2. Configure Git for signing (`git config --global commit.gpgsign true`)
3. Add GPG public key to GitHub account
4. Test commit signing locally

**Phase 2: CI/CD Infrastructure** (Before Alpha Release)
1. Generate bot GPG key for GitHub Actions
2. Add bot GPG key to GitHub Secrets (`GPG_PRIVATE_KEY`, `GPG_PASSPHRASE`)
3. Update `.github/workflows/release.yml` with signing steps
4. Create `scripts/sign-release.sh` for automated signing
5. Test automated release signing

**Phase 3: Cosign Setup** (Before Alpha Release)
1. Generate Cosign keypair
2. Add Cosign keys to GitHub Secrets
3. Integrate Cosign signing in Docker build workflow
4. Document Cosign verification for users

**Phase 4: Enforcement** (Before Beta Release)
1. Enable branch protection on `main` branch
2. Require signed commits via GitHub settings
3. Update contributing guidelines
4. Verify all team members have GPG configured

**Phase 5: Documentation** (Before Beta Release)
1. Update SECURITY.md with GPG key information
2. Publish GPG public keys in releases
3. Document verification procedures for users
4. Add signature verification to README

### Acceptance Criteria

This deviation will be closed when:
- [ ] All team members have GPG keys configured and added to GitHub
- [ ] Branch protection enabled requiring signed commits on `main`
- [ ] GitHub Actions automatically signs all release artifacts
- [ ] Cosign configured for Docker image signing
- [ ] Release includes: `.asc` signatures, signed `SHA256SUMS`, Cosign signatures
- [ ] Documentation updated with verification instructions
- [ ] First beta release (v1.0.0-beta.1 or later) is fully signed

### Verification

**Current State** (Development):
```bash
# Check if commit is signed (optional)
git log --show-signature -1
# May show: "No signature" - acceptable during DEV-004

# Check for release signatures
ls publish/*.asc 2>/dev/null || echo "No signatures yet - acceptable during DEV-004"
```

**Target State** (after remediation):
```bash
# All commits must be signed
git log --show-signature -1
# Must show: "Good signature from..."

# All releases must have signatures
ls publish/*.asc
# Must list: sysdocs-linux-x64.tar.gz.asc, SHA256SUMS.gpg, etc.

# Verify signatures
gpg --verify publish/sysdocs-linux-x64.tar.gz.asc
# Must show: "Good signature from SysDocs Bot <bot@sysdocs.dev>"
```

### Review Schedule

- **Weekly**: Check progress on team GPG key setup
- **Before Alpha Release**: Verify CI/CD signing infrastructure operational
- **Before Beta Release**: **Critical** - Must be fully resolved with enforcement enabled
- **Monthly**: Review key expiration dates and rotation needs

---

## Future Deviations

Additional deviations will be documented here using the same format:
- **DEV-XXX**: Description
- Date, Status, Severity, Requirements affected
- Reason, Impact, Remediation plan

