# GitHub Secrets Setup for Release Pipeline

**📖 Navigation**: [⬅️ Back: Release Process](GITHUB_RELEASE_PROCESS.md) | [🏠 Main README](../README.md) | [🗺️ Docs Navigation](DOCUMENTATION_NAVIGATION.md)

> **For Repository Maintainers**: Configure these secrets to enable automated signing in the release pipeline

## Overview

The SysDocs release pipeline requires several GitHub Secrets for automated code signing (GPG and Cosign). This guide explains how to generate and configure each secret.

---

## Required Secrets (GPG Signing)

### 1. GPG_PRIVATE_KEY (Required)

**Purpose**: Bot GPG key for signing release artifacts in GitHub Actions

**Generate**:

```bash
# Generate bot GPG key (on Linux or WSL2)
gpg --batch --gen-key <<EOF
Key-Type: RSA
Key-Length: 4096
Subkey-Type: RSA
Subkey-Length: 4096
Name-Real: SysDocs Release Bot
Name-Email: bot@sysdocs.dev
Expire-Date: 2y
Passphrase: YOUR_STRONG_PASSPHRASE_HERE
%commit
EOF

# List keys to get Key ID
gpg --list-secret-keys --keyid-format=long

# Export private key (ASCII armored)
gpg --armor --export-secret-keys YOUR_KEY_ID > gpg-bot-private.asc

# Display the key (copy this for GitHub)
cat gpg-bot-private.asc
```

**Add to GitHub**:

1. Go to: `https://github.com/CodeOOf/SysDocs/settings/secrets/actions`
2. Click **"New repository secret"**
3. Name: `GPG_PRIVATE_KEY`
4. Value: Paste the entire contents of `gpg-bot-private.asc` (including BEGIN/END lines)
5. Click **"Add secret"**

**Security**: Keep `gpg-bot-private.asc` in a secure location (encrypted backup). Never commit to Git!

---

### 2. GPG_PASSPHRASE (Required)

**Purpose**: Passphrase to unlock the bot GPG key during signing

**Value**: The passphrase you used when generating the GPG key above

**Add to GitHub**:

1. Go to: `https://github.com/CodeOOf/SysDocs/settings/secrets/actions`
2. Click **"New repository secret"**
3. Name: `GPG_PASSPHRASE`
4. Value: Your GPG key passphrase
5. Click **"Add secret"**

**Security**: Use a strong, unique passphrase (20+ characters recommended)

---

## Required Secrets (Docker Signing)

### 3. COSIGN_PRIVATE_KEY (Required for Docker releases)

**Purpose**: Cosign private key for signing Docker images

**Generate**:

```bash
# Install Cosign if not already installed
# See DEVELOPER_SETUP_SIGNING.md for installation instructions

# Generate Cosign keypair
cosign generate-key-pair

# Enter passphrase when prompted
# This creates:
# - cosign.key (private key)
# - cosign.pub (public key)

# Display private key (copy this for GitHub)
cat cosign.key
```

**Add to GitHub**:

1. Go to: `https://github.com/CodeOOf/SysDocs/settings/secrets/actions`
2. Click **"New repository secret"**
3. Name: `COSIGN_PRIVATE_KEY`
4. Value: Paste the entire contents of `cosign.key`
5. Click **"Add secret"**

**Publish Public Key**:

```bash
# Users need cosign.pub to verify signatures
# Commit this to the repository
cp cosign.pub docs/cosign.pub
git add docs/cosign.pub
git commit -m "docs: Add Cosign public key for Docker image verification"
git push
```

**Security**: Keep `cosign.key` secure. Only `cosign.pub` should be committed to Git.

---

### 4. COSIGN_PASSWORD (Required for Docker releases)

**Purpose**: Passphrase to unlock Cosign private key

**Value**: The passphrase you used when generating the Cosign keypair

**Add to GitHub**:

1. Go to: `https://github.com/CodeOOf/SysDocs/settings/secrets/actions`
2. Click **"New repository secret"**
3. Name: `COSIGN_PASSWORD`
4. Value: Your Cosign key passphrase
5. Click **"Add secret"**

---

## Optional Secrets (Docker Registry)

### 5. DOCKER_USERNAME (Optional)

**Purpose**: Docker Hub username for pushing images

**Value**: Your Docker Hub username

**Add to GitHub**: Same process as above, name: `DOCKER_USERNAME`

---

### 6. DOCKER_PASSWORD (Optional)

**Purpose**: Docker Hub access token for authentication

**Generate**:

