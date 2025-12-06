# Documentation Navigation Guide

**📖 Navigation**: [🏠 README](../README.md) - You can always return to the main page

> 📝 **MANUAL DOCUMENTATION**  
> Quick reference for finding V&V information.

## Reader's Thread: Following a Requirement

```
Start: I need to understand requirement FR-01
  ↓
1. [REQUIREMENTS_MATRIX.md](REQUIREMENTS_MATRIX.md#fr-01)
   └─ See: Implementation, Tests, Deviations (all in one place)
      ↓
2. Need more implementation details?
   └─ [TRACEABILITY.md](../project/TRACEABILITY.md) (architecture & design decisions)
      ↓
3. Need detailed test data?
   └─ [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) (auto-generated counts)
      ↓
4. Need deviation details?
   └─ [DEVIATIONS.md](../project/DEVIATIONS.md) (justification & remediation)
      ↓
5. Run the tests yourself:
   └─ `dotnet test --filter "RequirementId=FR-01"`
```

## Document Purpose

### Core Requirements Documents

| Document | Purpose | When to Use |
|----------|---------|-------------|
| **[REQUIREMENTS.md](../project/REQUIREMENTS.md)** | Source of truth for WHAT to build | Defining or reviewing requirements |
| **[REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)** | ⭐ **START HERE** - Complete V&V status per requirement | Checking if a requirement is implemented/tested/verified |
| **[TRACEABILITY.md](../project/TRACEABILITY.md)** | HOW requirements are implemented | Understanding architecture decisions |
| **[reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md)** | Current test counts and coverage | Getting latest test statistics |
| **[DEVIATIONS.md](../project/DEVIATIONS.md)** | Approved deviations with justification | Understanding why something differs from requirements |
| **[VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md)** | V&V strategy and procedures | Understanding the V&V approach |

### Build & Determinism Documents

| Document | Purpose | When to Use |
|----------|---------|-------------|
| **[docs/DETERMINISM_EXPLAINED.md](DETERMINISM_EXPLAINED.md)** | Explains build vs output determinism | Understanding the two types of determinism |
| **[docs/DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md)** | How to use Nix for reproducible builds | Setting up deterministic build environment |
| **[docs/DETERMINISTIC_BUILD_SUMMARY.md](DETERMINISTIC_BUILD_SUMMARY.md)** | Summary of build system changes | Understanding build system implementation |

### Quick Access by Need

#### "I need to verify requirement FR-XX is properly handled"
→ [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md) - See implementation, tests, and deviations in one view

#### "I need to understand how feature X is architected"
→ [TRACEABILITY.md](../project/TRACEABILITY.md) - See design decisions and implementation approach

#### "I need current test statistics"
→ [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) - Auto-generated test counts
→ Run: `dotnet run --project tests/SysDocs.Tests -- --traceability`

#### "I need to understand why we deviated from a requirement"
→ [DEVIATIONS.md](../project/DEVIATIONS.md) - See justification and remediation plans

#### "I need to add a new requirement"
1. Add to [REQUIREMENTS.md](../project/REQUIREMENTS.md)
2. Add to [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)
3. Update [TRACEABILITY.md](../project/TRACEABILITY.md) with implementation plan
4. Write tests with `[RequirementTest("XX-YY")]` attribute
5. Regenerate: `dotnet run --project tests/SysDocs.Tests -- --traceability`

#### "I need to check license compliance"
→ [reports/LICENSE_COMPLIANCE.md](reports/LICENSE_COMPLIANCE.md) - Auto-generated dependency licenses
→ See policies: [docs/LICENSE_COMPLIANCE_SUMMARY.md](docs/LICENSE_COMPLIANCE_SUMMARY.md)

#### "I need to understand determinism in SysDocs"
→ [docs/DETERMINISM_EXPLAINED.md](DETERMINISM_EXPLAINED.md) - Build vs output determinism
→ Build determinism (BR-02): Tool binary builds identically on Linux distros
→ Output determinism (FR-06, FR-07, NFR-01): Tool produces identical PDFs on all platforms

#### "I need to set up deterministic builds"
→ [docs/DETERMINISTIC_BUILDS.md](DETERMINISTIC_BUILDS.md) - Nix setup and usage

## Document Types

