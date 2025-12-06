# Contributing to SysDocs

**📖 Navigation**: [⬅️ Back to README](../README.md) | [➡️ Next: Branch Strategy](BRANCH_STRATEGY.md) | [🗺️ Docs Navigation](../docs/DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This is a **HUMAN-MAINTAINED GUIDE** for contributors.  
> For current test/license status, regenerate the auto-generated reports (see commands below).  
> For branch strategy and PR requirements, see: [BRANCH_STRATEGY.md](BRANCH_STRATEGY.md)

Thank you for your interest in contributing to SysDocs! This guide will help you set up your development environment and understand our development workflow.

---

## Quick Links

- **[BRANCH_STRATEGY.md](../project/BRANCH_STRATEGY.md)** - Branch strategy, PR checklists, and review guidelines
- **[REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)** - V&V status per requirement
- **[docs/DOCUMENTATION_NAVIGATION.md](docs/DOCUMENTATION_NAVIGATION.md)** - Documentation guide

---

## Table of Contents

1. [Getting Started](#getting-started)
2. [Development Environment Setup](#development-environment-setup)
3. [Branch Strategy & Workflow](#branch-strategy--workflow)
4. [Building the Project](#building-the-project)
5. [Testing](#testing)
6. [Deterministic Builds with Nix](#deterministic-builds-with-nix)
7. [Code Style](#code-style)
8. [Commit Guidelines](#commit-guidelines)
9. [Pull Request Process](#pull-request-process)
10. [Architecture Overview](#architecture-overview)

---

## Getting Started

### ⚠️ Important: Nix is Mandatory for Official Builds

**SysDocs requires Nix for all official builds to ensure 100% determinism.** While you can use `dotnet` for quick local development, **all builds that produce artifacts must use Nix**.

### Prerequisites

**Required:**
- [Nix](https://nixos.org/download.html) with flakes enabled
- [Git](https://git-scm.com/)

**Platform-Specific:**
- **Linux/macOS**: Nix runs natively
- **Windows**: Requires WSL2 (see [docs/DETERMINISTIC_BUILDS.md](docs/DETERMINISTIC_BUILDS.md))

**Optional (for quick iteration only):**
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (for local development)
- A code editor ([VS Code](https://code.visualstudio.com/), [Visual Studio](https://visualstudio.microsoft.com/), or [Rider](https://www.jetbrains.com/rider/))

### Clone the Repository

```bash
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs
```

---

## Branch Strategy & Workflow

**See [BRANCH_STRATEGY.md](../project/BRANCH_STRATEGY.md) for complete details.**

### Quick Overview

**Branch Structure**:
- `main` - Protected, stable integration branch
- `release/*` - Protected, version-specific release branches
- `feature/*` - Development branches for features/fixes
- `hotfix/*` - Emergency fixes (rare)

**Development Workflow**:
1. Branch from `main`: `git checkout -b feature/fr-XX-description`
2. Develop and test locally
3. Open PR to `main` with completed checklist
4. Pass automated checks + manual V&V review
5. Merge after approval

**PR Requirements**:
- ✅ All automated tests pass
- ✅ Nix build succeeds
- ✅ Manual V&V checklist completed
- ✅ Documentation updated
- ✅ One reviewer approval

**Manual V&V Requirements**:

For PRs affecting these requirements, reviewer must manually verify:
- **FR-08**: Change reports (accuracy, Git integration, diffs)
- **FR-09**: Tool qualification (audit logs, validation steps)
- **FR-10**: INCOSE alignment (template review vs handbook)
- **FR-11**: V-Model support (phase alignment, artifacts)
- **NFR-03**: Performance (benchmarks, large docs >250 pages)
- **NFR-06**: Security (credential handling, no secrets in code)

See [BRANCH_STRATEGY.md#manual-vv-checklist](BRANCH_STRATEGY.md#manual-vv-checklist) for complete checklist.

---

## Development Environment Setup

### Nix Development Shell (Recommended)

The Nix development shell provides all dependencies in a reproducible environment:

```bash
# Enter the development environment
nix develop

# You now have access to all tools:
# - dotnet SDK
# - docker
# - git
# - all required libraries
```

### Windows Users: WSL2 Setup

If you're on Windows, you **must** use WSL2 to run Nix:

1. **Install WSL2:**
   ```powershell
   wsl --install
   ```

2. **Enter WSL2:**
   ```powershell
   wsl
   ```

3. **Install Nix in WSL2:**
   ```bash
   sh <(curl -L https://nixos.org/nix/install) --daemon
   
   # Enable flakes
   mkdir -p ~/.config/nix
   echo "experimental-features = nix-command flakes" >> ~/.config/nix/nix.conf
   ```

4. **Access your Windows files:**
   ```bash
   cd /mnt/c/Users/YourUsername/source/SysDocs
   ```

See [docs/DETERMINISTIC_BUILDS.md](docs/DETERMINISTIC_BUILDS.md) for detailed instructions.

### VS Code Setup

1. Install recommended extensions:
   - C# Dev Kit
   - C#
   - NuGet Package Manager
   - Nix IDE

2. Open the project:
   ```bash
   code .
   ```

3. For WSL2 users, install "WSL" extension and use "Remote-WSL" mode

---

## Building the Project

### Official Builds (Mandatory for Releases)

**All official builds MUST use Nix** to ensure determinism:

```bash
# Build the application
nix build

# Build the Docker image (deterministic)
nix build .#docker

# Load Docker image
docker load < result

# Verify determinism (hash should be identical across builds)
sha256sum result
```

### Quick Development Iteration (Optional)

For rapid testing during development, you can use `dotnet` directly:

```bash
# Enter Nix shell first (ensures correct dependencies)
nix develop

# Quick build
dotnet build src/SysDocs.sln

# Quick test
dotnet test
```

**⚠️ Important:** Builds made with `dotnet` directly are **not deterministic** and should never be used for releases or official artifacts.

---

## Testing

### Running Tests with Nix (Recommended)

```bash
# Nix automatically runs tests during build
nix build

# Or run tests explicitly
nix develop
dotnet test tests/SysDocs.Tests/SysDocs.Tests.csproj
```

### Quick Test Iteration

```bash
# Run all tests
dotnet test src/SysDocs.Tests/SysDocs.Tests.csproj

# Run with detailed output
dotnet test --verbosity detailed

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Structure

- **Unit Tests**: `src/SysDocs.Tests/Core/` - Test individual components
- **Integration Tests**: `src/SysDocs.Tests/Integration/` - Test workflows
- **Determinism Tests**: Verify identical outputs across runs

### Writing Tests

We use **xUnit**, **FluentAssertions**, and **NSubstitute**:

```csharp
using FluentAssertions;
using Xunit;

public class ExampleTests
{
    [Fact]
    public void Example_ShouldWork()
    {
        // Arrange
        var input = "test";
        
        // Act
        var result = input.ToUpper();
        
        // Assert
        result.Should().Be("TEST");
    }
}
```

---

## Deterministic Builds with Nix

### Why Nix is Mandatory

SysDocs uses Nix to guarantee **100% reproducible builds**. This means:

✅ Same source code → Same binary output  
✅ Works identically on Windows (WSL2), Linux, and macOS  
✅ Cryptographically verifiable builds  
✅ No dependency drift over time

**Traditional Docker is NOT deterministic:**
- Base images can change under the same tag
- `apt-get update` gets different package versions
- Build timestamps vary
- Different file system ordering

See [docs/DETERMINISTIC_BUILDS.md](docs/DETERMINISTIC_BUILDS.md) for complete details.

### Nix Commands

```bash
# Build application
nix build

# Build Docker image (deterministic!)
nix build .#docker

# Enter development shell
nix develop

# Run the application directly
nix run

# Update dependencies
nix flake update

# Check flake
nix flake check
```

### Verifying Determinism

```bash
# Build twice and compare hashes
nix build .#docker && sha256sum result > build1.txt
rm result
nix build .#docker && sha256sum result > build2.txt
diff build1.txt build2.txt  # Should be identical!
```

### Docker Image Usage

```bash
# Build with Nix
nix build .#docker

# Load into Docker
docker load < result

# Run
docker run sysdocs:0.1.0-alpha --help

# Verify the image hash
sha256sum result
```

------

## Code Style

### C# Conventions

- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use **nullable reference types** (`#nullable enable`)
- Use **file-scoped namespaces** where appropriate
- XML documentation comments for public APIs

### EditorConfig

The project includes `.editorconfig` for consistent formatting:

```bash
# Format code
dotnet format
```

### Code Analysis

- Treat warnings as errors (`<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`)
- Latest analysis level (`<AnalysisLevel>latest</AnalysisLevel>`)
- Code style enforcement in builds

---

## Commit Guidelines

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

**Types:**
- `feat`: New feature
- `fix`: Bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks
- `build`: Build system changes
- `ci`: CI/CD changes

**Example:**
```
feat(importers): add LaTeX document importer

Implement LaTeX importer using TexSoup library to parse
.tex files and convert them to internal document model.

Closes #42
```

---

## Pull Request Process

**See [BRANCH_STRATEGY.md](../project/BRANCH_STRATEGY.md) for detailed PR requirements and review guidelines.**

### Quick PR Workflow

1. **Create Feature Branch**
   ```bash
   git checkout main
   git pull origin main
   git checkout -b feature/fr-XX-description
   ```

2. **Develop with V&V in Mind**
   - Write code following conventions
   - Add tests with `[RequirementTest("XX-YY")]` attributes
   - Update REQUIREMENTS_MATRIX.md if V&V status changes
   - Update TRACEABILITY.md if implementation approach changes
   - Run `dotnet test` locally

3. **Pre-PR Checklist**
   ```bash
   # Build
   dotnet build src/SysDocs.sln
   
   # Test
   dotnet test
   
   # Regenerate reports if needed
   dotnet run --project tests/SysDocs.Tests -- --traceability
   dotnet run --project tests/SysDocs.Tests -- --license-compliance
   
   # Format
   dotnet format
   
   # Nix build (if available)
   nix build
   ```

4. **Push & Create PR**
   ```bash
   git push origin feature/fr-XX-description
   ```
   - Use PR template (auto-populated)
   - Complete all checklist items
   - Mark N/A for manual requirements not affected

5. **Review Process**
   - CI checks must pass (build, test, Nix, determinism)
   - Manual V&V checklist verified by reviewer
   - Code review feedback addressed
   - All conversations resolved

6. **Merge**
   - Squash merge preferred (clean history)
   - Maintainer merges after approval

### What Reviewers Check

**Automated** (CI/CD):
- ✅ Builds on Ubuntu, Windows, macOS
- ✅ All tests pass
- ✅ Nix build succeeds
- ✅ Determinism verified
- ✅ Reports are up to date

**Manual** (Reviewer):
- ✅ Code quality and conventions
- ✅ Tests are comprehensive
- ✅ Documentation is accurate
- ✅ Manual V&V checklist completed
- ✅ Manual requirements verified (if applicable)

See [BRANCH_STRATEGY.md#reviewer-guidelines](BRANCH_STRATEGY.md#reviewer-guidelines) for complete review guidelines.

---

## Architecture Overview

### Project Structure

```
src/
├── SysDocs.Cli/              # Command-line interface
│   └── Program.cs            # Entry point, DI setup
│
├── SysDocs.Core/             # Core business logic
│   ├── Model/                # Document object model
│   │   └── Document.cs       # Internal representation
│   ├── Importers/            # Input format handlers
│   │   ├── IDocumentImporter.cs
│   │   ├── MarkdownImporter.cs
│   │   ├── WordImporter.cs
│   │   └── PdfImporter.cs
│   ├── Exporters/            # Output format handlers
│   │   └── PdfExporter.cs    # Deterministic PDF generation
│   ├── Templates/            # Template engine
│   ├── Tracing/              # Change tracking
│   └── Qualification/        # Tool qualification support
│
└── SysDocs.Templates/        # SE artifact templates
    ├── Templates/            # Template definitions
    └── Fonts/                # Embedded fonts (determinism)

tests/
└── SysDocs.Tests/            # Test suite
    ├── Core/                 # Unit tests
    └── Integration/          # Integration tests
```

### Data Flow

1. **Import** → Parse input (MD/Word/LaTeX/PDF) → Internal Model
2. **Process** → Apply templates, Git metadata, traceability
3. **Export** → Generate deterministic PDF

### Key Design Principles

- **Determinism**: Same input → same output, always
- **Portability**: Works identically on Windows, Linux, macOS
- **Traceability**: Full audit trail of changes
- **Qualification**: Suitable for DO-178C/DO-330 workflows
- **Modularity**: Clean separation of concerns

---

## Questions?

- Open an issue on GitHub
- Check existing issues and discussions
- Read the [README](README.md) and [REQUIREMENTS](../project/REQUIREMENTS.md)

Thank you for contributing to SysDocs! 🚀

