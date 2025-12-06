# Developer Setup - Code Signing

**📖 Navigation**: [⬅️ Back: CODE_SIGNING_GUIDE](CODE_SIGNING_GUIDE.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> **Quick Start Guide**: Step-by-step instructions for setting up GPG and Cosign on Windows and Linux

## Overview

This guide walks you through setting up your development environment for code signing on both Windows and Linux. All developers must configure GPG for commit signing before contributing to protected branches.

---

## Part 1: GPG Setup (Required for All Developers)

### Option A: Linux Native Environment

#### Step 1: Install GPG

```bash
# Ubuntu/Debian
sudo apt-get update
sudo apt-get install gnupg pinentry-curses

# Fedora/RHEL
sudo dnf install gnupg2 pinentry

# Arch Linux
sudo pacman -S gnupg
```

#### Step 2: Configure GPG Agent

```bash
# Create GPG config directory if it doesn't exist
mkdir -p ~/.gnupg
chmod 700 ~/.gnupg

# Configure GPG agent for better UX
cat > ~/.gnupg/gpg-agent.conf <<EOF
default-cache-ttl 3600
max-cache-ttl 7200
pinentry-program /usr/bin/pinentry-curses
EOF

# Restart GPG agent
gpgconf --kill gpg-agent
gpg-agent --daemon
```

#### Step 3: Generate GPG Key

```bash
# Generate 4096-bit RSA key (recommended)
gpg --full-generate-key

# Interactive prompts:
# 1. Please select what kind of key you want:
#    → (1) RSA and RSA (default)
# 
# 2. What keysize do you want?
#    → 4096
#
# 3. Key is valid for?
#    → 2y (expires in 2 years - good security practice)
#
# 4. Real name:
#    → Your Full Name
#
# 5. Email address:
#    → your-email@example.com (MUST match GitHub email!)
#
# 6. Comment (optional):
#    → SysDocs Developer
#
# 7. Passphrase:
#    → Enter a strong password (you'll need this for signing)
```

#### Step 4: Export and Configure

```bash
# List your keys to get the Key ID
gpg --list-secret-keys --keyid-format=long

# Example output:
# sec   rsa4096/3AA5C34371567BD2 2025-12-06 [SC] [expires: 2027-12-06]
#       ABCD1234EFGH5678IJKL9012MNOP3456QRST7890
# uid                 [ultimate] Your Name <your-email@example.com>
# ssb   rsa4096/4BB6D45482678CE3 2025-12-06 [E] [expires: 2027-12-06]
#
# Your Key ID is: 3AA5C34371567BD2 (the part after "rsa4096/")

# Export public key
gpg --armor --export YOUR_KEY_ID > ~/gpg-public-key.asc

# Configure Git to use your key
git config --global user.signingkey YOUR_KEY_ID
git config --global commit.gpgsign true
git config --global tag.gpgsign true
git config --global gpg.program gpg

# Display your public key for GitHub
cat ~/gpg-public-key.asc
```

---

### Option B: Windows with WSL2 (Recommended for Serious Development)

#### Step 1: Install WSL2

```powershell
# Run in PowerShell as Administrator
wsl --install

# Restart computer when prompted

# After restart, set up Ubuntu user account when prompted
```

#### Step 2: Update WSL2 Ubuntu

```bash
# From WSL2 terminal
sudo apt-get update
sudo apt-get upgrade -y
sudo apt-get install gnupg pinentry-curses git -y
```

#### Step 3: Generate GPG Key (Same as Linux)

Follow **Option A, Steps 2-4** above in your WSL2 terminal.

#### Step 4: Share GPG Keys Between WSL2 and Windows (Optional)

```bash
# Export your GPG keys from WSL2
gpg --export-secret-keys > /mnt/c/Users/YourUsername/gpg-backup.key
gpg --export-ownertrust > /mnt/c/Users/YourUsername/gpg-ownertrust.txt

# From Windows Git Bash (if you want to use GPG in native Windows):
gpg --import /c/Users/YourUsername/gpg-backup.key
gpg --import-ownertrust /c/Users/YourUsername/gpg-ownertrust.txt

# Configure Git on Windows
git config --global user.signingkey YOUR_KEY_ID
git config --global commit.gpgsign true
git config --global gpg.program "C:/Program Files/Git/usr/bin/gpg.exe"

# Clean up backup files (IMPORTANT for security!)
rm /c/Users/YourUsername/gpg-backup.key
rm /c/Users/YourUsername/gpg-ownertrust.txt
```

---

### Option C: Windows Native (Git Bash)

Git for Windows includes GPG, but WSL2 is recommended for serious development.

#### Step 1: Verify GPG Installation

```bash
# Open Git Bash
gpg --version

# If not found, reinstall Git for Windows with GPG support
```

#### Step 2: Generate GPG Key

```bash
# In Git Bash
gpg --full-generate-key

# Follow the same prompts as Option A, Step 3
```

#### Step 3: Configure Git

```bash
# Set your GPG key
git config --global user.signingkey YOUR_KEY_ID
git config --global commit.gpgsign true
git config --global tag.gpgsign true
git config --global gpg.program "C:/Program Files/Git/usr/bin/gpg.exe"
```

---

## Part 2: Add GPG Key to GitHub (Required)

### Step 1: Copy Public Key

```bash
# Linux/WSL2
cat ~/gpg-public-key.asc | xclip -selection clipboard
# If xclip not installed: sudo apt-get install xclip

# macOS
cat ~/gpg-public-key.asc | pbcopy

# Windows Git Bash
cat ~/gpg-public-key.asc | clip.exe

# Or just display and copy manually
cat ~/gpg-public-key.asc
```

### Step 2: Add to GitHub

1. Navigate to: https://github.com/settings/keys
2. Click **"New GPG key"**
3. Paste your public key (entire contents including `-----BEGIN PGP PUBLIC KEY BLOCK-----`)
4. Click **"Add GPG key"**

### Step 3: Verify

```bash
# Make a test commit
git commit --allow-empty -m "Test GPG signature"

# Check signature locally
git log --show-signature -1

# Push to GitHub and verify "Verified" badge appears
```

---

## Part 3: Cosign Setup (For Release Managers Only)

Cosign is used to sign Docker images. Only needed if you'll be creating releases.

### Linux Installation

```bash
# Download latest Cosign release
wget https://github.com/sigstore/cosign/releases/latest/download/cosign-linux-amd64

# Make executable
chmod +x cosign-linux-amd64

# Move to system path
sudo mv cosign-linux-amd64 /usr/local/bin/cosign

# Verify installation
cosign version
```

### Windows (WSL2) Installation

```bash
# Same as Linux, run in WSL2 terminal
wget https://github.com/sigstore/cosign/releases/latest/download/cosign-linux-amd64
chmod +x cosign-linux-amd64
sudo mv cosign-linux-amd64 /usr/local/bin/cosign
cosign version
```

### macOS Installation

```bash
# Using Homebrew
brew install cosign

# Or download binary
wget https://github.com/sigstore/cosign/releases/latest/download/cosign-darwin-amd64
chmod +x cosign-darwin-amd64
sudo mv cosign-darwin-amd64 /usr/local/bin/cosign
```

### Generate Cosign Keys (Team Lead Only)

```bash
# Generate keypair for signing Docker images
cosign generate-key-pair

# Enter passphrase when prompted
# This creates:
# - cosign.key (private key - DO NOT COMMIT!)
# - cosign.pub (public key - distribute to users)

# Add cosign.key to .gitignore (already done in SysDocs)
echo "cosign.key" >> .gitignore
```

---

## Part 4: Testing Your Setup

### Test GPG Commit Signing

```bash
# Clone SysDocs repository
git clone https://github.com/CodeOOf/SysDocs.git
cd SysDocs

# Create test branch
git checkout -b test/gpg-signing

# Make a test commit
git commit --allow-empty -m "test: Verify GPG signing setup"

# Verify signature locally
git log --show-signature -1

# Expected output:
# gpg: Signature made [date]
# gpg: using RSA key [your key ID]
# gpg: Good signature from "Your Name <your-email@example.com>" [ultimate]

# Push to verify GitHub shows "Verified" badge
git push origin test/gpg-signing
```

### Test Cosign (Release Managers Only)

```bash
# Build a test Docker image
docker build -t sysdocs-test:latest .

# Sign the image
cosign sign --key cosign.key sysdocs-test:latest

# Verify signature
cosign verify --key cosign.pub sysdocs-test:latest
```

---

## Part 5: Troubleshooting

### GPG "No secret key" Error

```bash
# List your keys
gpg --list-secret-keys --keyid-format=long

# If no keys shown, you need to generate one (see Part 1)

# If keys exist but Git can't find them:
git config --global gpg.program $(which gpg)
```

### GPG Agent Not Running

```bash
# Start GPG agent
gpg-agent --daemon

# Or restart it
gpgconf --kill gpg-agent
gpg-agent --daemon

# Test
echo "test" | gpg --clearsign
```

### "Failed to sign the data" Error

```bash
# Check GPG can sign
echo "test" | gpg --clearsign

# If this fails, check your passphrase
gpg --edit-key YOUR_KEY_ID
# Type: passwd
# Enter old passphrase, then new passphrase
# Type: save

# Update Git config
git config --global gpg.program $(which gpg)
```

### WSL2 GPG Passphrase Prompt Issues

```bash
# Use pinentry-curses for terminal passphrase entry
echo "pinentry-program /usr/bin/pinentry-curses" >> ~/.gnupg/gpg-agent.conf

# Restart GPG agent
gpgconf --kill gpg-agent
gpg-agent --daemon

# Set GPG_TTY environment variable (add to ~/.bashrc)
echo 'export GPG_TTY=$(tty)' >> ~/.bashrc
source ~/.bashrc
```

### Cosign "No signatures found" Error

```bash
# Verify you're checking the correct image
docker images

# Verify the image was pushed to registry
# (Local images can't be signed with Cosign)

# Sign after pushing:
docker push myregistry/sysdocs:1.0.0
cosign sign --key cosign.key myregistry/sysdocs:1.0.0
```

---

## Part 6: Daily Workflow

### Making Commits

```bash
# Your commits are automatically signed (commit.gpgsign=true)
git add .
git commit -m "feat: Add new feature"

# Push normally
git push origin feature-branch
```

### Creating Tags

```bash
# Tags are automatically signed (tag.gpgsign=true)
git tag v1.0.0-alpha.1 -m "Alpha release 1"

# Verify tag signature
git tag -v v1.0.0-alpha.1

# Push tag
git push origin v1.0.0-alpha.1
```

### Verifying Other's Commits

```bash
# Check if commit is signed
git log --show-signature

# Verify specific commit
git verify-commit abc123def456

# Check tag signature
git verify-tag v1.0.0
```

---

## Part 7: Security Best Practices

### DO:
- ✅ Use 4096-bit RSA keys
- ✅ Set key expiration (2 years recommended)
- ✅ Use strong passphrase
- ✅ Backup your keys securely
- ✅ Generate revocation certificate
- ✅ Rotate keys before expiration
- ✅ Keep private keys on encrypted storage

### DON'T:
- ❌ Share your private key
- ❌ Commit private keys to Git
- ❌ Use weak passphrases
- ❌ Skip key expiration
- ❌ Store keys in cloud sync folders (Dropbox, OneDrive)
- ❌ Use same key for multiple purposes

### Backup Your Keys

```bash
# Export private key (KEEP SECURE!)
gpg --export-secret-keys YOUR_KEY_ID > gpg-private-backup.key

# Export public key
gpg --export YOUR_KEY_ID > gpg-public-backup.key

# Export trust database
gpg --export-ownertrust > gpg-ownertrust-backup.txt

# Store these files on encrypted USB drive or secure password manager
# DO NOT store in Git or unencrypted cloud storage!
```

### Generate Revocation Certificate

```bash
# Create revocation certificate (in case key is compromised)
gpg --output revoke.asc --gen-revoke YOUR_KEY_ID

# Store this file securely
# If your key is compromised, import and publish this certificate
```

---

## Part 8: Team Checklist

Before your first commit to protected branches:

- [ ] GPG installed and working
- [ ] 4096-bit RSA key generated with 2-year expiration
- [ ] Git configured for automatic signing
- [ ] Public key added to GitHub account
- [ ] Test commit shows "Verified" badge on GitHub
- [ ] Private key backed up securely
- [ ] Revocation certificate generated and stored

For release managers (additional):

- [ ] Cosign installed
- [ ] Cosign keypair generated (team lead provides)
- [ ] Docker access configured
- [ ] Test image signed and verified successfully

---

## Part 9: Quick Reference

### Common Commands

```bash
# List your GPG keys
gpg --list-secret-keys --keyid-format=long

# Sign a commit explicitly
git commit -S -m "message"

# Verify last commit
git log --show-signature -1

# Sign a file
gpg --armor --detach-sign file.tar.gz

# Verify a signature
gpg --verify file.tar.gz.asc

# Sign Docker image
cosign sign --key cosign.key image:tag

# Verify Docker signature
cosign verify --key cosign.pub image:tag
```

### Configuration Files

**~/.gitconfig**:
```ini
[user]
    name = Your Name
    email = your-email@example.com
    signingkey = YOUR_KEY_ID

[commit]
    gpgsign = true

[tag]
    gpgsign = true

[gpg]
    program = gpg
```

**~/.gnupg/gpg-agent.conf**:
```ini
default-cache-ttl 3600
max-cache-ttl 7200
pinentry-program /usr/bin/pinentry-curses
```

**~/.bashrc** (add these lines):
```bash
export GPG_TTY=$(tty)
```

---

## Part 10: Getting Help

### Resources

- **GPG Documentation**: https://gnupg.org/documentation/
- **GitHub GPG Guide**: https://docs.github.com/en/authentication/managing-commit-signature-verification
- **Cosign Documentation**: https://docs.sigstore.dev/cosign/overview/
- **SysDocs CODE_SIGNING_GUIDE**: [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md)

### Contact

If you encounter issues:
1. Check the **Troubleshooting** section (Part 5)
2. Search existing GitHub issues
3. Ask in the team chat
4. Create a GitHub issue with `[signing-setup]` prefix

---

**Next Steps**: After completing this setup, return to [CODE_SIGNING_GUIDE.md](CODE_SIGNING_GUIDE.md) for release signing procedures and CI/CD integration details.
