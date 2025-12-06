# Maintenance Guidelines

> 📝 **MANUAL DOCUMENTATION**  
> This is a **HUMAN-MAINTAINED GUIDE** explaining maintenance principles.

## Core Principle: Single Source of Truth

**Every piece of data should have exactly one source of truth.**

### The Problem We Solved

Documentation contained hardcoded numbers and temporal status updates (e.g., "20 dependencies", "16 of 24 requirements", "5 tests passing", "✅ Implemented", "⏳ Planned") that would become outdated as the project evolved. Developers would need to remember to update multiple files whenever tests were added, dependencies changed, or features were implemented.

### The Solution

**Auto-Generated Reports (Data Sources)**
- `reports/LICENSE_COMPLIANCE.md` - Current dependency list with licenses
- `reports/TEST_TRACEABILITY.md` - Current test count and requirement coverage

**Manual Documentation (References Only)**
- Never duplicate data from reports
- Reference reports with links
- Explain policies and procedures
- Document how to interpret reports

## How to Maintain Documentation

### ✅ DO: Reference Dynamic Data

```markdown
<!-- GOOD: References the report -->
See [reports/LICENSE_COMPLIANCE.md](reports/LICENSE_COMPLIANCE.md) for current dependencies.

<!-- GOOD: Explains how to get current data -->
For current test coverage, run:
dotnet run --project tests/SysDocs.Tests -- --traceability
```

### ❌ DON'T: Hardcode Statistics or Status

```markdown
<!-- BAD: Will become outdated -->
We have 20 dependencies, all open-source.

<!-- BAD: Will become outdated -->
16 of 24 requirements have tests.

<!-- BAD: Will become outdated -->
5 tests are currently passing.

<!-- BAD: Will become outdated -->
| FR-01 | Import Markdown | ✅ Implemented | ... |
| FR-02 | Import Images   | ⏳ Planned     | ... |

<!-- BAD: Will become outdated -->
### Current Status
- ✅ Document Model Tests (5 passing)
- ⏳ Importers (pending)
```

## File Types

### Auto-Generated (⚠️ Headers)

**Files:**
- `reports/LICENSE_COMPLIANCE.md`
- `reports/TEST_TRACEABILITY.md`

**Rules:**
- ⚠️ NEVER manually edit
- ⚠️ ALWAYS regenerate when data changes
- Contains: Counts, lists, statistics, current state

**How to Update:**
```bash
# When dependencies change
dotnet run --project tests/SysDocs.Tests -- --license-compliance

# When tests change
dotnet run --project tests/SysDocs.Tests -- --traceability
```

### Manual Documentation (📝 Headers)

**Files:**
- `README.md`
- `REQUIREMENTS.md`
- `TRACEABILITY.md`
- `VERIFICATION_VALIDATION.md`
- `docs/*.md` (except auto-generated reports)

**Rules:**
- 📝 Human-maintained policies and procedures
- 📝 Explains "why" and "how"
- 📝 References reports, never copies data
- 📝 Updated when policies or processes change
**What NOT to Include:**
- Specific package counts
- Test counts
- Requirement coverage percentages
- Specific package names/versions
- Implementation status (✅ Implemented, ⏳ Planned, etc.)
- "Current Status" sections
- "What's Working" / "What's Pending" lists
- Any data that changes when code changes
- Interpretation guides
- Architecture decisions

**What NOT to Include:**
- Specific package counts
- Test counts
- Requirement coverage percentages
- Specific package names/versions
- Any data that changes when code changes

## When to Update What

### You Added a Dependency

```bash
# 1. Add package
dotnet add package SomePackage

# 2. Regenerate license report
dotnet run --project tests/SysDocs.Tests -- --license-compliance

# 3. Verify license is acceptable
cat reports/LICENSE_COMPLIANCE.md

# 4. Commit both
git add src/**/*.csproj reports/LICENSE_COMPLIANCE.md
git commit -m "Add SomePackage dependency"
```

**DO NOT update:** README.md, LICENSE_COMPLIANCE_SUMMARY.md (they reference the report)

### You Added a Test

```bash
# 1. Write test with [RequirementTest("XX-YY")] attribute
# 2. Run tests
dotnet test

# 3. Regenerate traceability
dotnet run --project tests/SysDocs.Tests -- --traceability

# 4. Verify coverage
cat reports/TEST_TRACEABILITY.md

# 5. Commit both
git add tests/**/*.cs reports/TEST_TRACEABILITY.md
git commit -m "Add test for requirement XX-YY"
```

**DO NOT update:** README.md, TRACEABILITY.md, VV_SETUP_SUMMARY.md (they reference the report)

### You Changed a Policy

Example: Changed the audit schedule from quarterly to monthly