1. Go to: https://hub.docker.com/settings/security
2. Click **"New Access Token"**
3. Description: "SysDocs GitHub Actions"
4. Access permissions: **Read, Write, Delete**
5. Click **"Generate"**
6. Copy the token (you won't see it again!)

**Add to GitHub**: Same process as above, name: `DOCKER_PASSWORD`

---

## Optional Secrets (NuGet Publishing)

### 7. NUGET_API_KEY (Optional)

**Purpose**: NuGet.org API key for publishing packages

**Generate**:

1. Go to: https://www.nuget.org/account/apikeys
2. Click **"Create"**
3. Key Name: "SysDocs GitHub Actions"
4. Expiration: 365 days (or custom)
5. Scopes: **Push** (select specific packages if available)
6. Click **"Create"**
7. Copy the API key

**Add to GitHub**: Same process as above, name: `NUGET_API_KEY`

---

## Summary of All Secrets

| Secret Name | Required? | Purpose | How to Generate |
|------------|-----------|---------|----------------|
| **GPG_PRIVATE_KEY** | ✅ Yes | Sign release artifacts | `gpg --gen-key` + `gpg --export-secret-keys` |
| **GPG_PASSPHRASE** | ✅ Yes | Unlock GPG key | Passphrase from GPG key generation |
| **COSIGN_PRIVATE_KEY** | ✅ Yes (Docker) | Sign Docker images | `cosign generate-key-pair` |
| **COSIGN_PASSWORD** | ✅ Yes (Docker) | Unlock Cosign key | Passphrase from Cosign generation |
| **DOCKER_USERNAME** | ⚠️ Optional | Docker Hub login | Your Docker Hub username |
| **DOCKER_PASSWORD** | ⚠️ Optional | Docker Hub auth | Docker Hub access token |
| **NUGET_API_KEY** | ⚠️ Optional | Publish to NuGet.org | NuGet.org API key |

---

## Verification Checklist

After configuring secrets, verify the setup:

### GPG Secrets

```bash
# Test local GPG signing (to ensure key works)
echo "test" | gpg --clearsign --armor

# If this works locally, the key should work in GitHub Actions
```

### Cosign Secrets

```bash
# Test local Cosign signing
docker pull alpine:latest
docker tag alpine:latest test-image:latest
cosign sign --key cosign.key test-image:latest

# Verify signature
cosign verify --key cosign.pub test-image:latest
```

### GitHub Actions Test

```bash
# Create a test release to verify secrets work
git tag v0.0.1-test
git push origin v0.0.1-test

# Monitor GitHub Actions: https://github.com/CodeOOf/SysDocs/actions
# Check for GPG signing steps and Cosign signing steps
```

---

## Rotating Keys

### When to Rotate

- **GPG Keys**: Every 2 years (before expiration)
- **Cosign Keys**: Every 2 years or if compromised
- **Docker Tokens**: Every year or if compromised
- **NuGet API Keys**: Every year or if compromised

### How to Rotate GPG Keys

```bash
# Generate new GPG key
gpg --full-generate-key

# Export new private key
gpg --armor --export-secret-keys NEW_KEY_ID > gpg-bot-private-new.asc

# Update GitHub Secret (replace GPG_PRIVATE_KEY with new key)

# Update GPG_PASSPHRASE if changed

# Revoke old key after verification
gpg --gen-revoke OLD_KEY_ID > revoke-old-key.asc
gpg --import revoke-old-key.asc
gpg --send-keys OLD_KEY_ID
```

### How to Rotate Cosign Keys

```bash
# Generate new Cosign keypair
cosign generate-key-pair -o cosign-new

# Update GitHub Secret (replace COSIGN_PRIVATE_KEY)
# Update COSIGN_PASSWORD if changed

# Publish new public key
cp cosign-new.pub docs/cosign.pub
git add docs/cosign.pub
git commit -m "security: Rotate Cosign public key"
git push

# Archive old key securely (don't delete immediately)
```

---

## Security Best Practices

### DO:
- ✅ Use strong passphrases (20+ characters, mix of types)
- ✅ Rotate keys every 2 years
- ✅ Keep private keys in encrypted backups
- ✅ Generate dedicated bot keys (don't reuse personal keys)
- ✅ Review GitHub Actions logs for signing success
- ✅ Set key expiration dates

### DON'T:
- ❌ Commit private keys to Git
- ❌ Share private keys between environments
- ❌ Use weak passphrases
- ❌ Skip key backups (you'll lose signing ability)
- ❌ Use personal GPG keys for CI/CD
- ❌ Store keys in unencrypted cloud storage

---

## Troubleshooting

### "GPG signing failed" in GitHub Actions

1. Verify `GPG_PRIVATE_KEY` is complete (includes BEGIN/END lines)
2. Check `GPG_PASSPHRASE` is correct
3. Ensure key hasn't expired: `gpg --list-keys`
4. Review GitHub Actions logs for specific error

### "Cosign verification failed"

1. Verify `COSIGN_PRIVATE_KEY` matches `cosign.pub`
2. Check `COSIGN_PASSWORD` is correct
3. Ensure Docker image was pushed before signing
4. Verify Cosign version compatibility

### "Docker push failed"

1. Verify `DOCKER_USERNAME` is correct
2. Check `DOCKER_PASSWORD` is a valid access token (not password)
3. Ensure token has Write permissions
4. Verify repository name matches Docker Hub

---

## References

- **GPG Documentation**: https://gnupg.org/documentation/
- **Cosign Documentation**: https://docs.sigstore.dev/cosign/overview/
- **GitHub Secrets**: https://docs.github.com/en/actions/security-guides/encrypted-secrets
- **Docker Access Tokens**: https://docs.docker.com/docker-hub/access-tokens/
- **NuGet API Keys**: https://learn.microsoft.com/en-us/nuget/nuget-org/publish-a-package

---

**Next Steps**: After configuring secrets, see [GITHUB_RELEASE_PROCESS.md](GITHUB_RELEASE_PROCESS.md) for release procedures.