### 📝 Manual Documentation
Human-maintained, updated when processes/requirements change:
- REQUIREMENTS.md
- REQUIREMENTS_MATRIX.md
- TRACEABILITY.md
- DEVIATIONS.md
- VERIFICATION_VALIDATION.md
- README.md
- CONTRIBUTING.md
- All docs/*.md files

### ⚠️ Auto-Generated Reports
Machine-generated, DO NOT manually edit:
- reports/TEST_TRACEABILITY.md
- reports/LICENSE_COMPLIANCE.md

Regenerate with:
```bash
# Test coverage report
dotnet run --project tests/SysDocs.Tests -- --traceability

# License compliance report
dotnet run --project tests/SysDocs.Tests -- --license-compliance
```

## Information Flow

```
[REQUIREMENTS.md]
    │
    ├──> [REQUIREMENTS_MATRIX.md] ← Central V&V reference
    │        │
    │        ├──> Links to [TRACEABILITY.md] (implementation details)
    │        ├──> Links to [reports/TEST_TRACEABILITY.md] (test data)
    │        └──> Links to [DEVIATIONS.md] (deviations)
    │
    ├──> [TRACEABILITY.md] (implementation mapping)
    │
    ├──> [Tests with RequirementTest attributes]
    │        │
    │        └──> Generates [reports/TEST_TRACEABILITY.md]
    │
    └──> [DEVIATIONS.md] (when needed)
```

## Maintenance Workflows

### Adding a Test
```bash
1. Write test with [RequirementTest("XX-YY")] attribute
2. Run tests: dotnet test
3. Regenerate report: dotnet run --project tests/SysDocs.Tests -- --traceability
4. Verify in reports/TEST_TRACEABILITY.md
5. Update REQUIREMENTS_MATRIX.md test status if needed
```

### Adding a Requirement
```bash
1. Add to REQUIREMENTS.md
2. Add section to REQUIREMENTS_MATRIX.md with ❌ No tests status
3. Update TRACEABILITY.md with implementation approach
4. Write tests (mark ✅ Has tests in matrix)
5. Regenerate: dotnet run --project tests/SysDocs.Tests -- --traceability
```

### Adding a Deviation
```bash
1. Create DEV-XXX section in DEVIATIONS.md with justification
2. Update affected requirements in REQUIREMENTS_MATRIX.md with ⚠️ and link
3. Add note in TRACEABILITY.md if implementation is affected
4. Document remediation plan
```

## Finding Information

### By Requirement ID
1. **REQUIREMENTS_MATRIX.md** - Complete status
2. **REQUIREMENTS.md** - Detailed specification
3. **TRACEABILITY.md** - Implementation location
4. **reports/TEST_TRACEABILITY.md** - Test details

### By Implementation File
1. **TRACEABILITY.md** - Find which requirements it fulfills
2. **REQUIREMENTS_MATRIX.md** - Check those requirements' status

### By Test Name
1. **reports/TEST_TRACEABILITY.md** - Find which requirement(s) it tests
2. **REQUIREMENTS_MATRIX.md** - See requirement's full V&V status

### By Deviation ID
1. **DEVIATIONS.md** - Read justification and remediation
2. **REQUIREMENTS_MATRIX.md** - Find affected requirements

## Common Questions

**Q: How do I know if requirement FR-XX has tests?**  
A: Check [REQUIREMENTS_MATRIX.md](REQUIREMENTS_MATRIX.md#fr-xx) - Shows ✅/⚠️/❌ status

**Q: Where are the current test counts?**  
A: [reports/TEST_TRACEABILITY.md](reports/TEST_TRACEABILITY.md) - Auto-generated statistics

**Q: Why is requirement FR-XX implemented differently than specified?**  
A: Check [REQUIREMENTS_MATRIX.md](REQUIREMENTS_MATRIX.md#fr-xx) for deviation links, then [DEVIATIONS.md](../project/DEVIATIONS.md)

**Q: How do I run tests for a specific requirement?**  
A: `dotnet test --filter "RequirementId=FR-XX"`

**Q: Which file should I update when...**
- Requirement changes → REQUIREMENTS.md, REQUIREMENTS_MATRIX.md, TRACEABILITY.md
- Implementation changes → TRACEABILITY.md (if approach changes)
- Tests change → Regenerate TEST_TRACEABILITY.md
- Deviation needed → DEVIATIONS.md, REQUIREMENTS_MATRIX.md
- Process changes → VERIFICATION_VALIDATION.md

---

**When in doubt, start with [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md) - it's your central V&V navigator!**

