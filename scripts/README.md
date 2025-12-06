# Scripts Directory

This directory contains platform-specific build and utility scripts for SysDocs.

## 📋 Available Scripts

### Build Scripts

#### `build.ps1` (Windows/PowerShell)
PowerShell build script for Windows environments.

```powershell
# Usage
.\scripts\build.ps1 [command] [-Configuration Release|Debug]

# Examples
.\scripts\build.ps1 build
.\scripts\build.ps1 test
.\scripts\build.ps1 publish -Configuration Release
```

#### `build.sh` (Linux/macOS/Unix)
Bash build script for Unix-like environments.

```bash
# Usage
./scripts/build.sh [command]

# Examples
./scripts/build.sh build
./scripts/build.sh test
export CONFIGURATION=Debug && ./scripts/build.sh publish
```

**Available commands:**
- `restore` - Restore NuGet packages
- `build` - Build the solution
- `test` - Run tests
- `publish` - Publish the application
- `docker` - Build Docker image
- `clean` - Clean build artifacts
- `all` - Full build pipeline (restore, build, test, publish)

### Verification Scripts

#### `verify-determinism.ps1` / `verify-determinism.sh`
Verifies that builds are deterministic by building twice and comparing output hashes.

```powershell
# Windows
.\scripts\verify-determinism.ps1

# Linux/macOS
./scripts/verify-determinism.sh
```

## 🎯 Recommended Usage

**Use Makefile instead of direct script execution:**

The Makefile at the root provides a cross-platform interface that automatically selects the correct script based on your OS:

```bash
make build      # Builds using the appropriate script
make test       # Runs tests
make publish    # Publishes the application
make all        # Full build pipeline
```

See `make help` for all available targets.

## 🔧 When to Use Scripts Directly

Use these scripts directly when:
- You need platform-specific behavior
- You're on a system without `make` installed
- You're debugging build issues
- You need to pass specific parameters

Otherwise, prefer using the Makefile for cross-platform consistency.

## 📝 Adding New Scripts

When adding platform-specific scripts:

1. Create both `.ps1` (PowerShell) and `.sh` (Bash) versions
2. Keep the interface consistent between platforms
3. Document the script in this README
4. Add a corresponding target to the Makefile if appropriate
5. Make `.sh` scripts executable: `chmod +x scripts/yourscript.sh`

## 🔗 Related Documentation

- [../README.md](../README.md) - Main project documentation
- [../project/CONTRIBUTING.md](../project/CONTRIBUTING.md) - Development guidelines
- [../docs/DETERMINISTIC_BUILDS.md](../docs/DETERMINISTIC_BUILDS.md) - Build system details
