#!/bin/bash
# Verify GPG signatures on all release artifacts
# Usage: ./verify-signatures.sh

set -e

ARTIFACTS_DIR="publish"
FAILED=0
SUCCESS=0

echo "🔍 Verifying GPG signatures on SysDocs release artifacts..."
echo ""

# Check if GPG is available
if ! command -v gpg &> /dev/null; then
    echo "❌ GPG is not installed. Please install gnupg."
    exit 1
fi

# Check if artifacts directory exists
if [ ! -d "${ARTIFACTS_DIR}" ]; then
    echo "❌ Artifacts directory '${ARTIFACTS_DIR}' not found."
    exit 1
fi

# Verify all .asc signature files
echo "📦 Verifying release archive signatures..."
for asc_file in ${ARTIFACTS_DIR}/*.asc; do
    if [ -f "$asc_file" ]; then
        artifact="${asc_file%.asc}"
        if [ ! -f "$artifact" ]; then
            echo "  ⚠️  Warning: Signature file exists but artifact missing: $(basename $artifact)"
            FAILED=$((FAILED + 1))
            continue
        fi
        
        echo "  🔍 Verifying: $(basename $artifact)"
        
        # Capture GPG output
        gpg_output=$(gpg --verify "$asc_file" "$artifact" 2>&1)
        
        if echo "$gpg_output" | grep -q "Good signature"; then
            # Extract signer info
            signer=$(echo "$gpg_output" | grep "Good signature" | sed 's/.*Good signature from "\(.*\)".*/\1/')
            echo "      ✅ Valid signature from: $signer"
            SUCCESS=$((SUCCESS + 1))
        else
            echo "      ❌ Invalid or missing signature!"
            echo "$gpg_output" | grep -E "(BAD signature|Can't check signature)" | sed 's/^/         /'
            FAILED=$((FAILED + 1))
        fi
    fi
done

echo ""
# Verify checksums signature
if [ -f "${ARTIFACTS_DIR}/SHA256SUMS.gpg" ]; then
    echo "📋 Verifying checksums signature..."
    echo "  🔍 Verifying: SHA256SUMS.gpg"
    
    gpg_output=$(gpg --verify "${ARTIFACTS_DIR}/SHA256SUMS.gpg" 2>&1)
    
    if echo "$gpg_output" | grep -q "Good signature"; then
        signer=$(echo "$gpg_output" | grep "Good signature" | sed 's/.*Good signature from "\(.*\)".*/\1/')
        echo "      ✅ Valid signature from: $signer"
        SUCCESS=$((SUCCESS + 1))
        
        # Verify file integrity using checksums
        echo ""
        echo "🔐 Verifying file integrity against checksums..."
        cd ${ARTIFACTS_DIR}
        if gpg --decrypt SHA256SUMS.gpg 2>/dev/null | sha256sum -c --ignore-missing 2>&1 | grep -v "^sha256sum:"; then
            echo "      ✅ All checksums verified successfully"
        else
            echo "      ❌ Checksum verification failed"
            FAILED=$((FAILED + 1))
        fi
        cd - > /dev/null
    else
        echo "      ❌ Invalid or missing signature on checksums!"
        echo "$gpg_output" | grep -E "(BAD signature|Can't check signature)" | sed 's/^/         /'
        FAILED=$((FAILED + 1))
    fi
elif [ -f "${ARTIFACTS_DIR}/SHA256SUMS" ]; then
    echo "⚠️  Warning: SHA256SUMS exists but is not signed (SHA256SUMS.gpg missing)"
    FAILED=$((FAILED + 1))
fi

echo ""
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo "📊 Verification Summary:"
echo "   ✅ Valid signatures: $SUCCESS"
echo "   ❌ Failed/Missing:   $FAILED"
echo "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━"
echo ""

if [ $FAILED -eq 0 ]; then
    echo "✅ All signatures verified successfully!"
    echo ""
    echo "💡 These artifacts are safe to distribute."
    exit 0
else
    echo "❌ Some signatures failed verification!"
    echo ""
    echo "⚠️  DO NOT distribute these artifacts until signing issues are resolved."
    echo ""
    echo "📖 See docs/CODE_SIGNING_GUIDE.md for help with GPG signing."
    exit 1
fi
