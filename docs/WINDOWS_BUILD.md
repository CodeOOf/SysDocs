# Windows Build Instructions for SysDocs

**📖 Navigation**: [⬅️ Back: Build Summary](DETERMINISTIC_BUILD_SUMMARY.md) | [🏠 README](../README.md) | [➡️ Next: License Compliance](LICENSE_COMPLIANCE_SUMMARY.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This is a **HUMAN-MAINTAINED GUIDE** for Windows developers.  
> For detailed Nix information, see [DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md)

## Overview

SysDocs requires **Nix** for deterministic builds. Since Nix cannot run natively on Windows, you must use **Windows Subsystem for Linux 2 (WSL2)** to build the project.

## Quick Start

### 1. Install WSL2

Open PowerShell as Administrator and run:

```powershell
wsl --install
```

Restart your computer when prompted.

### 2. Open WSL2

```powershell
wsl
```

You're now in a Linux environment!

### 3. Install Nix

```bash
sh <(curl -L https://nixos.org/nix/install) --daemon

# Enable flakes
mkdir -p ~/.config/nix
echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf

# Reload shell
exec bash
```

### 4. Access Your Windows Files

Your Windows drives are mounted at `/mnt/`:

```bash
cd /mnt/c/Users/YourUsername/source/SysDocs
```

### 5. Build with Nix

```bash
# Build the application
nix build

# Build Docker image
nix build .#docker

# Load Docker image (accessible from Windows Docker Desktop!)
docker load < result
```

## Development Workflow

### Option A: VS Code with WSL Extension (Recommended)

1. Install "Remote - WSL" extension in VS Code (on Windows)
2. Open VS Code in Windows
3. Press `F1` → "WSL: Connect to WSL"
4. Open folder: `/mnt/c/Users/YourUsername/source/SysDocs`
5. VS Code now runs in Linux with full Nix support!

### Option B: Terminal Only

```bash
# From Windows PowerShell
wsl

# Now in Linux
cd /mnt/c/Users/YourUsername/source/SysDocs
nix develop

# All tools available:
dotnet build src/SysDocs.sln
dotnet test
nix build .#docker
```

## File Access

- **Windows → Linux**: Access via `/mnt/c/`, `/mnt/d/`, etc.
- **Linux → Windows**: Files are synchronized automatically
- **Performance**: Store repo on Windows drive (`/mnt/c/`) for better IDE integration
- **Git**: Can use Git from either Windows or Linux, changes sync automatically

## Docker Integration

Docker Desktop for Windows automatically works with WSL2:

1. Install Docker Desktop for Windows
2. In Settings → Resources → WSL Integration: Enable for your distro
3. In WSL2: `docker` commands work automatically
4. Images built in WSL2 appear in Windows Docker Desktop

## Verifying Your Setup

```bash
# From WSL2
nix --version          # Should show Nix version
dotnet --version       # After 'nix develop'
docker --version       # Should connect to Windows Docker

# Build and verify
nix build .#docker
sha256sum result       # Shows deterministic hash
docker load < result   # Loads into Windows Docker Desktop
```

## Common Issues

### "wsl not found"

- Make sure you're on Windows 10 (build 19041+) or Windows 11
- Run PowerShell as Administrator

### "Nix: command not found"

```bash
# Make sure installation completed
cat ~/.nix-profile/etc/profile.d/nix.sh

# Source it manually if needed
. ~/.nix-profile/etc/profile.d/nix.sh
```

### "experimental features not enabled"

```bash
mkdir -p ~/.config/nix
echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf
exec bash
```

### Slow File System Performance

Store the repository on the Linux file system for better performance:

```bash
# Copy to Linux home directory
cp -r /mnt/c/Users/YourUsername/source/SysDocs ~/SysDocs
cd ~/SysDocs

# Or clone directly in Linux
cd ~
git clone https://github.com/CodeOOf/SysDocs.git
```

## FAQ

### Can I build without WSL2?

No. Nix requires Linux, and Docker images must be built on Linux for Linux. WSL2 is the official Microsoft-supported way to run Linux on Windows.

### What about using Docker for Windows?

Traditional Dockerfiles are not deterministic. Nix provides cryptographic guarantees of reproducibility.

### Can I use PowerShell?

For quick development, yes (`dotnet build`, `dotnet test`). But official builds must use Nix from WSL2.

### Do I need to know Linux?

Basic commands help, but VS Code with WSL extension makes it transparent. Most operations work exactly as on Windows.

## Next Steps

1. ✅ Install WSL2
2. ✅ Install Nix in WSL2
3. ✅ Clone/access repository
4. ✅ Run `nix develop`
5. 📖 Read [docs/DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md)
6. 🚀 Start developing!

---

**Need Help?**
- WSL2 Docs: https://docs.microsoft.com/en-us/windows/wsl/
- Nix Manual: https://nixos.org/manual/nix/stable/
- SysDocs Issues: https://github.com/CodeOOf/SysDocs/issues

