# Quick Release Reference Card

Quick reference for common release operations in SysDocs.

---

## Creating Releases

### Standard Stable Release
```bash
git checkout main && git pull
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0
# Done! GitHub Actions handles the rest
```

### Alpha/Beta Release
```bash
# Alpha
git checkout alpha && git pull
git tag -a v1.0.0-alpha.1 -m "Alpha release"
git push origin v1.0.0-alpha.1

# Beta
git checkout beta && git pull
git tag -a v1.0.0-beta.1 -m "Beta release"
git push origin v1.0.0-beta.1
```

### Hotfix Release
```bash
git checkout -b hotfix/v1.0.1
# Make fixes
git commit -am "Fix critical bug"
git checkout main && git merge hotfix/v1.0.1
git tag -a v1.0.1 -m "Hotfix v1.0.1"
git push origin v1.0.1 main
```

---

## GitHub Secrets Setup

### Convert Certificate to Base64
```powershell
$bytes = [System.IO.File]::ReadAllBytes("cert.pfx")
[Convert]::ToBase64String($bytes) | Out-File cert-base64.txt
Get-Content cert-base64.txt | Set-Clipboard
Remove-Item cert-base64.txt
```

### Add to GitHub
```
https://github.com/CodeOOf/SysDocs/settings/secrets/actions
→ New repository secret
→ NAME: CODE_SIGNING_CERT
→ VALUE: (paste base64)
```

---

## Local Signing

### Sign All Binaries
```bash
# Windows
make sign

# Or directly
pwsh -File scripts/sign-assemblies.ps1
```

### Verify Signatures
```bash
make verify-signatures

# Or check specific file
Get-AuthenticodeSignature path\to\file.exe
```

---

## Monitoring

### Watch Release Progress
```
https://github.com/CodeOOf/SysDocs/actions
```

### Download Latest Release
```
https://github.com/CodeOOf/SysDocs/releases/latest
```

---

## Troubleshooting

### Delete Failed Release
```bash
gh release delete v1.0.0 --yes
git push origin :refs/tags/v1.0.0
```

### Test Certificate Password
```powershell
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2
$cert.Import("cert.pfx", "password", "DefaultKeySet")
# Success = correct password
```

### View Detailed Logs
```
https://github.com/CodeOOf/SysDocs/actions
→ Click workflow run
→ Click job
→ Expand steps
```

---

## Required Secrets

| Secret | Purpose |
|--------|---------|
| `CODE_SIGNING_CERT` | Base64 certificate (Required) |
| `CODE_SIGNING_PASSWORD` | Certificate password (Required) |
| `NUGET_API_KEY` | Publish to NuGet.org (Optional) |
| `DOCKER_USERNAME` | Docker Hub username (Optional) |
| `DOCKER_PASSWORD` | Docker Hub token (Optional) |

---

## Version Naming

```
v<major>.<minor>.<patch>[-<prerelease>]

Examples:
✅ v1.0.0           Stable
✅ v1.0.0-alpha.1   Alpha
✅ v1.0.0-beta.2    Beta
✅ v1.0.0-rc.1      Release candidate
❌ v1.0.0.1         Wrong format
❌ 1.0.0            Missing 'v' prefix
```

---

## Full Documentation

- **Complete Guide**: [GITHUB_RELEASE_PROCESS.md](GITHUB_RELEASE_PROCESS.md)
- **Code Signing**: [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)
- **Requirements**: [REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)
