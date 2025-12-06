# Documentation Directory

**📖 Navigation**: [🏠 Back to Main README](../README.md) | [🗺️ Docs Navigation Guide](DOCUMENTATION_NAVIGATION.md)

This directory contains all supporting documentation for the SysDocs project.

---

## 📚 Complete Documentation Index

### 🎯 Core Project Documents (Root Level)

Start from the main README and follow the reader thread:

1. **[../README.md](../README.md)** - Project overview, quick start, and navigation hub
2. **[../CONTRIBUTING.md](../CONTRIBUTING.md)** - Development setup and contribution guidelines
3. **[../BRANCH_STRATEGY.md](../BRANCH_STRATEGY.md)** - Git workflow and PR process

### 📋 Requirements & V&V Documents (Root Level)

The complete V&V documentation thread:

4. **[../REQUIREMENTS_MATRIX.md](../REQUIREMENTS_MATRIX.md)** ⭐ Central V&V reference
5. **[../REQUIREMENTS.md](../REQUIREMENTS.md)** - Detailed requirements specification
6. **[../TRACEABILITY.md](../TRACEABILITY.md)** - Implementation mapping
7. **[../VERIFICATION_VALIDATION.md](../VERIFICATION_VALIDATION.md)** - V&V strategy
8. **[../DEVIATIONS.md](../DEVIATIONS.md)** - Approved technical deviations

### 📊 Auto-Generated Reports (Root /reports/)

9. **[../reports/TEST_TRACEABILITY.md](../reports/TEST_TRACEABILITY.md)** - Test coverage report
10. **[../reports/LICENSE_COMPLIANCE.md](../reports/LICENSE_COMPLIANCE.md)** - Dependency licenses

### 🔧 Technical Documentation (This Directory)

#### Determinism & Build System
11. **[DETERMINISM_EXPLAINED.md](DETERMINISM_EXPLAINED.md)** - Build vs output determinism explained
12. **[DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md)** - Nix setup and usage guide
13. **[DETERMINISTIC_BUILD_SUMMARY.md](DETERMINISTIC_BUILD_SUMMARY.md)** - Build system implementation summary
14. **[PLATFORM_ARCHITECTURE.md](PLATFORM_ARCHITECTURE.md)** - Platform strategy: Docker-only deployment, cross-platform development
15. **[WINDOWS_BUILD.md](WINDOWS_BUILD.md)** - Windows/WSL2 setup instructions

#### License & Compliance
16. **[LICENSE_COMPLIANCE_SUMMARY.md](LICENSE_COMPLIANCE_SUMMARY.md)** - Compliance procedures and policy

#### Security
17. **[CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)** - Code signing setup and implementation guide
18. **[DEVELOPER_SETUP_SIGNING.md](DEVELOPER_SETUP_SIGNING.md)** - Quick start guide for GPG and Cosign setup (Windows/Linux)
19. **[GITHUB_SECRETS_SETUP.md](GITHUB_SECRETS_SETUP.md)** - Configure GitHub Secrets for release pipeline
20. **[GITHUB_RELEASE_PROCESS.md](GITHUB_RELEASE_PROCESS.md)** - Complete GitHub release workflow and procedures

#### Maintenance & Guidelines
21. **[MAINTENANCE_GUIDELINES.md](MAINTENANCE_GUIDELINES.md)** - How to maintain documentation
19. **[DOCUMENTATION_NAVIGATION.md](DOCUMENTATION_NAVIGATION.md)** 🗺️ Navigation guide (you are here!)
20. **[DOCUMENTATION_GUIDE.md](DOCUMENTATION_GUIDE.md)** - Documentation writing guide

#### Systems Engineering Standards
21. **[SE_ARTIFACT_DEFINITIONS.md](SE_ARTIFACT_DEFINITIONS.md)** - V-Model phases, INCOSE mappings, artifact definitions

#### V&V Setup Reference
22. **[VV_SETUP_SUMMARY.md](VV_SETUP_SUMMARY.md)** - V&V setup history and implementation details

---

## 🎯 Quick Access by Topic

### Getting Started
- New to the project? → [../README.md](../README.md)
- Want to contribute? → [../CONTRIBUTING.md](../CONTRIBUTING.md)
- Need to understand determinism? → [DETERMINISM_EXPLAINED.md](DETERMINISM_EXPLAINED.md)

