# Branch Strategy

**📖 Navigation**: [⬅️ Back: Contributing](../project/CONTRIBUTING.md) | [🏠 README](README.md) | [➡️ Next: Requirements Matrix](../project/REQUIREMENTS_MATRIX.md) | [🗺️ Docs Navigation](docs/DOCUMENTATION_NAVIGATION.md)

> 📝 **MANUAL DOCUMENTATION**  
> This document defines the branching strategy and merge requirements.

## Branch Structure

```
main (protected)
  ├── Stable, tested code
  ├── All manual V&V checklists validated
  └── Ready for release
      ↓
release/* (protected)
  ├── Release candidates
  ├── Version-specific branches (e.g., release/0.1.0)
  └── Cherry-picks from main for patches
      ↓
feature/* or forks
  ├── New features
  ├── Bug fixes
  └── Documentation updates
```

## Branches

### `main` - Protected Integration Branch
**Purpose**: Stable, tested code that has passed all V&V checks

**Protection Rules**:
- ✅ Require pull request reviews (1 approver minimum)
- ✅ Require status checks to pass:
  - Build & Test (all platforms)
  - Nix Build (deterministic)
  - Determinism Check
  - Manual V&V Checklist completed
- ✅ Require conversation resolution
- ✅ Require linear history (rebase/squash)
- ✅ Dismiss stale reviews on push
- ❌ No direct commits (PRs only)

**Merge From**: `feature/*` branches or forks  
**Merge To**: `release/*` branches

---

### `release/*` - Protected Release Branches
**Purpose**: Version-specific release preparation and maintenance

**Naming**: `release/X.Y.Z` (e.g., `release/0.1.0`)

**Protection Rules**:
- ✅ Require pull request reviews (2 approvers for releases)
- ✅ All `main` protection rules apply
- ✅ Additional manual verification required
- ✅ Release notes must be complete
- ❌ No direct commits (PRs only from main or hotfix branches)

**Merge From**: `main` (normal flow) or `hotfix/*` (critical patches)  
**Merge To**: Git tags for releases

**Lifecycle**:
1. Created from `main` when preparing release
2. Final testing and documentation updates
3. Tagged with version (e.g., `v0.1.0`)
4. Remains for patch management

---

### `feature/*` - Development Branches
**Purpose**: Feature development, bug fixes, documentation updates

**Naming Convention**:
- `feature/fr-XX-description` - New features (e.g., `feature/fr-01-markdown-import`)
- `feature/nfr-XX-description` - Non-functional features
- `feature/fix-issue-description` - Bug fixes
- `feature/docs-description` - Documentation updates

**Workflow**:
1. Branch from `main`
2. Develop and commit
3. Keep up to date with `main` (rebase regularly)
4. Open PR to `main` when ready
5. Complete PR checklist
6. Request review
7. Merge via squash or rebase

**Best Practices**:
- Keep focused on single requirement/issue
- Commit messages reference requirement IDs
- Update tests and documentation
- Run V&V checks locally before PR

---

### `hotfix/*` - Emergency Fixes (if needed)
**Purpose**: Critical production fixes

**Naming**: `hotfix/X.Y.Z-description`

**Workflow**:
1. Branch from `release/*` branch
2. Fix issue
3. Test thoroughly
4. PR to `release/*` branch
5. After merge, cherry-pick to `main`

---

## Merge Requirements

### Pull Request to `main`

**Required Checks (Automated)**:
- ✅ Build & Test passes on Ubuntu, Windows, macOS
- ✅ Nix build succeeds
- ✅ Determinism check passes
- ✅ No merge conflicts
- ✅ All conversations resolved

**Required Checks (Manual - Reviewer)**:
- ✅ PR checklist completed by contributor
- ✅ Manual V&V checklist completed (see PR template)
- ✅ Code review passed
- ✅ Documentation updated

**Merge Method**: Squash or Rebase (prefer squash for features)

---

### Pull Request to `release/*`

**All `main` requirements PLUS**:
- ✅ Version bump in project files
- ✅ CHANGELOG.md updated
- ✅ Release notes drafted
- ✅ All requirements verified in REQUIREMENTS_MATRIX.md
- ✅ Full V&V audit completed
- ✅ Two approvers required

**Merge Method**: Merge commit (preserve release history)

---

## Manual V&V Checklist

### For Every PR to `main`

The following must be verified by **both contributor and reviewer**:

#### Documentation Verification
- [ ] If requirement changed: REQUIREMENTS.md updated
- [ ] If implementation changed: TRACEABILITY.md reviewed for accuracy
- [ ] If test added/changed: Tests run successfully locally
- [ ] If deviation needed: DEVIATIONS.md updated with justification
- [ ] REQUIREMENTS_MATRIX.md reviewed for affected requirements
- [ ] No hardcoded numbers or temporal status in manual docs

#### Test Verification
- [ ] All tests pass: `dotnet test`
- [ ] Test traceability report regenerated if needed
- [ ] License compliance report regenerated if dependencies changed
- [ ] Cross-platform compatibility considered

#### Build Verification
- [ ] Local build succeeds: `dotnet build src/SysDocs.sln`
- [ ] Nix build succeeds (if Nix available): `nix build`
- [ ] Docker build succeeds (optional): `docker build -t sysdocs:test .`

#### Manual Requirements Verification

For requirements that cannot be automatically tested, reviewer must verify:

**FR-08: Change and trace report** (if affected)
- [ ] Manual review: Change reports are accurate
- [ ] Manual review: Git integration works correctly
- [ ] Manual review: Diff generation is correct

**FR-09: Tool qualification** (if affected)
- [ ] Manual review: Audit logs are complete
- [ ] Manual review: Validation steps documented
- [ ] Manual review: Qualification package updated

