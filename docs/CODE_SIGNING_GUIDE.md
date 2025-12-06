# Code Signing Guide - Open Source Edition

**📖 Navigation**: [⬅️ Back: Docs Index](README.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> **Requirement Traceability**: This guide implements **NFR-07** - GPG Commit and Artifact Signing

## 1. Overview

SysDocs follows open-source best practices for code signing using **GPG (GNU Privacy Guard)** to ensure authenticity and integrity. This approach is appropriate for open-source projects and works consistently across all platforms.

### Why GPG Signing?

- ✅ **Cross-platform**: Works on Linux, macOS, Windows (WSL2)
- ✅ **Open-source standard**: Used by kernel.org, Debian, Python, etc.
- ✅ **Free**: No certificate purchases required
- ✅ **GitHub integrated**: Native "Verified" badges
- ✅ **Community trust**: Web of trust model

### What Gets Signed

| Artifact | Signing Method | Platform | Verification |
|----------|---------------|----------|--------------|
| Git commits | GPG signature | All | `git log --show-signature` |
| Release tarballs | GPG detached signature (.asc) | All | `gpg --verify` |
| Docker images | Cosign signature | All | `cosign verify` |
| NuGet packages | NuGet signing | All | `dotnet nuget verify` |
| Checksums (SHA256SUMS) | GPG signature | All | `gpg --verify` |

---

## 2. GPG Setup for Developers

### 2.1 Install GPG

#### Linux (Ubuntu/Debian)
```bash
sudo apt-get update
sudo apt-get install gnupg
```

#### macOS
```bash
brew install gnupg
```

#### Windows (via WSL2)
```bash
# From WSL2 terminal
sudo apt-get install gnupg

# Or native Windows (Git Bash includes GPG)
gpg --version
```

### 2.2 Generate GPG Key

```bash
# Generate a new GPG key (4096-bit RSA recommended)
gpg --full-generate-key

# Prompts:
# 1. Kind of key: (1) RSA and RSA
# 2. Key size: 4096
# 3. Expiration: 2y (2 years, good security practice)
# 4. Real name: Your Name
# 5. Email: your-email@example.com (must match GitHub email)
# 6. Passphrase: Strong password to protect your private key
```

**Important**: Use the **same email** as your GitHub account!

### 2.3 List Your Keys

```bash
# List your GPG keys
gpg --list-secret-keys --keyid-format=long

# Output example:
# sec   rsa4096/3AA5C34371567BD2 2024-12-06 [SC] [expires: 2026-12-06]
#       ABCD1234EFGH5678IJKL9012MNOP3456QRST7890
# uid                 [ultimate] Your Name <your-email@example.com>
# ssb   rsa4096/4BB6D45482678CE3 2024-12-06 [E] [expires: 2026-12-06]

# Your Key ID is: 3AA5C34371567BD2 (after "rsa4096/")
```

### 2.4 Export Public Key

```bash
# Export your public key for sharing
gpg --armor --export 3AA5C34371567BD2 > gpg-public-key.asc

# Display public key (for copy/paste)
cat gpg-public-key.asc
```

### 2.5 Configure Git to Sign Commits

```bash
# Set your GPG key for Git
git config --global user.signingkey 3AA5C34371567BD2

# Enable automatic commit signing
git config --global commit.gpgsign true

# Enable automatic tag signing
git config --global tag.gpgsign true

# Configure GPG program (if needed)
git config --global gpg.program gpg
```

### 2.6 Add GPG Key to GitHub

1. **Copy your public key**:
   ```bash
   cat gpg-public-key.asc | xclip -selection clipboard
   # Or on macOS:
   cat gpg-public-key.asc | pbcopy
   ```

2. **Navigate to GitHub**:
   ```
   https://github.com/settings/keys
   ```

3. **Add GPG key**:
   - Click "New GPG key"
   - Paste your public key
   - Click "Add GPG key"

4. **Verify**:
   - Your commits will now show "Verified" badge on GitHub

---

## 3. Signing Commits

### 3.1 Sign Individual Commits

```bash
# Automatically signed (if commit.gpgsign=true)
git commit -m "Add new feature"

# Explicitly sign a commit
git commit -S -m "Add new feature"

# Verify your commit is signed
git log --show-signature -1
```

### 3.2 Sign Tags

```bash
# Create signed annotated tag
git tag -s v1.0.0 -m "Release v1.0.0"

# Verify tag signature
git tag -v v1.0.0
```

### 3.3 Verify Commit Signatures

```bash
# Show signature for last commit
git log --show-signature -1

# Show signatures for all commits
git log --show-signature

# Verify specific commit
git verify-commit HEAD
```

---

## 4. Release Artifact Signing

### 4.1 Sign Release Tarballs

```bash
# Create release tarball
tar -czf sysdocs-1.0.0-linux-x64.tar.gz sysdocs/

# Sign tarball (creates .asc detached signature)
gpg --armor --detach-sign sysdocs-1.0.0-linux-x64.tar.gz

# This creates: sysdocs-1.0.0-linux-x64.tar.gz.asc

# Verify signature
gpg --verify sysdocs-1.0.0-linux-x64.tar.gz.asc sysdocs-1.0.0-linux-x64.tar.gz
```

### 4.2 Sign Checksums File

```bash
# Generate checksums for all release files
sha256sum sysdocs-1.0.0-*.tar.gz > SHA256SUMS

# Sign the checksums file
gpg --clearsign SHA256SUMS

# This creates: SHA256SUMS.asc (signed checksums)

# Verify checksums signature
gpg --verify SHA256SUMS.asc

# Verify file integrity
sha256sum -c SHA256SUMS
```

### 4.3 Sign Docker Images with Cosign

Cosign is the modern standard for signing container images (part of Sigstore project).

#### Install Cosign

```bash
# Linux
wget https://github.com/sigstore/cosign/releases/latest/download/cosign-linux-amd64
chmod +x cosign-linux-amd64
sudo mv cosign-linux-amd64 /usr/local/bin/cosign

# macOS
brew install cosign

# Verify installation
cosign version
```

#### Generate Cosign Keypair

```bash
# Generate cosign key pair
cosign generate-key-pair

# This creates:
# - cosign.key (private key - keep secure!)
# - cosign.pub (public key - distribute to users)
```

#### Sign Docker Image

```bash
# Build Docker image
docker build -t codeof/sysdocs:1.0.0 .

# Push to registry
docker push codeof/sysdocs:1.0.0

# Sign the image
cosign sign --key cosign.key codeof/sysdocs:1.0.0

# Verify signature
cosign verify --key cosign.pub codeof/sysdocs:1.0.0
```

### 4.4 Sign NuGet Packages (Optional)

```bash
# Create NuGet package
dotnet pack --configuration Release

# Sign with certificate (if you have one)
dotnet nuget sign SysDocs.Core.1.0.0.nupkg \
  --certificate-path cert.pfx \
  --certificate-password "password" \
  --timestamper http://timestamp.digicert.com

# Verify package signature
dotnet nuget verify SysDocs.Core.1.0.0.nupkg
```

**Note**: NuGet signing requires a code signing certificate. For open-source projects, this is optional but recommended for NuGet.org publishing.

---

## 5. Automated Signing Scripts

### 5.1 Sign Release Script

Create `scripts/sign-release.sh`:

```bash
#!/bin/bash
# Sign all release artifacts with GPG

set -e

VERSION="${1:-unknown}"
ARTIFACTS_DIR="publish"

echo "🔐 Signing SysDocs v${VERSION} release artifacts..."

# Sign all tarballs
for tarball in ${ARTIFACTS_DIR}/*.tar.gz; do
    if [ -f "$tarball" ]; then
        echo "  📦 Signing: $(basename $tarball)"
        gpg --armor --detach-sign "$tarball"
    fi
done

# Generate and sign checksums
cd ${ARTIFACTS_DIR}
sha256sum *.tar.gz *.zip 2>/dev/null > SHA256SUMS || true
if [ -f "SHA256SUMS" ]; then
    echo "  ✍️  Signing checksums..."
    gpg --clearsign SHA256SUMS
    mv SHA256SUMS.asc SHA256SUMS.gpg
fi
cd ..

echo "✅ All artifacts signed successfully!"
echo ""
echo "📋 Verification commands:"
echo "  gpg --verify publish/sysdocs-linux-x64.tar.gz.asc"
echo "  gpg --verify publish/SHA256SUMS.gpg"
```

### 5.2 Verify Signatures Script

Create `scripts/verify-signatures.sh`:

```bash
#!/bin/bash
# Verify GPG signatures on all release artifacts

set -e

ARTIFACTS_DIR="publish"
FAILED=0

echo "🔍 Verifying GPG signatures..."

# Verify all .asc signature files
for asc_file in ${ARTIFACTS_DIR}/*.asc; do
    if [ -f "$asc_file" ]; then
        artifact="${asc_file%.asc}"
        echo "  Verifying: $(basename $artifact)"
        if gpg --verify "$asc_file" "$artifact" 2>&1 | grep -q "Good signature"; then
            echo "    ✅ Valid signature"
        else
            echo "    ❌ Invalid signature"
            FAILED=1
        fi
    fi
done

# Verify checksums signature
if [ -f "${ARTIFACTS_DIR}/SHA256SUMS.gpg" ]; then
    echo "  Verifying: SHA256SUMS"
    if gpg --verify "${ARTIFACTS_DIR}/SHA256SUMS.gpg" 2>&1 | grep -q "Good signature"; then
        echo "    ✅ Valid signature"
    else
        echo "    ❌ Invalid signature"
        FAILED=1
    fi
fi

if [ $FAILED -eq 0 ]; then
    echo ""
    echo "✅ All signatures verified successfully!"
    exit 0
else
    echo ""
    echo "❌ Some signatures failed verification!"
    exit 1
fi
```

---

## 6. GitHub Branch Protection

### 6.1 Require Signed Commits

Enforce signed commits on protected branches:

1. **Navigate to**: `Settings → Branches → Branch protection rules`

2. **Add rule for `main` branch**:
   - Branch name pattern: `main`
   - ✅ Require signed commits
   - ✅ Require a pull request before merging
   - ✅ Require status checks to pass
   - ✅ Include administrators

3. **Result**: All commits to `main` must be GPG-signed

### 6.2 GitHub Actions GPG Signing

Configure GitHub Actions to sign commits:

```yaml
- name: Import GPG key
  uses: crazy-max/ghaction-import-gpg@v6
  with:
    gpg_private_key: ${{ secrets.GPG_PRIVATE_KEY }}
    passphrase: ${{ secrets.GPG_PASSPHRASE }}
    git_user_signingkey: true
    git_commit_gpgsign: true

- name: Commit changes (automatically signed)
  run: |
    git add .
    git commit -m "Automated update"
    git push
```

**Required Secrets**:
- `GPG_PRIVATE_KEY`: Your private key (`gpg --armor --export-secret-keys YOUR_KEY_ID`)
- `GPG_PASSPHRASE`: Your GPG key passphrase

---

## 7. Windows Development (WSL2)

For serious development on Windows, use **WSL2** with Ubuntu:

### 7.1 Setup WSL2

```powershell
# Install WSL2 (PowerShell as Administrator)
wsl --install -d Ubuntu-22.04

# Enter WSL2
wsl
```

### 7.2 Configure GPG in WSL2

```bash
# Inside WSL2
sudo apt-get update
sudo apt-get install gnupg git

# Follow steps in Section 2 to generate GPG key

# Configure Git
git config --global user.name "Your Name"
git config --global user.email "your-email@example.com"
git config --global user.signingkey YOUR_KEY_ID
git config --global commit.gpgsign true
```

### 7.3 Share GPG Key Between Windows and WSL2

```bash
# Export from WSL2
gpg --export-secret-keys --armor YOUR_KEY_ID > /mnt/c/Users/YourName/gpg-key.asc

# Import in Windows (Git Bash)
gpg --import /c/Users/YourName/gpg-key.asc

# Clean up
rm /mnt/c/Users/YourName/gpg-key.asc
```

---

## 8. Best Practices

### 8.1 DO

- ✅ Use 4096-bit RSA keys
- ✅ Set key expiration (2 years recommended)
- ✅ Sign all commits on protected branches
- ✅ Sign all release tags
- ✅ Sign all release artifacts
- ✅ Backup private key securely
- ✅ Generate revocation certificate
- ✅ Use the same email as GitHub
- ✅ Add GPG key to GitHub
- ✅ Enable branch protection for signed commits

### 8.2 DON'T

- ❌ Share your private key
- ❌ Commit private keys to git
- ❌ Use keys without expiration
- ❌ Skip key backups
- ❌ Use weak passphrases
- ❌ Sign commits with wrong email
- ❌ Forget to push GPG key to GitHub
- ❌ Skip signature verification before merging

---

## 9. References

- **[GITHUB_RELEASE_PROCESS.md](GITHUB_RELEASE_PROCESS.md)** - Complete GitHub release workflow
- **[REQUIREMENTS.md](../project/REQUIREMENTS.md)** - NFR-07 requirement definition
- **[REQUIREMENTS_MATRIX.md](../project/REQUIREMENTS_MATRIX.md)** - NFR-07 V&V status
- **[TRACEABILITY.md](../project/TRACEABILITY.md)** - NFR-07 implementation mapping
- **GNU Privacy Guard**: https://gnupg.org/
- **GitHub GPG Documentation**: https://docs.github.com/en/authentication/managing-commit-signature-verification
- **Sigstore Cosign**: https://docs.sigstore.dev/cosign/overview/
- **Dev.to Guide**: https://dev.to/adityabhuyan/the-ultimate-guide-to-digitally-signing-code-for-open-source-software-on-github-and-bitbucket-2laa

---

**📖 Navigation**: [⬅️ Back: Docs Index](README.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)
