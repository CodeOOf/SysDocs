#!/usr/bin/env bash
# Build script for SysDocs (Unix/Linux/macOS)
# Usage: ./scripts/build.sh [command]
# Commands: restore, build, test, publish, docker, clean

set -e

COMMAND="${1:-build}"
CONFIGURATION="${CONFIGURATION:-Release}"
PROJECT_ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
SRC_DIR="$PROJECT_ROOT/src"
SOLUTION_FILE="$SRC_DIR/SysDocs.sln"

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

cmd_restore() {
    print_header "Restoring dependencies"
    dotnet restore "$SOLUTION_FILE"
}

cmd_build() {
    print_header "Building solution"
    dotnet build "$SOLUTION_FILE" \
        --configuration "$CONFIGURATION" \
        --no-restore \
        /p:TreatWarningsAsErrors=true \
        /p:Deterministic=true
}

cmd_test() {
    print_header "Running tests"
    dotnet test "$SOLUTION_FILE" \
        --configuration "$CONFIGURATION" \
        --no-build \
        --verbosity normal \
        --logger "trx;LogFileName=test-results.trx"
}

cmd_publish() {
    print_header "Publishing application"
    local publish_dir="$PROJECT_ROOT/publish"
    
    dotnet publish "$SRC_DIR/SysDocs.Cli/SysDocs.Cli.csproj" \
        --configuration "$CONFIGURATION" \
        --output "$publish_dir" \
        /p:Deterministic=true \
        /p:ContinuousIntegrationBuild=true
    
    echo -e "\n${GREEN}Published to: $publish_dir${NC}"
}

cmd_docker() {
    print_header "Building Docker image"
    docker build -t sysdocs:latest "$PROJECT_ROOT"
    
    if [ $? -eq 0 ]; then
        echo -e "\n${GREEN}Docker image built successfully: sysdocs:latest${NC}"
    fi
}

cmd_clean() {
    print_header "Cleaning build artifacts"
    
    find "$SRC_DIR" -type d -name bin -o -name obj | xargs rm -rf 2>/dev/null || true
    
    local publish_dir="$PROJECT_ROOT/publish"
    [ -d "$publish_dir" ] && rm -rf "$publish_dir"
    
    local test_results="$SRC_DIR/TestResults"
    [ -d "$test_results" ] && rm -rf "$test_results"
    
    echo -e "${GREEN}Clean complete${NC}"
}

cmd_all() {
    cmd_restore
    cmd_build
    cmd_test
    cmd_publish
}

show_help() {
    echo -e "${YELLOW}Unknown command: $COMMAND${NC}"
    echo ""
    echo "Available commands:"
    echo "  restore  - Restore NuGet packages"
    echo "  build    - Build the solution"
    echo "  test     - Run tests"
    echo "  publish  - Publish the application"
    echo "  docker   - Build Docker image"
    echo "  clean    - Clean build artifacts"
    echo "  all      - Restore, build, test, and publish"
    exit 1
}

# Main execution
case "$COMMAND" in
    restore)
        cmd_restore
        ;;
    build)
        cmd_restore
        cmd_build
        ;;
    test)
        cmd_restore
        cmd_build
        cmd_test
        ;;
    publish)
        cmd_publish
        ;;
    docker)
        cmd_docker
        ;;
    clean)
        cmd_clean
        ;;
    all)
        cmd_all
        ;;
    *)
        show_help
        ;;
esac

echo -e "\n${GREEN}✓ $COMMAND completed successfully${NC}"
