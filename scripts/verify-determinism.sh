#!/usr/bin/env bash
# Verify Deterministic Builds
# This script builds the project twice and compares the output hashes

set -e

CONFIGURATION="${CONFIGURATION:-Release}"
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SRC_DIR="$PROJECT_ROOT/src"
CLI_PROJECT="$SRC_DIR/SysDocs.Cli/SysDocs.Cli.csproj"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
CYAN='\033[0;36m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

print_header() {
    echo -e "\n${CYAN}========================================${NC}"
    echo -e "${CYAN} $1${NC}"
    echo -e "${CYAN}========================================${NC}\n"
}

get_directory_hash() {
    local dir=$1
    find "$dir" -type f | sort | xargs cat | sha256sum | awk '{print $1}'
}

cleanup() {
    echo -e "\n${YELLOW}Cleaning up verification builds...${NC}"
    [ -d "$BUILD1_DIR" ] && rm -rf "$BUILD1_DIR"
    [ -d "$BUILD2_DIR" ] && rm -rf "$BUILD2_DIR"
}

trap cleanup EXIT

print_header "Verifying Deterministic Builds"

# First build
echo -e "${YELLOW}Building first time...${NC}"
BUILD1_DIR="$PROJECT_ROOT/verify-build-1"
dotnet publish "$CLI_PROJECT" \
    --configuration "$CONFIGURATION" \
    --output "$BUILD1_DIR" \
    /p:Deterministic=true \
    /p:ContinuousIntegrationBuild=true \
    --verbosity quiet

HASH1=$(get_directory_hash "$BUILD1_DIR")
echo -e "${CYAN}Build 1 Hash: $HASH1${NC}"

# Clean
sleep 2

# Second build
echo -e "\n${YELLOW}Building second time...${NC}"
BUILD2_DIR="$PROJECT_ROOT/verify-build-2"
dotnet publish "$CLI_PROJECT" \
    --configuration "$CONFIGURATION" \
    --output "$BUILD2_DIR" \
    /p:Deterministic=true \
    /p:ContinuousIntegrationBuild=true \
    --verbosity quiet

HASH2=$(get_directory_hash "$BUILD2_DIR")
echo -e "${CYAN}Build 2 Hash: $HASH2${NC}"

# Compare
echo -e "\n${YELLOW}Comparing builds...${NC}"

if [ "$HASH1" = "$HASH2" ]; then
    echo -e "\n${GREEN}✓ SUCCESS: Builds are deterministic!${NC}"
    echo -e "${GREEN}  Both builds produced identical outputs${NC}"
    exit 0
else
    echo -e "\n${RED}✗ FAILURE: Builds are NOT deterministic!${NC}"
    echo -e "${RED}  Build outputs differ between runs${NC}"
    exit 1
fi
