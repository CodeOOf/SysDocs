# Platform Architecture & Deployment Model

**📖 Navigation**: [⬅️ Back: README](../README.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> **Architecture Overview**: SysDocs deployment and development platform strategy

## Executive Summary

**Production Deployment**: Linux Docker image only  
**Development**: Linux/macOS native OR Windows via WSL2  
**Cross-Platform Execution**: Via Docker (no platform-specific binaries)

---

## 1. Production Deployment Architecture

### Primary Deployment Method

```
┌─────────────────────────────────────────────────────┐
│          PRODUCTION DEPLOYMENT                      │
│                                                     │
│    Linux Docker Image (ghcr.io/codeof/sysdocs)    │
│    Built on: Linux CI/CD (GitHub Actions)         │
│    Runs on:  Any platform via Docker               │
│    Signed with: Cosign (container signing)         │
└─────────────────────────────────────────────────────┘
                           │
              ┌────────────┼────────────┐
              │            │            │
         Windows       Linux        macOS
           via          native        via
         Docker        Docker       Docker
```

### Why Docker-Only?

1. **Consistent Runtime**: Same Linux environment on all platforms
2. **Simplified CI/CD**: Single build target
3. **Reduced Complexity**: No platform-specific code paths
4. **Easier Testing**: One artifact to verify
5. **Better Security**: Cosign signing for containers
6. **Standard Practice**: Industry standard for tool distribution

### What This Means

- ✅ **Users**: Run Docker image on any platform
- ✅ **Releases**: Only Linux Docker image + Linux tarball
- ✅ **No Windows `.exe`**: Docker handles Windows execution
- ✅ **No macOS binary**: Docker handles macOS execution
- ✅ **No platform detection**: Docker provides consistent Linux

---

## 2. Development Platform Support

### Development Options

| Platform | Development | Build Verification | Production Build |
|----------|-------------|-------------------|------------------|
| **Linux** | ✅ Native .NET SDK | ✅ Nix (deterministic) | ✅ CI/CD builds here |
| **macOS** | ✅ Native .NET SDK | ✅ Nix (deterministic) | ❌ Dev only |
| **Windows** | ⚠️ WSL2 required for Linux tools | ⚠️ Nix via WSL2 | ❌ Dev only |

### Linux Development (Recommended)

```bash
# Install .NET SDK
sudo apt-get install dotnet-sdk-10

# Install Nix for deterministic builds
sh <(curl -L https://nixos.org/nix/install) --daemon

# Clone and build
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs
dotnet build  # Fast development build
nix build     # Deterministic build (slow but reproducible)
```

### macOS Development

```bash
# Install .NET SDK
brew install dotnet-sdk

# Install Nix for deterministic builds
sh <(curl -L https://nixos.org/nix/install) --daemon

# Clone and build
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs
dotnet build  # Fast development build
nix build     # Deterministic build (slow but reproducible)
```

### Windows Development (WSL2 Required)

```powershell
# Install WSL2
wsl --install

# Inside WSL2 Ubuntu terminal:
sudo apt-get update
sudo apt-get install dotnet-sdk-10

# Install Nix
sh <(curl -L https://nixos.org/nix/install) --daemon

# Clone and build
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs
dotnet build  # Fast development build
nix build     # Deterministic build
```

**Why WSL2 for Windows?**
- Nix requires Unix-like environment
- Linux tooling (bash, make, tar, etc.)
- GPG signing tools
- Consistent with production Linux environment
- Docker Desktop integrates with WSL2

---

## 3. CI/CD Pipeline Architecture

### GitHub Actions Workflow

```
┌─────────────────────────────────────────────┐
│  Release Pipeline (Linux CI/CD)             │
└─────────────────────────────────────────────┘
                    │
    ┌───────────────┼───────────────┐
    │               │               │
    ▼               ▼               ▼
┌─────────┐   ┌─────────┐   ┌─────────────┐
│ Build   │   │ Build   │   │ Build &     │
│ Linux   │   │ Docker  │   │ Sign Docker │
│ Tarball │   │ Image   │   │ (Cosign)    │
└─────────┘   └─────────┘   └─────────────┘
    │               │               │
    └───────────────┼───────────────┘
                    │
            ┌───────▼───────┐
            │ GitHub        │
            │ Release       │
            │               │
            │ Artifacts:    │
            │ • Docker img  │
            │ • Linux .tgz  │
            │ • Signatures  │
            └───────────────┘
```

### Build Jobs

1. **`build-linux`**: Build Linux tarball + GPG signatures
2. **`build-docker`**: Build Docker image + Cosign signatures
3. **`create-release`**: Publish to GitHub Releases
4. **`build-nuget`** (optional): NuGet packages for library consumers

**No Windows/macOS build jobs** - Docker handles execution on those platforms.

---

## 4. Release Artifacts

### What's in a Release

```
v1.0.0/
├── sysdocs-linux-x64.tar.gz          # Linux native binary
├── sysdocs-linux-x64.tar.gz.asc      # GPG signature
├── SHA256SUMS.asc                    # Signed checksums
└── Docker Image:
    ghcr.io/codeof/sysdocs:1.0.0      # Cosign-signed Docker image
    ghcr.io/codeof/sysdocs:latest
```

### What's NOT in a Release

- ❌ No `sysdocs-windows-x64.zip`
- ❌ No `sysdocs-macos-x64.tar.gz`
- ❌ No platform-specific executables
- ❌ No Authenticode signatures (Windows-only)

### Why?

**Use Docker instead:**
```bash
# Windows
docker pull ghcr.io/codeof/sysdocs:1.0.0
docker run ghcr.io/codeof/sysdocs:1.0.0 --help

# Linux
docker pull ghcr.io/codeof/sysdocs:1.0.0
docker run ghcr.io/codeof/sysdocs:1.0.0 --help

# macOS
docker pull ghcr.io/codeof/sysdocs:1.0.0
docker run ghcr.io/codeof/sysdocs:1.0.0 --help
```

Same image, same behavior, all platforms.

---

## 5. Code Signing Strategy

### GPG Signing (Linux Tarball)

- **What**: Linux native binary tarball
- **How**: GPG detached signatures (`.asc` files)
- **Verify**: `gpg --verify sysdocs-linux-x64.tar.gz.asc`
- **Platform**: Cross-platform (GPG available everywhere)

### Cosign Signing (Docker Images)

- **What**: Docker container images
- **How**: Sigstore Cosign signatures
- **Verify**: `cosign verify --key cosign.pub ghcr.io/codeof/sysdocs:1.0.0`
- **Platform**: Cross-platform (Cosign available everywhere)

### No Authenticode Signing

- **Reason**: Windows-specific, requires paid certificates
- **Alternative**: Docker + Cosign (open-source, cross-platform)
- **Impact**: None - Docker handles Windows execution

---

## 6. Determinism Strategy & Requirements Verification

### Build Determinism (BR-02)

```
Requirement: BR-02 (Build Determinism)
Scope: Linux distributions (Fedora, Debian)
Verification: Nix builds on CI/CD
Result: Identical Docker image hash

Fedora build:  sha256:abc123...
Debian build:  sha256:abc123...  ✅ Match
```

### Output Determinism (FR-06, FR-07)

```
Requirement: FR-06, FR-07 (Output Determinism)
Verification: CI/CD tests on Windows, Linux, macOS
Result: Identical PDF hash during development/testing

Windows (.NET): sha256:xyz789...
Linux (.NET):   sha256:xyz789...  ✅ Match
macOS (.NET):   sha256:xyz789...  ✅ Match
```

### Cross-Platform Execution & Reproducibility (FR-14, NFR-01)

```
FR-14: Identical behavior across platforms
NFR-01: 100% reproducible - byte-for-byte identical PDFs

Implementation: Docker deployment
Verification: Release pipeline tests Docker on all platforms
Result: Identical PDF hash from Docker on all host OS

Docker on Ubuntu:  sha256:def456...
Docker on Windows: sha256:def456...  ✅ Match
Docker on macOS:   sha256:def456...  ✅ Match
```

**Verification Process** (Release Pipeline):

1. **Job 4** (`verify-docker-cross-platform`):
   - Runs Docker image on ubuntu-latest, windows-latest, macos-latest
   - Same test input → generates PDF on each platform
   - Calculates SHA256 hash of output PDF
   - Uploads hash for comparison
   
2. **Job 5** (`verify-fr14-nfr01-compliance`):
   - Downloads all three hashes
   - Compares Ubuntu vs Windows vs macOS
   - ✅ **Pass**: All hashes identical → **FR-14 & NFR-01 verified**
   - ❌ **Fail**: Any hash differs → **Release blocked**

**What This Proves**:
- **FR-14**: Docker executes identically on Windows, Linux, macOS hosts
- **NFR-01**: Same input produces byte-for-byte identical output (100% reproducible)
- **FR-06**: Output is deterministic
- **FR-07**: Identical text, images, ordering, metadata, layout

**Key Point**: This is **automated proof** in every release, not just a claim. If Docker behavior diverges on any platform, the release pipeline fails.

---

## 7. Development Workflow

### Typical Developer Flow

```bash
# 1. Clone repository
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs

# 2. Development build (fast)
dotnet build
dotnet run --project src/SysDocs.Cli -- --help

# 3. Run tests
dotnet test

# 4. Test with Docker (production-like)
docker build -t sysdocs:local .
docker run sysdocs:local --help

# 5. Deterministic build (for verification)
nix build
./result/bin/sysdocs --help
```

### When to Use What

| Task | Tool | Why |
|------|------|-----|
| Daily development | `dotnet build` | Fast incremental builds |
| Unit testing | `dotnet test` | Quick feedback loop |
| Integration testing | `docker build` | Test production environment |
| Verify determinism | `nix build` | Cryptographic reproducibility |
| Final release | CI/CD | Official build artifacts |

---

## 8. Platform-Specific Notes

### Linux Developers

✅ **Fully Supported**
- Native .NET development
- Nix for deterministic builds
- All tools available natively
- Can verify production builds locally

### macOS Developers

✅ **Fully Supported**
- Native .NET development
- Nix for deterministic builds
- All tools available via Homebrew
- Can verify production builds locally

### Windows Developers

⚠️ **WSL2 Required for Serious Development**

**What works in native Windows:**
- .NET development (`dotnet build`, `dotnet test`)
- Running via Docker Desktop
- Git operations

**What requires WSL2:**
- Nix builds (deterministic verification)
- GPG signing setup
- Bash scripts (`make`, `sign-release.sh`)
- Linux tooling (tar, gzip, sha256sum)

**Recommended Setup:**
1. Install WSL2 with Ubuntu
2. Install .NET SDK in WSL2
3. Install Nix in WSL2
4. Clone repository in WSL2 filesystem
5. Use VS Code with Remote-WSL extension

---

## 9. User Installation

### Recommended: Docker

```bash
# Pull image
docker pull ghcr.io/codeof/sysdocs:1.0.0

# Run
docker run -v $(pwd):/workspace ghcr.io/codeof/sysdocs:1.0.0 \
  --input /workspace/input.md \
  --output /workspace/output.pdf

# Or create alias
alias sysdocs='docker run -v $(pwd):/workspace ghcr.io/codeof/sysdocs:1.0.0'
sysdocs --help
```

### Alternative: Linux Native Binary

```bash
# Download and extract
wget https://github.com/CodeOOf/SysDocs/releases/download/v1.0.0/sysdocs-linux-x64.tar.gz
tar -xzf sysdocs-linux-x64.tar.gz

# Verify signature
gpg --verify sysdocs-linux-x64.tar.gz.asc

# Run
./sysdocs --help
```

**Windows/macOS**: Use Docker method (native binaries not provided).

---

## 10. FAQ

### Q: Why no Windows `.exe`?

**A**: Docker provides consistent Linux runtime on Windows. No need for platform-specific binaries.

### Q: Why no macOS binary?

**A**: Same reason - Docker handles macOS execution. Simpler and more maintainable.

### Q: Can I develop on Windows without WSL2?

**A**: For basic .NET development, yes. For serious development (Nix builds, signing), WSL2 required.

### Q: Why Docker-only for production?

**A**: Consistency, simplicity, industry standard. One build, runs everywhere.

### Q: How do I verify signatures on Windows?

**A**: Install GPG via Git Bash or Cosign for Docker images. Both are cross-platform.

### Q: What about air-gapped environments?

**A**: Docker images can be saved/loaded offline:
```bash
docker save ghcr.io/codeof/sysdocs:1.0.0 > sysdocs.tar
# Transfer sysdocs.tar to air-gapped system
docker load < sysdocs.tar
```

---

## 11. Summary

| Aspect | Decision | Reason |
|--------|----------|--------|
| **Production** | Linux Docker image | Cross-platform via Docker |
| **Development** | Linux/macOS native, Windows WSL2 | Flexibility for developers |
| **CI/CD** | Linux only | Single build target |
| **Releases** | Docker + Linux tarball | No platform-specific binaries |
| **Signing** | GPG + Cosign | Open-source, cross-platform |
| **Determinism** | Linux-to-Linux verified | Docker ensures consistency |

**Bottom Line**: Build once on Linux, deploy everywhere via Docker. Simple, consistent, maintainable.

---

**Next Steps**: See [DEVELOPER_SETUP_SIGNING.md](DEVELOPER_SETUP_SIGNING.md) for development environment setup.