### Requirements & Verification
- Check requirement status? → [../REQUIREMENTS_MATRIX.md](../REQUIREMENTS_MATRIX.md)
- Read requirements? → [../REQUIREMENTS.md](../REQUIREMENTS.md)
- Understand implementation? → [../TRACEABILITY.md](../TRACEABILITY.md)
- See test coverage? → [../reports/TEST_TRACEABILITY.md](../reports/TEST_TRACEABILITY.md)

### Building & Deployment
- Set up Nix builds? → [DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md)
- Using Windows? → [WINDOWS_BUILD.md](WINDOWS_BUILD.md)
- Understand build system? → [DETERMINISTIC_BUILD_SUMMARY.md](DETERMINISTIC_BUILD_SUMMARY.md)

### Compliance & Licensing
- Check licenses? → [../reports/LICENSE_COMPLIANCE.md](../reports/LICENSE_COMPLIANCE.md)
- Understand policy? → [LICENSE_COMPLIANCE_SUMMARY.md](LICENSE_COMPLIANCE_SUMMARY.md)

### Security & Code Signing
- Set up code signing? → [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)
- Verify signatures? → [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md#verification)
- Understand NFR-07? → [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)

### Systems Engineering
- Understand V-Model phases? → [SE_ARTIFACT_DEFINITIONS.md](SE_ARTIFACT_DEFINITIONS.md)
- Need INCOSE process mappings? → [SE_ARTIFACT_DEFINITIONS.md](SE_ARTIFACT_DEFINITIONS.md)
- Looking for artifact templates? → [SE_ARTIFACT_DEFINITIONS.md](SE_ARTIFACT_DEFINITIONS.md)

### Release Management
- Creating a release? → [GITHUB_RELEASE_PROCESS.md](GITHUB_RELEASE_PROCESS.md)
- Setting up code signing? → [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)
- Configuring GitHub Secrets? → [GITHUB_RELEASE_PROCESS.md](GITHUB_RELEASE_PROCESS.md#21-required-github-secrets)

---

## 📖 Reading Paths

### Path 1: Quick Start Developer
```
README → CONTRIBUTING → BRANCH_STRATEGY
```

### Path 2: Requirements Reviewer
```
README → REQUIREMENTS_MATRIX → REQUIREMENTS → TRACEABILITY → VERIFICATION_VALIDATION → DEVIATIONS
```

### Path 3: Build Engineer
```
README → DETERMINISM_EXPLAINED → DETERMINISTIC_BUILDS → DETERMINISTIC_BUILD_SUMMARY → WINDOWS_BUILD (if needed)
```

### Path 4: Compliance Auditor
```
README → REQUIREMENTS_MATRIX → LICENSE_COMPLIANCE_SUMMARY → LICENSE_COMPLIANCE (report)
```

### Path 5: Systems Engineer
```
README → REQUIREMENTS_MATRIX → SE_ARTIFACT_DEFINITIONS → REQUIREMENTS → TRACEABILITY → VERIFICATION_VALIDATION
```

### Path 6: Read Everything (Complete Thread)
Follow the navigation breadcrumbs starting from README.md - each document has ⬅️ Back and ➡️ Next links to guide you through all 20+ documents in logical order.

---

## 🔄 Document Maintenance

### Auto-Generated (DO NOT EDIT MANUALLY)
- `../reports/TEST_TRACEABILITY.md`
- `../reports/LICENSE_COMPLIANCE.md`

Regenerate with:
```bash
dotnet run --project tests/SysDocs.Tests -- --traceability
dotnet run --project tests/SysDocs.Tests -- --license-compliance
```

### Manual Documentation (MAINTAIN AS NEEDED)
All other `.md` files are human-maintained. When updating:
1. Maintain the navigation breadcrumbs
2. Update cross-references as needed
3. Keep the reader thread intact
4. Follow maintenance guidelines in [MAINTENANCE_GUIDELINES.md](MAINTENANCE_GUIDELINES.md)

---

**Lost?** Check the [🗺️ Documentation Navigation Guide](DOCUMENTATION_NAVIGATION.md) for visual flowcharts and quick reference!

