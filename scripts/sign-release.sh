#!/bin/bash
# Sign all release artifacts with GPG
# Usage: ./sign-release.sh <version>

set -e

VERSION="${1:-unknown}"
ARTIFACTS_DIR="publish"

echo "🔐 Signing SysDocs v${VERSION} release artifacts with GPG..."
echo ""

# Check if GPG is available
if ! command -v gpg &> /dev/null; then
    echo "❌ GPG is not installed. Please install gnupg."
    exit 1
fi

# Check if GPG key is configured
if ! gpg --list-secret-keys &> /dev/null; then
    echo "❌ No GPG secret key found. Please generate a GPG key first."
    echo "   Run: gpg --full-generate-key"
    exit 1
fi

# Create artifacts directory if it doesn't exist
mkdir -p "${ARTIFACTS_DIR}"

# Check if there are files to sign
if [ ! "$(ls -A ${ARTIFACTS_DIR}/*.tar.gz 2>/dev/null)" ] && [ ! "$(ls -A ${ARTIFACTS_DIR}/*.zip 2>/dev/null)" ]; then
    echo "⚠️  No release artifacts found in ${ARTIFACTS_DIR}/"
    echo "   Run 'make publish' first to create release artifacts."
    exit 1
fi

echo "📦 Signing release archives..."
# Sign all tarballs
for tarball in ${ARTIFACTS_DIR}/*.tar.gz; do
    if [ -f "$tarball" ]; then
        echo "  ✍️  Signing: $(basename $tarball)"
        gpg --armor --detach-sign "$tarball"
        if [ $? -eq 0 ]; then
            echo "      ✅ Created: $(basename $tarball).asc"
        else
            echo "      ❌ Failed to sign $(basename $tarball)"
            exit 1
        fi
    fi
done

# Sign all zip files
for zipfile in ${ARTIFACTS_DIR}/*.zip; do
    if [ -f "$zipfile" ]; then
        echo "  ✍️  Signing: $(basename $zipfile)"
        gpg --armor --detach-sign "$zipfile"
        if [ $? -eq 0 ]; then
            echo "      ✅ Created: $(basename $zipfile).asc"
        else
            echo "      ❌ Failed to sign $(basename $zipfile)"
            exit 1
        fi
    fi
done

echo ""
echo "📋 Generating checksums..."
# Generate and sign checksums
cd ${ARTIFACTS_DIR}
sha256sum *.tar.gz *.zip 2>/dev/null > SHA256SUMS || true

if [ -f "SHA256SUMS" ]; then
    echo "  ✍️  Signing checksums file..."
    gpg --clearsign SHA256SUMS
    if [ $? -eq 0 ]; then
        mv SHA256SUMS.asc SHA256SUMS.gpg
        echo "      ✅ Created: SHA256SUMS.gpg"
    else
        echo "      ❌ Failed to sign checksums"
        exit 1
    fi
    
    # Display checksums for verification
    echo ""
    echo "📊 SHA256 Checksums:"
    cat SHA256SUMS
fi
cd - > /dev/null

echo ""
echo "✅ All artifacts signed successfully!"
echo ""
echo "📋 Files created:"
ls -lh ${ARTIFACTS_DIR}/*.asc ${ARTIFACTS_DIR}/*.gpg 2>/dev/null | awk '{print "   " $9 " (" $5 ")"}'
echo ""
echo "🔍 Verification commands:"
echo "   gpg --verify ${ARTIFACTS_DIR}/sysdocs-linux-x64.tar.gz.asc"
echo "   gpg --verify ${ARTIFACTS_DIR}/SHA256SUMS.gpg"
echo ""
echo "📤 For GitHub Release, upload:"
echo "   - All .tar.gz and .zip files"
echo "   - All .asc signature files"
echo "   - SHA256SUMS.gpg"
