# Deterministic Builds with Nix

**📖 Navigation**: [⬅️ Back: Determinism Explained](DETERMINISM_EXPLAINED.md) | [🏠 README](../README.md) | [➡️ Next: Build Summary](DETERMINISTIC_BUILD_SUMMARY.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This is a **HUMAN-MAINTAINED GUIDE** for the Nix build system.  
> Build configuration is in `flake.nix` (version controlled, not auto-generated).

## Overview

SysDocs uses **Nix** as the **mandatory** build system to ensure 100% deterministic, reproducible builds across all platforms. This document explains why Nix is required, how it works on both Windows and Linux, and how to verify build reproducibility.

## Why Nix is Mandatory

### The Determinism Problem

Traditional build systems (including Docker with Dockerfiles) have several non-deterministic aspects:
- Package manager updates can pull different versions
- Build timestamps vary
- File ordering can differ between platforms
- Environment variables affect output
- Network dependencies can change

### The Nix Solution

Nix solves this by:
1. **Content-Addressable Storage** - Every package has a hash based on ALL inputs
2. **Hermetic Builds** - No access to system state, network, or time
3. **Reproducible Environments** - Identical dependencies on all platforms
4. **Binary Cache** - If hash exists, reuse the exact same binary
5. **Cryptographic Verification** - Outputs are verifiable across machines

**Result:** The same source code + Nix flake = identical binary output, whether built on Windows, Linux, or macOS.

---

## Platform Support

### Linux (Native)

Nix runs natively on Linux and is the primary development platform.

```bash
# Install Nix with flakes enabled
sh <(curl -L https://nixos.org/nix/install) --daemon

# Enable flakes (if not already enabled)
mkdir -p ~/.config/nix
echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf

# Build SysDocs
nix build

# Build Docker image
nix build .#docker
```

### Windows (WSL2 Required)

Windows cannot run Nix natively, but **Windows Subsystem for Linux 2 (WSL2)** provides a complete Linux environment.

#### Setup WSL2 on Windows

1. **Enable WSL2:**
   ```powershell
   # Run as Administrator in PowerShell
   wsl --install
   # Restart your computer
   ```

2. **Install Ubuntu (or your preferred distro):**
   ```powershell
   wsl --install -d Ubuntu
   ```

3. **Enter WSL2:**
   ```powershell
   wsl
   ```

4. **Install Nix inside WSL2:**
   ```bash
   sh <(curl -L https://nixos.org/nix/install) --daemon
   
   # Enable flakes
   mkdir -p ~/.config/nix
   echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf
   
   # Reload shell
   exec bash
   ```

5. **Clone and build:**
   ```bash
   cd /mnt/c/Users/YourUsername/source/SysDocs  # Access Windows files
   nix build
   nix build .#docker
   ```

#### Why WSL2 is Required

- **Nix requires Linux kernel features** - Cannot run on Windows directly
- **Docker images are Linux-based** - Must be built on Linux
- **WSL2 provides native Linux** - Near-native performance, full compatibility
- **Seamless file access** - Can access Windows files via `/mnt/c/`

#### Alternative: Remote Linux Build Server

If WSL2 is not an option, you can set up a remote Linux build machine:

1. Set up a Linux VM or cloud instance
2. Install Nix on the remote machine
3. Configure SSH access
4. Use Nix's distributed build feature:

```bash
# On your Windows machine
nix build --builders "ssh://user@linux-server x86_64-linux"
```

See: https://nixos.org/manual/nix/stable/advanced-topics/distributed-builds.html

### macOS

Nix runs natively on macOS (Intel and Apple Silicon).

```bash
# Install Nix
sh <(curl -L https://nixos.org/nix/install)

# Enable flakes
mkdir -p ~/.config/nix
echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf

# Build (will cross-compile to Linux for Docker)
nix build
nix build .#docker
```

---

## Build Commands

### Standard Build

```bash
# Build the SysDocs application
nix build

# Result is symlinked to ./result
./result/bin/sysdocs --help
```

### Docker Image Build

```bash
# Build the deterministic Docker image
nix build .#docker

# Load into Docker
docker load < result

# Run the container
docker run sysdocs:0.1.0-alpha --help
```

### Development Shell

```bash
# Enter development environment with all dependencies
nix develop

# Now you have dotnet, docker, and all tools available
dotnet build src/SysDocs.sln
dotnet test
```

---

## Verifying Determinism

### Same Machine, Different Times

```bash
# Build 1
nix build .#docker
sha256sum result > build1.sha256

# Clean and rebuild
rm result
nix build .#docker
sha256sum result > build2.sha256

# Compare
diff build1.sha256 build2.sha256
# Should be identical!
```

### Different Machines, Same Result

**Machine A (e.g., Windows WSL2):**
```bash
nix build .#docker
sha256sum result
# Output: abc123...def456
```

**Machine B (e.g., Linux server):**
```bash
nix build .#docker
sha256sum result
# Output: abc123...def456 (IDENTICAL)
```

### Verifying Against CI/CD

```bash
# Get the hash from CI artifacts
curl https://ci.example.com/sysdocs/latest/docker.sha256

# Compare with your local build
nix build .#docker
sha256sum result

# Hashes must match for true reproducibility
```

---

## How It Works

### Nix Flake Structure

The `flake.nix` file defines:

1. **Inputs** - Pinned dependencies (nixpkgs version)
2. **Build Process** - Deterministic .NET build
3. **Docker Image** - Using `dockerTools.buildLayeredImage`
4. **Development Environment** - Consistent dev shell

### Key Determinism Features

#### 1. Pinned Dependencies

```nix
inputs = {
  nixpkgs.url = "github:NixOS/nixpkgs/nixos-24.05";  # Exact commit
  flake-utils.url = "github:numtide/flake-utils";
};
```

#### 2. Fixed Timestamps

```nix
SOURCE_DATE_EPOCH = "315532800";  # 1980-01-01 00:00:00 UTC
created = "1980-01-01T00:00:00Z";
```

#### 3. Deterministic .NET Build

```nix
dotnet build \
  /p:Deterministic=true \
  /p:ContinuousIntegrationBuild=true \
  /p:SourceRevisionId=${gitHash}
```

#### 4. Layered Docker Images

```nix
dockerTools.buildLayeredImage {
  name = "sysdocs";
  tag = "0.1.0-alpha";
  maxLayers = 100;  # Optimize layer reuse
}
```

### Content-Addressable Storage

Every build output has a hash like:
```
/nix/store/abc123...xyz-sysdocs-0.1.0-alpha
```

This hash is computed from:
- Source code
- All dependencies (transitively)
- Build scripts
- Environment variables
- Everything that could affect the output

**Same inputs = Same hash = Same output**

---

## Advantages Over Traditional Docker

### Traditional Dockerfile Approach

```dockerfile
# ❌ Non-deterministic
FROM mcr.microsoft.com/dotnet/sdk:10.0  # Tag can change!
RUN apt-get update && apt-get install -y fonts  # Package versions vary!
COPY . .
RUN dotnet publish  # Build time affects output!
```

**Problems:**
- `sdk:10.0` tag can point to different images over time
- `apt-get update` gets latest packages (not reproducible)
- Build timestamps embedded in output
- No cryptographic verification

### Nix Approach

```nix
# ✅ Deterministic
dockerTools.buildLayeredImage {
  name = "sysdocs";
  tag = "0.1.0-alpha";
  contents = [ sysdocs fontconfig freetype ];  # Exact packages by hash
  created = "1980-01-01T00:00:00Z";  # Fixed timestamp
}
```

**Benefits:**
- Every dependency pinned by cryptographic hash
- No timestamps in output
- Reproducible across all platforms
- Can verify against any other build

---

## Updating Dependencies

### Updating nixpkgs

```bash
# Update to latest nixos-24.05 commit
nix flake update

# Or pin to specific commit
nix flake lock --override-input nixpkgs github:NixOS/nixpkgs/abc123def456
```

### Updating .NET Packages

```bash
# Update NuGet packages
dotnet restore src/SysDocs.sln

# Lock the versions
dotnet restore --locked-mode

# Commit packages.lock.json
git add src/*/packages.lock.json
git commit -m "Update .NET dependencies"
```

### Verification After Update

```bash
# Rebuild
nix build .#docker

# Verify new hash
sha256sum result

# Document in DEVIATIONS.md if major changes
```

---

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Deterministic Build

on: [push, pull_request]

jobs:
  build:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Install Nix
        uses: cachix/install-nix-action@v24
        with:
          extra_nix_config: |
            experimental-features = nix-command flakes
      
      - name: Build Docker Image
        run: nix build .#docker
      
      - name: Compute and Store Hash
        run: |
          sha256sum result > docker-image.sha256
          cat docker-image.sha256
      
      - name: Upload Artifact
        uses: actions/upload-artifact@v4
        with:
          name: sysdocs-docker
          path: |
            result
            docker-image.sha256
      
      - name: Load and Test Image
        run: |
          docker load < result
          docker run sysdocs:0.1.0-alpha --version
```

---

## Troubleshooting

### "error: experimental features not enabled"

```bash
mkdir -p ~/.config/nix
echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf
```

### "error: building on Windows is not supported"

- Install WSL2 and build from inside Linux environment
- Or use remote builder: `nix build --builders "ssh://linux-server"`

### "error: hash mismatch"

This means something changed that shouldn't have:
1. Check git status - uncommitted changes?
2. Verify flake.lock is committed
3. Ensure no local modifications to dependencies

### Build is Slow

```bash
# Use binary cache (automatic with nixpkgs)
nix build --option substitute true

# For private cache
nix build --option substituters "https://your-cache.com"
```

---

## Best Practices

### 1. Always Use Nix for Official Builds

```bash
# ✅ Deterministic
nix build .#docker

# ❌ Not deterministic
docker build -t sysdocs .
```

### 2. Commit flake.lock

```bash
# After any nix flake update
git add flake.lock
git commit -m "Update Nix flake dependencies"
```

### 3. Document Deviations

If you need to make changes that affect determinism:
1. Document in `DEVIATIONS.md`
2. Explain why the change was necessary
3. Describe the impact on reproducibility

### 4. Verify Regularly

```bash
# In CI, compare hashes between builds
# Alert if hash changes without code changes
```

### 5. Use Development Shell

```bash
# Always enter Nix shell for development
nix develop

# This ensures everyone uses the same tool versions
```

---

## FAQ

### Why not just use Docker?

Docker (with Dockerfiles) is **not deterministic**:
- Base images can change under the same tag
- `apt-get update` gets different package versions
- Build timestamps vary
- File system ordering differs by platform

Nix provides true reproducibility with cryptographic guarantees.

### Can I develop without Nix?

For quick iteration, yes:
```bash
dotnet build src/SysDocs.sln
dotnet test
```

But **official builds MUST use Nix** to ensure determinism.

### What if I'm on Windows?

Use WSL2. It's required because:
1. Nix needs Linux
2. Docker images are Linux-based
3. WSL2 provides near-native Linux performance

### How do I know my build is truly deterministic?

```bash
# Build twice
nix build .#docker && sha256sum result > hash1.txt
nix build .#docker && sha256sum result > hash2.txt
diff hash1.txt hash2.txt

# Build on different machine
# Compare hashes - they must be identical
```

---

## References

- [Nix Manual](https://nixos.org/manual/nix/stable/)
- [Nix Pills - Understanding Nix](https://nixos.org/guides/nix-pills/)
- [Building Docker Images with Nix](https://nix.dev/tutorials/nixos/building-and-running-docker-images)
- [Deterministic Builds](https://reproducible-builds.org/)
- [WSL2 Installation](https://docs.microsoft.com/en-us/windows/wsl/install)

---

**Remember:** Nix is not optional. It's the foundation of SysDocs' determinism guarantee.

