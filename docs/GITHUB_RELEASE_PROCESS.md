# GitHub Release Process Guide

**📖 Navigation**: [⬅️ Back: Code Signing Guide](CODE_SIGNING_GUIDE.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> **Related Requirements**: NFR-07 (Code Signing), BR-01 to BR-05 (Build Requirements)

This guide explains how to create releases for the SysDocs project on GitHub, including code signing, multi-platform builds, and automated publishing.

---

## 1. Overview

The SysDocs release process is **fully automated** through GitHub Actions. When you push a version tag, the pipeline automatically:

1. ✅ Builds for Windows, Linux, and macOS
2. ✅ Signs all Windows binaries (NFR-07)
3. ✅ Runs all tests including signature verification
4. ✅ Creates NuGet packages and signs them
5. ✅ Creates a GitHub Release with all artifacts
6. ✅ Publishes to NuGet.org (for stable releases)
7. ✅ Builds and publishes Docker images

**No manual intervention required** for standard releases!

---

## 2. Prerequisites

### 2.1 Required GitHub Secrets

Before creating your first release, configure these secrets in your repository:

**Navigate to**: `https://github.com/CodeOOf/SysDocs/settings/secrets/actions`

#### Essential Secrets

| Secret Name | Required | Description |
|-------------|----------|-------------|
| `CODE_SIGNING_CERT` | ✅ Yes | Base64-encoded PFX certificate for signing Windows binaries |
| `CODE_SIGNING_PASSWORD` | ✅ Yes | Password for the code signing certificate |

#### Optional Secrets (for publishing)

| Secret Name | Required | Description |
|-------------|----------|-------------|
| `NUGET_API_KEY` | For NuGet | API key from https://www.nuget.org/account/apikeys |
| `DOCKER_USERNAME` | For Docker | Docker Hub username |
| `DOCKER_PASSWORD` | For Docker | Docker Hub access token |

### 2.2 Setting Up Code Signing Certificate

**Step 1: Acquire Certificate**
- Purchase a code signing certificate (see [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md))
- Export as PFX with password protection

**Step 2: Encode for GitHub**
```powershell
# Convert PFX to Base64
$bytes = [System.IO.File]::ReadAllBytes("path\to\certificate.pfx")
$base64 = [Convert]::ToBase64String($bytes)
$base64 | Out-File cert-base64.txt

# Copy the content and add to GitHub Secrets as CODE_SIGNING_CERT
Get-Content cert-base64.txt | Set-Clipboard

# Clean up
Remove-Item cert-base64.txt
```

**Step 3: Add to GitHub Secrets**
1. Go to: `https://github.com/CodeOOf/SysDocs/settings/secrets/actions`
2. Click "New repository secret"
3. Name: `CODE_SIGNING_CERT`
4. Value: Paste the base64 string
5. Click "Add secret"

**Step 4: Add Certificate Password**
1. Click "New repository secret"
2. Name: `CODE_SIGNING_PASSWORD`
3. Value: Your certificate password
4. Click "Add secret"

---

## 3. Release Types

### 3.1 Version Naming Convention

SysDocs uses **Semantic Versioning** (semver):

```
v<major>.<minor>.<patch>[-<prerelease>]

Examples:
  v1.0.0         - Stable release
  v1.0.1         - Patch release
  v1.1.0         - Minor release
  v2.0.0         - Major release
  v1.0.0-alpha.1 - Alpha prerelease
  v1.0.0-beta.1  - Beta prerelease
  v1.0.0-rc.1    - Release candidate
```

### 3.2 Release Branch Strategy

| Branch | Purpose | Release Type |
|--------|---------|--------------|
| `alpha` | Development/testing | Alpha prereleases (v1.0.0-alpha.x) |
| `beta` | Integration testing | Beta prereleases (v1.0.0-beta.x) |
| `main` | Production-ready | Stable releases (v1.0.0) |
| `release/*` | Release preparation | Release candidates (v1.0.0-rc.x) |

---

## 4. Creating a Release

### 4.1 Standard Release (Recommended)

**Use this method for most releases**:

```bash
# 1. Ensure you're on the correct branch
git checkout main
git pull origin main

# 2. Create and push version tag
git tag -a v1.0.0 -m "Release v1.0.0 - Initial stable release"
git push origin v1.0.0

# 3. GitHub Actions automatically builds and releases
# Monitor at: https://github.com/CodeOOf/SysDocs/actions
```

**That's it!** The release pipeline handles everything else.

### 4.2 Prerelease (Alpha/Beta)

For testing before stable release:

```bash
# Alpha release (from alpha branch)
git checkout alpha
git pull origin alpha
git tag -a v1.0.0-alpha.1 -m "Release v1.0.0-alpha.1"
git push origin v1.0.0-alpha.1

# Beta release (from beta branch)
git checkout beta
git pull origin beta
git tag -a v1.0.0-beta.1 -m "Release v1.0.0-beta.1"
git push origin v1.0.0-beta.1
```

**Prereleases**:
- ✅ Are marked as "Pre-release" on GitHub
- ✅ Are signed and tested
- ❌ Are NOT published to NuGet.org automatically

### 4.3 Hotfix Release

For urgent fixes to production:

```bash
# Create hotfix branch from main
git checkout main
git pull origin main
git checkout -b hotfix/v1.0.1

# Make your fixes
git add .
git commit -m "Fix critical bug"

# Merge back to main
git checkout main
git merge hotfix/v1.0.1

# Create release tag
git tag -a v1.0.1 -m "Hotfix v1.0.1 - Fix critical bug"
git push origin v1.0.1
git push origin main

# Clean up
git branch -d hotfix/v1.0.1
```

---

## 5. Release Pipeline Details

### 5.1 What Happens When You Push a Tag

The `.github/workflows/release.yml` workflow triggers automatically:

#### Stage 1: Build Windows (15-20 minutes)
```
1. Checkout code
2. Setup .NET 10
3. Restore dependencies
4. Build solution
5. Run all tests
6. Import code signing certificate 🔐
7. Sign all .dll assemblies ✍️
8. Sign all .exe executables ✍️
9. Verify signatures ✅
10. Publish for Windows x64
11. Create sysdocs-windows-x64.zip
12. Upload artifact
```

#### Stage 2: Build Linux (10-15 minutes)
```
1. Checkout code
2. Setup .NET 10
3. Build and test
4. Publish for Linux x64
5. Create sysdocs-linux-x64.tar.gz
6. Upload artifact
```

#### Stage 3: Build macOS (10-15 minutes)
```
1. Checkout code
2. Setup .NET 10
3. Build and test
4. Publish for macOS x64
5. Create sysdocs-macos-x64.tar.gz
6. Upload artifact
```

#### Stage 4: Create NuGet Packages (5-10 minutes)
```
1. Pack SysDocs.Core
2. Pack SysDocs.Templates
3. Sign packages with certificate 🔐
4. Verify package signatures ✅
5. Upload artifacts
```

#### Stage 5: Create GitHub Release (2-5 minutes)
```
1. Download all artifacts
2. Generate release notes
3. Create GitHub Release
4. Upload all archives and packages
```

#### Stage 6: Publish to NuGet.org (2-5 minutes, stable only)
```
1. Download packages
2. Push to NuGet.org
3. Verify publication
```

#### Stage 7: Docker Build (10-15 minutes, optional)
```
1. Build Docker image
2. Push to Docker Hub
3. Tag with version and latest
```

**Total Time**: ~40-60 minutes for complete release

### 5.2 Monitoring Release Progress

**View real-time progress**:
```
https://github.com/CodeOOf/SysDocs/actions
```

Each job shows:
- ✅ Success (green checkmark)
- ❌ Failure (red X)
- 🟡 In progress (yellow circle)
- ⏸️ Skipped (gray dash)

**Check specific logs**:
1. Click on the workflow run
2. Click on a job (e.g., "Build & Sign Windows")
3. Expand steps to see detailed logs
4. Look for signing output: "All assemblies signed successfully"

### 5.3 Release Artifacts

After successful completion, your GitHub Release includes:

```
📦 sysdocs-windows-x64.zip       (signed executables)
📦 sysdocs-linux-x64.tar.gz
📦 sysdocs-macos-x64.tar.gz
📦 SysDocs.Core.1.0.0.nupkg      (signed)
📦 SysDocs.Templates.1.0.0.nupkg (signed)
```

Users can download directly from:
```
https://github.com/CodeOOf/SysDocs/releases/latest
```

---

## 6. Verifying Releases

### 6.1 Before Public Announcement

**Checklist**:

```bash
# 1. Download Windows release
curl -LO https://github.com/CodeOOf/SysDocs/releases/download/v1.0.0/sysdocs-windows-x64.zip

# 2. Extract
Expand-Archive sysdocs-windows-x64.zip -DestinationPath test-release

# 3. Verify signature (Windows)
Get-AuthenticodeSignature test-release/SysDocs.Cli.exe

# Expected output:
#   Status: Valid
#   SignerCertificate: CN=Your Cert Name
#   TimeStamperCertificate: CN=DigiCert...

# 4. Test execution
cd test-release
.\SysDocs.Cli.exe --version

# 5. Clean up
cd ..
Remove-Item -Recurse test-release
```

### 6.2 NuGet Package Verification

```bash
# Verify package signature
nuget verify -Signatures SysDocs.Core.1.0.0.nupkg

# Expected: "Successfully verified package 'SysDocs.Core.1.0.0'"

# Test installation
dotnet new console -n TestProject
cd TestProject
dotnet add package SysDocs.Core --version 1.0.0
dotnet build
```

---

## 7. Troubleshooting

### 7.1 Common Issues

#### Issue: "SignTool not found"

**Cause**: Windows SDK not installed on runner

**Solution**: The workflow uses `windows-latest` which includes SignTool. If testing locally:
```powershell
# Install Windows SDK
choco install windows-sdk-10.1
```

#### Issue: "Certificate password is incorrect"

**Cause**: Wrong password in `CODE_SIGNING_PASSWORD` secret

**Solution**:
1. Verify password locally:
   ```powershell
   $cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
   $cert.Import("cert.pfx", "your-password", "DefaultKeySet")
   ```
2. Update GitHub Secret with correct password

#### Issue: "Timestamp server unavailable"

**Cause**: DigiCert timestamp server is down

**Solution**: The workflow will retry automatically. If persistent, update workflow to use alternate timestamper:
```yaml
# Change from:
/tr http://timestamp.digicert.com

# To:
/tr http://timestamp.sectigo.com
```

#### Issue: "NuGet push failed: Package already exists"

**Cause**: Version already published to NuGet.org

**Solution**: Bump version number and create new tag:
```bash
git tag -a v1.0.1 -m "Release v1.0.1"
git push origin v1.0.1
```

### 7.2 Failed Release Recovery

If a release fails partway through:

```bash
# 1. Delete the failed release and tag on GitHub
gh release delete v1.0.0 --yes
git push origin :refs/tags/v1.0.0

# 2. Fix the issue (update code, secrets, etc.)
git add .
git commit -m "Fix release issue"
git push origin main

# 3. Recreate the tag
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0
```

---

## 8. Manual Release (Emergency)

If GitHub Actions is unavailable, perform manual release:

```bash
# 1. Build all platforms locally
make clean
make build

# 2. Sign Windows binaries (Windows only)
make sign

# 3. Create release archives
make publish

# 4. Create GitHub release using gh CLI
gh release create v1.0.0 \
  --title "SysDocs v1.0.0" \
  --notes-file RELEASE_NOTES.md \
  publish/sysdocs-windows-x64.zip \
  publish/sysdocs-linux-x64.tar.gz \
  publish/sysdocs-macos-x64.tar.gz

# 5. Publish NuGet packages manually
dotnet nuget push publish/SysDocs.Core.1.0.0.nupkg \
  --api-key $NUGET_API_KEY \
  --source https://api.nuget.org/v3/index.json
```

---

## 9. Release Checklist

### Pre-Release

- [ ] All tests passing on target branch
- [ ] Code signing certificate valid and not expiring soon
- [ ] Version number follows semver convention
- [ ] CHANGELOG.md updated with changes
- [ ] Documentation updated if needed
- [ ] GitHub Secrets configured correctly

### During Release

- [ ] Tag created with correct version
- [ ] GitHub Actions workflow triggered
- [ ] All jobs completed successfully
- [ ] Signatures verified on Windows artifacts
- [ ] NuGet packages signed and verified

### Post-Release

- [ ] Downloaded and tested Windows release
- [ ] Verified code signature on downloaded executable
- [ ] Tested NuGet package installation
- [ ] Updated project README with new version
- [ ] Announced release (if applicable)
- [ ] Closed related issues/PRs

---

## 10. Best Practices

### 10.1 Version Management

**DO**:
- ✅ Use annotated tags (`git tag -a`)
- ✅ Include meaningful release messages
- ✅ Follow semantic versioning strictly
- ✅ Tag from stable commits only

**DON'T**:
- ❌ Reuse version numbers
- ❌ Tag uncommitted changes
- ❌ Skip version numbers
- ❌ Use lightweight tags for releases

### 10.2 Certificate Management

**DO**:
- ✅ Rotate certificates before expiration
- ✅ Use timestamping (survives cert expiration)
- ✅ Store certificates in Azure Key Vault (production)
- ✅ Monitor certificate expiration (30+ days notice)

**DON'T**:
- ❌ Commit certificates to git
- ❌ Share certificate passwords
- ❌ Use expired certificates
- ❌ Skip signature verification

### 10.3 Release Cadence

Recommended release schedule:

| Release Type | Frequency | Example |
|-------------|-----------|---------|
| **Alpha** | Weekly | Every Friday from alpha branch |
| **Beta** | Bi-weekly | Every other Monday from beta branch |
| **Stable** | Monthly | First Tuesday of month from main |
| **Hotfix** | As needed | Immediately when critical |

---

## 11. References

- **[CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)** - Code signing implementation details
- **[REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)** - NFR-07 verification status
- **[.github/workflows/release.yml](../.github/workflows/release.yml)** - Complete release pipeline
- **GitHub Actions Docs**: https://docs.github.com/en/actions
- **Semantic Versioning**: https://semver.org/
- **gh CLI**: https://cli.github.com/

---

**📖 Navigation**: [⬅️ Back: Code Signing Guide](CODE_SIGNING_GUIDE.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)