```bash
# 1. Edit the manual document
vim docs/LICENSE_COMPLIANCE_SUMMARY.md

# 2. Commit
git add docs/LICENSE_COMPLIANCE_SUMMARY.md
git commit -m "Update audit schedule to monthly"
```

**DO NOT regenerate:** Reports contain data, not policies

### You Added a Requirement

```bash
# 1. Add requirement to REQUIREMENTS.md
# 2. Add tests if testable
# 3. Regenerate traceability
dotnet run --project tests/SysDocs.Tests -- --traceability

# 4. Update TRACEABILITY.md if needed
# 5. Commit all
git add REQUIREMENTS.md tests/**/*.cs reports/TEST_TRACEABILITY.md TRACEABILITY.md
git commit -m "Add requirement XX-YY"
```

## Benefits of This Approach

### 1. Always Accurate
- Reports are generated from source code
- No manual counting needed
- No stale statistics

### 2. Easy to Maintain
- Developers don't need to remember to update counts
- Single command regenerates reports
- Clear separation of concerns

### 3. Audit-Friendly
- Commands show how data is generated
- Reproducible results
- Version controlled

### 4. Prevents Inconsistency
- Cannot have conflicting numbers across documents
- One source of truth per data point
- References always point to current data

## Quick Reference

### Commands to Know

```bash
# Check current dependency licenses
dotnet run --project tests/SysDocs.Tests -- --license-compliance

# Check current test coverage
dotnet run --project tests/SysDocs.Tests -- --traceability

# Run all tests
dotnet test

# Run specific requirement tests
dotnet test --filter "RequirementId=FR-01"

# Run license compliance test
dotnet test --filter "FullyQualifiedName~Constraint_ShouldUseOnlyOpenSourceDependencies"
```

### Files to Never Edit Manually

- ⚠️ `reports/LICENSE_COMPLIANCE.md` (regenerate with `--license-compliance`)
- ⚠️ `reports/TEST_TRACEABILITY.md` (regenerate with `--traceability`)

### Safe to Edit Anytime

- 📝 `docs/LICENSE_COMPLIANCE_SUMMARY.md` (policies)
- 📝 `docs/VV_SETUP_SUMMARY.md` (procedures)
- 📝 `README.md` (overview)
- 📝 `REQUIREMENTS.md` (requirements)
- 📝 `TRACEABILITY.md` (strategy)
### Before (Hardcoded Numbers and Status)

```markdown
<!-- README.md -->
SysDocs and all 20 of its dependencies use MIT, Apache-2.0, or BSD licenses.

<!-- TRACEABILITY.md -->
Current Test Coverage: 16 of 24 requirements have associated tests

| FR-01 | Import Markdown | ✅ Implemented | src/... | ... |
| FR-02 | Import Images   | ⏳ Planned     | src/... | ... |

<!-- VV_SETUP_SUMMARY.md -->
## Current Status

### What's Working ✅
1. Document Model Tests (5 passing)
2. Constraint Verification (3 passing)
### After (References)

```markdown
<!-- README.md -->
SysDocs and all its dependencies use MIT, Apache-2.0, or BSD licenses.
See reports/LICENSE_COMPLIANCE.md for current dependency list.

<!-- TRACEABILITY.md -->
Current Test Coverage: See reports/TEST_TRACEABILITY.md for detailed mapping

| FR-01 | Import Markdown | src/SysDocs.Core/Importers/ | MarkDig library |
| FR-02 | Import Images   | src/... | ImageSharp library |

<!-- VV_SETUP_SUMMARY.md -->
For current test counts and requirement coverage, see:
reports/TEST_TRACEABILITY.md
```

**Solution:** Numbers and status are always current in auto-generated reports. Documentation stays focused on requirements mapping, not temporal status.
See reports/LICENSE_COMPLIANCE.md for current dependency list.

<!-- TRACEABILITY.md -->
Current Test Coverage: See reports/TEST_TRACEABILITY.md for detailed mapping

<!-- VV_SETUP_SUMMARY.md -->
For current test counts and requirement coverage, see:
reports/TEST_TRACEABILITY.md
```

**Solution:** Numbers are always current. Just regenerate the reports.

## Architecture Decision

**Decision:** Separate auto-generated data from manual documentation

**Rationale:**
1. Prevent outdated statistics in documentation
2. Make maintenance easier (no manual counting)
3. Ensure consistency (one source of truth)
4. Improve accuracy (generated from code)
5. Support auditing (reproducible reports)

**Trade-offs:**
- Need to remember to regenerate reports (but CI can check this)
- Two-step process (code change → regenerate report)
- Readers must follow links for specific numbers

**Benefits outweigh trade-offs because:**
- Automation prevents human error
- Version control tracks changes
- Reports are fast to generate (<5 seconds)
- Clear ownership (machine vs human)

