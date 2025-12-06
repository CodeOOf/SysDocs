## Description

<!-- Provide a brief description of the changes -->

**Requirement ID(s)**: <!-- e.g., FR-01, NFR-03 -->  
**Issue/Feature**: <!-- Link to issue or describe feature -->

## Type of Change

- [ ] 🐛 Bug fix (non-breaking change which fixes an issue)
- [ ] ✨ New feature (non-breaking change which adds functionality)
- [ ] 💥 Breaking change (fix or feature that would cause existing functionality to not work as expected)
- [ ] 📝 Documentation update
- [ ] ♻️ Code refactoring
- [ ] ✅ Test addition/improvement
- [ ] 🔧 Build/CI configuration

## Changes Made

<!-- List the main changes -->

- 
- 
- 

## Affected Requirements

<!-- List requirements affected by this PR - check REQUIREMENTS_MATRIX.md -->

| Requirement | Change Type | Updated Docs |
|-------------|-------------|--------------|
| FR-XX | Implementation/Tests/Docs | ✅ Yes / ❌ No |

## Testing Performed

### Automated Tests
- [ ] All existing tests pass: `dotnet test`
- [ ] New tests added for new functionality
- [ ] Tests properly tagged with `[RequirementTest("XX-YY")]`
- [ ] Cross-platform compatibility considered

### Manual Tests
<!-- Describe manual testing performed -->

- 
- 

### Test Results
```bash
# Paste test output here
```

## Documentation Updates

- [ ] REQUIREMENTS.md updated (if requirement changed)
- [ ] REQUIREMENTS_MATRIX.md updated (if V&V status changed)
- [ ] TRACEABILITY.md updated (if implementation approach changed)
- [ ] DEVIATIONS.md updated (if deviation needed)
- [ ] Test traceability regenerated: `dotnet run --project tests/SysDocs.Tests -- --traceability`
- [ ] License compliance checked: `dotnet run --project tests/SysDocs.Tests -- --license-compliance`
- [ ] No hardcoded numbers or temporal status in manual docs

## Build Verification

- [ ] Local build succeeds: `dotnet build src/SysDocs.sln`
- [ ] Local tests pass: `dotnet test`
- [ ] Nix build succeeds (if available): `nix build`
- [ ] Docker build succeeds (optional): `docker build -t sysdocs:test .`

## Manual V&V Checklist

### 🔍 Automated Requirements (Verified by Tests)
<!-- For requirements with automated tests, verify tests pass -->
- [ ] All automated tests for affected requirements pass
- [ ] CI/CD pipeline passes all checks

### ✋ Manual Requirements Verification

**Only check if this PR affects these requirements:**

#### FR-08: Change and trace report
- [ ] N/A - Not affected by this PR
- [ ] ✅ Manual review: Change reports are accurate
- [ ] ✅ Manual review: Git integration works correctly
- [ ] ✅ Manual review: Diff generation is correct

#### FR-09: Tool qualification
- [ ] N/A - Not affected by this PR
- [ ] ✅ Manual review: Audit logs are complete
- [ ] ✅ Manual review: Validation steps documented
- [ ] ✅ Manual review: Qualification package updated

#### FR-10: INCOSE alignment
- [ ] N/A - Not affected by this PR
- [ ] ✅ Manual review: Templates align with INCOSE SE Handbook
- [ ] ✅ Manual review: SE artifact structure is correct
- [ ] ✅ Manual review: Content meets INCOSE standards

#### FR-11: V-Model support
- [ ] N/A - Not affected by this PR
- [ ] ✅ Manual review: Templates align with V-Model phases
- [ ] ✅ Manual review: Artifact completeness checked
- [ ] ✅ Manual review: Phase relationships correct

#### NFR-03: Performance (if implementation could affect performance)
- [ ] N/A - Not affected by this PR
- [ ] ✅ Manual review: Performance benchmarks run
- [ ] ✅ Manual review: Large document tests completed (>250 pages)
- [ ] ✅ Manual review: Memory usage acceptable

#### NFR-06: Security (if touching credentials/security)
- [ ] N/A - Not affected by this PR
- [ ] ✅ Manual review: No credentials in code/logs
- [ ] ✅ Manual review: Environment variables properly used
- [ ] ✅ Manual review: Security audit completed

## Breaking Changes

<!-- If breaking changes, describe impact and migration path -->

- [ ] No breaking changes
- [ ] Breaking changes documented below:

**Impact**:

**Migration Path**:

## Checklist for Contributor

- [ ] My code follows the project's code style
- [ ] I have performed a self-review of my code
- [ ] I have commented my code, particularly in hard-to-understand areas
- [ ] I have updated the documentation accordingly
- [ ] My changes generate no new warnings
- [ ] I have added tests that prove my fix is effective or that my feature works
- [ ] New and existing unit tests pass locally with my changes
- [ ] I have checked REQUIREMENTS_MATRIX.md for affected requirements
- [ ] I have completed the manual V&V checklist above
- [ ] I have rebased on latest main and resolved conflicts

## Checklist for Reviewer

**See [BRANCH_STRATEGY.md](../BRANCH_STRATEGY.md#reviewer-guidelines) for detailed review guidance**

### Code Review
- [ ] Code quality is good (follows conventions, no anti-patterns)
- [ ] Error handling is appropriate
- [ ] Tests are comprehensive and properly tagged
- [ ] Documentation is accurate and complete

### V&V Review
- [ ] Contributor's manual V&V checklist is complete
- [ ] Manual requirements verification completed (if applicable)
- [ ] REQUIREMENTS_MATRIX.md checked for affected requirements
- [ ] All cross-references are correct
- [ ] No temporal status or hardcoded numbers in manual docs

### Verification Steps Performed
<!-- Reviewer: Document your verification steps -->

```bash
# Commands run to verify
dotnet test --filter "RequirementId=XX-YY"
# ... add other commands
```

**Manual verification findings**:
- 
- 

### Final Approval
- [ ] All automated checks pass
- [ ] Manual V&V checklist verified
- [ ] Code quality approved
- [ ] Documentation verified
- [ ] No unresolved conversations

---

## Additional Notes

<!-- Any additional context, concerns, or discussion points -->

---

**Related Documents:**
- [BRANCH_STRATEGY.md](../BRANCH_STRATEGY.md) - Branch strategy and review guidelines
- [REQUIREMENTS_MATRIX.md](../REQUIREMENTS_MATRIX.md) - V&V status per requirement
- [docs/DOCUMENTATION_NAVIGATION.md](../docs/DOCUMENTATION_NAVIGATION.md) - Documentation guide