**FR-10: INCOSE alignment** (if affected)
- [ ] Manual review: Templates align with INCOSE SE Handbook
- [ ] Manual review: SE artifact structure is correct
- [ ] Manual review: Content meets INCOSE standards

**FR-11: V-Model support** (if affected)
- [ ] Manual review: Templates align with V-Model phases
- [ ] Manual review: Artifact completeness checked
- [ ] Manual review: Phase relationships correct

**NFR-03: Performance** (if affected)
- [ ] Manual review: Performance benchmarks run
- [ ] Manual review: Large document tests completed
- [ ] Manual review: Memory usage acceptable

**NFR-06: Security** (if affected)
- [ ] Manual review: No credentials in code/logs
- [ ] Manual review: Environment variables properly used
- [ ] Manual review: Security audit completed

---

## Workflow Examples

### Feature Development Workflow

```bash
# 1. Create feature branch from main
git checkout main
git pull origin main
git checkout -b feature/fr-01-markdown-import

# 2. Develop feature
# - Write code
# - Write tests with [RequirementTest("FR-01")]
# - Update TRACEABILITY.md if needed
# - Update REQUIREMENTS_MATRIX.md if needed

# 3. Keep up to date with main
git fetch origin main
git rebase origin/main

# 4. Run local V&V checks
dotnet build src/SysDocs.sln
dotnet test
dotnet run --project tests/SysDocs.Tests -- --traceability
dotnet run --project tests/SysDocs.Tests -- --license-compliance

# 5. Push and create PR
git push origin feature/fr-01-markdown-import

# 6. Open PR on GitHub
# - Fill out PR template completely
# - Complete manual V&V checklist
# - Request review

# 7. Address review feedback
# - Make changes
# - Push updates
# - Re-request review

# 8. After approval, merge via squash
```

### Release Workflow

```bash
# 1. Verify main is ready
git checkout main
git pull origin main
dotnet test
nix build

# 2. Create release branch
git checkout -b release/0.1.0

# 3. Update version numbers
# - Update .csproj files
# - Update CHANGELOG.md
# - Update version badges

# 4. Final testing
dotnet test
nix build
# Verify hash: sha256sum result

# 5. Push release branch
git push origin release/0.1.0

# 6. Create PR to release/0.1.0
# - Complete full V&V audit checklist
# - Update release notes
# - Get 2 approvals

# 7. After merge, create tag
git tag -a v0.1.0 -m "Release version 0.1.0"
git push origin v0.1.0

# 8. Create GitHub release
# - Attach release notes
# - Attach Nix build artifacts
# - Publish release
```

### Hotfix Workflow

```bash
# 1. Branch from release
git checkout release/0.1.0
git checkout -b hotfix/0.1.1-critical-bug

# 2. Fix issue
# - Write test reproducing bug
# - Fix bug
# - Verify test passes

# 3. PR to release branch
git push origin hotfix/0.1.1-critical-bug
# Open PR to release/0.1.0

# 4. After merge to release
# - Tag new version: v0.1.1
# - Cherry-pick to main
git checkout main
git cherry-pick <commit-hash>
git push origin main
```

---

## Reviewer Guidelines

### Code Review Checklist

**Code Quality**:
- [ ] Code follows C# conventions
- [ ] No code smells or anti-patterns
- [ ] Error handling is appropriate
- [ ] Logging is sufficient

**Testing**:
- [ ] Tests are comprehensive
- [ ] Tests use correct [RequirementTest] attributes
- [ ] Edge cases covered
- [ ] Tests are deterministic

**Documentation**:
- [ ] Code comments are clear
- [ ] Public APIs documented
- [ ] TRACEABILITY.md accurate
- [ ] REQUIREMENTS_MATRIX.md updated

**V&V Compliance**:
- [ ] Manual V&V checklist completed
- [ ] Requirements properly tracked
- [ ] Deviations justified if present
- [ ] Cross-references correct

### How to Review Manual Requirements

For requirements marked ⚠️ in REQUIREMENTS_MATRIX.md:

1. **Find the requirement** in REQUIREMENTS_MATRIX.md
2. **Check the verification section** - lists what to review
3. **Review the implementation** - check files listed
4. **Verify against standard** (INCOSE, V-Model, etc.)
5. **Check in PR** - Confirm checklist item completed
6. **Document findings** - Comment on PR with verification results

### Approval Criteria

**Approve when**:
- All automated checks pass
- Manual V&V checklist complete
- Code quality is good
- Documentation is accurate
- No unresolved conversations

**Request changes when**:
- Checklist incomplete
- Tests missing or failing
- Documentation out of sync
- Manual requirements not verified
- Code quality issues

**Comment only when**:
- Suggestions for improvement
- Questions for clarification
- Non-blocking feedback

---

## Automation

### GitHub Actions

All PRs automatically run:
- Build on Ubuntu, Windows, macOS
- Test suite execution
- Nix deterministic build
- Determinism verification
- Docker image build (on push to main)

### Status Checks Required

Before merge to `main`:
- ✅ `build-ubuntu-latest`
- ✅ `build-windows-latest`
- ✅ `build-macos-latest`
- ✅ `nix-build`
- ✅ `determinism-check`
- ✅ Manual V&V checklist (verified by reviewer)

---

## References

- [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md) - V&V status per requirement
- [docs/DOCUMENTATION_NAVIGATION.md](docs/DOCUMENTATION_NAVIGATION.md) - Documentation guide
- [CONTRIBUTING.md](../project/CONTRIBUTING.md) - Contribution guidelines
- [VERIFICATION_VALIDATION.md](../project/VERIFICATION_VALIDATION.md) - V&V strategy

---

**Questions?** See [docs/DOCUMENTATION_NAVIGATION.md](docs/DOCUMENTATION_NAVIGATION.md) or open an issue.

