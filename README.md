# SysDocs

> **Deterministic Systems Engineering Documentation Pipeline**  
> Build reproducible, traceable SE artifacts aligned with INCOSE SE Handbook and the V-Model

[![.NET Version](https://img.shields.io/badge/.NET-10%20(LTS)-512BD4)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![Nix](https://img.shields.io/badge/Nix-Enabled-5277C3?logo=nixos)](https://nixos.org/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## 🎯 Overview

SysDocs is an open-source, Linux-native documentation automation tool designed for Systems Engineering projects. It transforms various input formats (Markdown, Word, LaTeX, PDF) into professional, deterministic PDF outputs with full traceability and tool qualification support.

### Deployment Model

- **Production**: Linux Docker image only (cross-platform execution via Docker)
- **Development**: Linux native, macOS native, or Windows via WSL2
- **No platform-specific binaries** - Docker handles all platform differences

### Key Features

- 🔄 **Deterministic Builds** - Identical outputs across all platforms and environments (see [docs/DETERMINISM_EXPLAINED.md](docs/DETERMINISM_EXPLAINED.md))
- 🐳 **Docker-First** - Primary deployment via Linux Docker image
- 🔒 **Air-Gap Compatible** - No external network dependencies
- 📊 **SE Compliance** - INCOSE SE Handbook & V-Model aligned
- 🔍 **Full Traceability** - Change tracking and requirements mapping
- 🛠️ **Tool Qualification Ready** - Audit logs, validation, and verification support

---

## 🚀 Quick Start

**🎯 You are here: README.md (Start)**

### 🐳 Using Docker (Recommended for All Platforms)

```bash
# Pull the latest version
docker pull ghcr.io/codeof/sysdocs:latest

# Run SysDocs
docker run -v $(pwd):/workspace ghcr.io/codeof/sysdocs:latest \
  --input /workspace/input.md \
  --output /workspace/output.pdf

# Or create an alias for easier use
alias sysdocs='docker run -v $(pwd):/workspace ghcr.io/codeof/sysdocs:latest'
sysdocs --help
```

**Works on**: Windows, Linux, macOS - Docker handles everything!

### 🔧 For Developers

**Platform-Specific Setup**:
- **Linux**: See [CONTRIBUTING.md](project/CONTRIBUTING.md) for .NET SDK + Nix setup
- **macOS**: See [CONTRIBUTING.md](project/CONTRIBUTING.md) for .NET SDK + Nix setup  
- **Windows**: See [WINDOWS_BUILD.md](docs/WINDOWS_BUILD.md) for WSL2 setup

**Architecture Overview**: See [PLATFORM_ARCHITECTURE.md](docs/PLATFORM_ARCHITECTURE.md) for complete platform strategy

### 📖 Choose Your Reading Path

#### 🚀 Quick Start Path
1. **README.md** (You are here) - Project overview and quick start
2. → [project/CONTRIBUTING.md](project/CONTRIBUTING.md) - Development setup and workflow
3. → [project/BRANCH_STRATEGY.md](project/BRANCH_STRATEGY.md) - Git workflow and PR process

#### 📋 Requirements & V&V Path
1. **README.md** (You are here)
2. → [project/REQUIREMENTS_MATRIX.md](project/REQUIREMENTS_MATRIX.md) ⭐ **START HERE for V&V**
3. → [project/REQUIREMENTS.md](project/REQUIREMENTS.md) - Detailed requirements specification
4. → [project/TRACEABILITY.md](project/TRACEABILITY.md) - Implementation mapping
5. → [project/VERIFICATION_VALIDATION.md](project/VERIFICATION_VALIDATION.md) - V&V strategy
6. → [project/DEVIATIONS.md](project/DEVIATIONS.md) - Approved deviations
7. → [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) - Test coverage report
8. → [reports/LICENSE_COMPLIANCE.md](reports/LICENSE_COMPLIANCE.md) - Dependency licenses

#### 🔧 Technical Deep Dive Path
1. **README.md** (You are here)
2. → [docs/DETERMINISM_EXPLAINED.md](docs/DETERMINISM_EXPLAINED.md) - Build vs output determinism
3. → [docs/DETERMINISTIC_BUILDS.md](docs/DETERMINISTIC_BUILDS.md) - Nix build setup
4. → [docs/DETERMINISTIC_BUILD_SUMMARY.md](docs/DETERMINISTIC_BUILD_SUMMARY.md) - Build system overview
5. → [docs/WINDOWS_BUILD.md](docs/WINDOWS_BUILD.md) - Windows/WSL2 setup (if needed)

#### 🗺️ Navigation Help
- **Lost?** → [docs/DOCUMENTATION_NAVIGATION.md](docs/DOCUMENTATION_NAVIGATION.md) - Navigation guide
- **Need docs overview?** → [docs/README.md](docs/README.md) - Docs directory index
- **Want complete map?** → [project/DOCUMENTATION_MAP.md](project/DOCUMENTATION_MAP.md) - Full documentation tree & all reading paths

---

### Prerequisites

**For Production Use:**
- [Docker](https://www.docker.com/get-started) (any platform)

**For Development:**
- **Linux**: [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) + [Nix](https://nixos.org/download.html) (for deterministic builds)
- **macOS**: [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) + [Nix](https://nixos.org/download.html) (for deterministic builds)
- **Windows**: [WSL2](https://docs.microsoft.com/en-us/windows/wsl/install) required for Linux tooling - see [docs/WINDOWS_BUILD.md](docs/WINDOWS_BUILD.md)

**Note**: Official releases are Linux Docker images built from Linux CI/CD. Windows/macOS developers use WSL2/native .NET for development, but final builds happen on Linux.

### Building with Nix (Deterministic)

**This produces the official, reproducible Linux Docker image:**

```bash
# Linux/macOS: Install Nix directly
# Windows: Install WSL2 first, then Nix inside WSL2

# Build the application
nix build

# Build Docker image (100% deterministic!)
nix build .#docker

# Load into Docker
docker load < result

# Run
docker run sysdocs:0.1.0-alpha --help

# Verify determinism (hash will be identical across all Linux builds)
sha256sum result
```

**Why Nix is mandatory for official releases:**
- ✅ Cryptographically guaranteed reproducible builds
- ✅ Same hash on Fedora, Debian, Ubuntu, etc.
- ✅ No dependency drift over time
- ✅ Audit-ready traceability

See [docs/DETERMINISTIC_BUILDS.md](docs/DETERMINISTIC_BUILDS.md) for details.

### Quick Development (Non-Deterministic)

For rapid iteration during development only:

```bash
# Clone the repository
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs

# Using Make (recommended - cross-platform)
make build       # Build the solution
make test        # Run tests
make reports     # Generate traceability reports

# Or use dotnet directly
dotnet build src/SysDocs.sln
dotnet run --project src/SysDocs.Cli -- --input docs/input.md --output output.pdf

# Platform-specific scripts available in scripts/
# Windows: .\scripts\build.ps1 build
# Linux/macOS: ./scripts/build.sh build
```

**Available Make targets:**
```bash
make help        # Show all available targets
make build       # Build the solution
make test        # Run all tests
make publish     # Publish the CLI application
make docker      # Build Docker image
make clean       # Clean build artifacts
make reports     # Generate V&V reports
make all         # Full build pipeline
```

### Using Nix (Deterministic Builds)

```bash
# Build with Nix
nix build

# Run the result
./result/bin/sysdocs --input docs/input.md --output output.pdf
```

---

## 📁 Project Structure

```
SysDocs/
├── src/
│   ├── SysDocs.Cli/              # Command-line interface
│   ├── SysDocs.Core/             # Core processing engine
│   │   ├── Importers/            # Document & image importers
│   │   ├── Model/                # Internal document model
│   │   ├── Exporters/            # PDF generation
│   │   ├── Templates/            # Document templates
│   │   ├── Tracing/              # Change tracking
│   │   └── Qualification/        # Tool qualification support
│   └── SysDocs.Templates/        # SE artifact templates
├── tests/
│   └── SysDocs.Tests/            # Unit & integration tests
├── project/                       # Project scope & requirements
├── docs/                          # Technical documentation
├── reports/                       # Auto-generated reports
├── scripts/                       # Platform-specific build scripts
├── Makefile                       # Cross-platform build automation
├── Dockerfile                     # Container definition
├── flake.nix                      # Nix build configuration
├── REQUIREMENTS.md                # Detailed requirements spec
├── TRACEABILITY.md                # Requirements traceability matrix
└── CONTRIBUTING.md                # Developer setup guide
```

---

## 🔧 Development Setup

### Advanced Features

#### Manifest-Based Document Assembly

SysDocs supports **manifest-based document assembly** for composing formal SE documents from distributed markdown files across git repository structures. This enables modern documentation practices while maintaining SE compliance.

**Example**: Compose a SEMP from `README.md` + `project_description.md` + `docs/engineering_process.md`:

```json
{
  "documents": [{
    "outputName": "01_SEMP_Project.pdf",
    "sources": [
      {"file": "README.md", "sections": ["1.1", "1.2"]},
      {"file": "project_description.md", "sections": ["2.1"]},
      {"file": "docs/engineering_process.md", "sections": ["3.1"]}
    ]
  }]
}
```

**Benefits**:
- ✅ Documentation lives in repository (README, docs/)
- ✅ Formal SE documents assembled automatically
- ✅ Byte-for-byte deterministic output
- ✅ Full traceability to source sections

See [docs/MANIFEST_BASED_ASSEMBLY.md](docs/MANIFEST_BASED_ASSEMBLY.md) for complete documentation.

**Example Projects**:
- `examples/adns-project/` - Traditional single-file SE documents
- `examples/skynet-repo/` - Modern git-repo with manifest assembly

---

## 🔧 Development Setup

See [project/CONTRIBUTING.md](project/CONTRIBUTING.md) for detailed instructions on:

- Setting up your development environment
- Branch strategy and workflow ([project/BRANCH_STRATEGY.md](project/BRANCH_STRATEGY.md))
- Building and testing locally
- Pull request process and V&V requirements
- Contributing guidelines
- Architecture overview

---

## 📋 Requirements & Verification

### Quick Start: V&V Navigator

**⭐ [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md) - Start here!**  
Complete requirements verification matrix showing each requirement's implementation, testing, and deviation status in one place.

**📖 [docs/DOCUMENTATION_NAVIGATION.md](docs/DOCUMENTATION_NAVIGATION.md) - Lost? Navigation guide!**  
Visual guide showing how to find information and follow the reader's thread through requirements.

### Detailed Documentation

- **Requirements**: [REQUIREMENTS.md](../project/REQUIREMENTS.md) - Complete functional and non-functional requirements specification
- **Verification Matrix**: [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md) - Central V&V reference with test and deviation links
- **Implementation**: [TRACEABILITY.md](../project/TRACEABILITY.md) - Implementation mapping and architecture decisions
- **Test Coverage**: [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) - Auto-generated test-to-requirement mapping
- **V&V Strategy**: [VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md) - Overall verification and validation approach
- **Deviations**: [DEVIATIONS.md](../project/DEVIATIONS.md) - Approved technical deviations with justification

### Checking a Specific Requirement

For any requirement (e.g., FR-01):

1. Look it up in [project/REQUIREMENTS_MATRIX.md](project/REQUIREMENTS_MATRIX.md)
2. See its implementation location, test status, and any deviations
3. Run tests: `dotnet test --filter "RequirementId=FR-01"`
4. Check auto-generated details: [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md)

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests for specific requirement
dotnet test --filter "RequirementId=FR-01"

# Generate traceability report
dotnet run --project tests/SysDocs.Tests -- --traceability

# Verify license compliance (all dependencies are open-source)
dotnet run --project tests/SysDocs.Tests -- --license-compliance
```

---

## 📋 License Compliance

**All dependencies are 100% open-source** ✅

SysDocs and all its dependencies use MIT, Apache-2.0, or BSD licenses. Automated compliance testing ensures no proprietary components are introduced.

```bash
# Run compliance test
dotnet test --filter "FullyQualifiedName~Constraint_ShouldUseOnlyOpenSourceDependencies"

# Generate detailed report
dotnet run --project tests/SysDocs.Tests -- --license-compliance
```

See [reports/LICENSE_COMPLIANCE.md](reports/LICENSE_COMPLIANCE.md) for current dependency list and [docs/LICENSE_COMPLIANCE_SUMMARY.md](docs/LICENSE_COMPLIANCE_SUMMARY.md) for policies.

---

## 🐳 Docker Deployment

The project builds into a self-contained Docker image suitable for CI/CD pipelines:

```bash
# Build
docker build -t sysdocs:latest .

# Run in CI/CD
docker run --rm \
  -v $(pwd)/input:/input \
  -v $(pwd)/output:/output \
  sysdocs:latest \
  --input /input/requirements.md \
  --output /output/requirements.pdf \
  --template incose
```

---

## 🎓 Use Cases

1. **Requirements Documentation** - Generate stakeholder and system requirements documents
2. **Design Documentation** - Create architecture and design specifications
3. **V&V Documentation** - Produce verification and validation matrices
4. **Traceability Reports** - Automated traceability matrix generation
5. **Change Management** - Track and report document changes over time

---

## 🤝 Contributing

We welcome contributions! Please see [project/CONTRIBUTING.md](project/CONTRIBUTING.md) for:

- Development environment setup
- Code style guidelines
- Testing requirements
- Pull request process

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🔗 Resources

- [INCOSE SE Handbook](https://www.incose.org/products-and-publications/se-handbook)
- [V-Model](https://en.wikipedia.org/wiki/V-Model)
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Docker Documentation](https://docs.docker.com/)
- [Nix Manual](https://nixos.org/manual/nix/stable/)

---

**Maintainer**: CodeOOf  
**Version**: 0.1.0-alpha  
**Status**: Early Development

