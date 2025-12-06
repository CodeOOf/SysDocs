# Makefile for SysDocs
# Cross-platform build automation

.PHONY: help restore build test publish docker clean all reports format lint sign verify-signatures

# Default configuration
CONFIGURATION ?= Release
SRC_DIR = src
SOLUTION = $(SRC_DIR)/SysDocs.sln
CLI_PROJECT = $(SRC_DIR)/SysDocs.Cli/SysDocs.Cli.csproj
TESTS_PROJECT = tests/SysDocs.Tests/SysDocs.Tests.csproj
PUBLISH_DIR = publish

# Detect OS for platform-specific commands
ifeq ($(OS),Windows_NT)
	SCRIPT_EXT = .ps1
	SCRIPT_RUNNER = pwsh -File
	RM_RF = powershell -Command "Remove-Item -Recurse -Force"
else
	SCRIPT_EXT = .sh
	SCRIPT_RUNNER = bash
	RM_RF = rm -rf
endif

help: ## Show this help message
	@echo "SysDocs Build System"
	@echo ""
	@echo "Usage: make [target]"
	@echo ""
	@echo "Targets:"
	@echo "  help            Show this help message"
	@echo "  restore         Restore NuGet dependencies"
	@echo "  build           Build the solution"
	@echo "  test            Run all tests"
	@echo "  test-unit       Run unit tests only"
	@echo "  test-integration Run integration tests only"
	@echo "  test-coverage   Run tests with coverage"
	@echo "  test-determinism Run determinism tests"
	@echo "  test-manifest   Run manifest-based assembly tests"
	@echo "  publish         Publish the CLI application"
	@echo "  docker          Build Docker image"
	@echo "  docker-nix      Build Docker image with Nix (deterministic)"
	@echo "  clean           Clean build artifacts"
	@echo "  reports         Generate traceability and license reports"
	@echo "  report-trace    Generate traceability report only"
	@echo "  report-license  Generate license compliance report only"
	@echo "  sign            Sign all release artifacts (Windows only)"
	@echo "  sign-assemblies Sign .NET assemblies (Windows only)"
	@echo "  verify-signatures Verify code signatures"
	@echo "  format          Format code using dotnet format"
	@echo "  lint            Check code formatting"
	@echo "  nix-build       Build with Nix (deterministic)"
	@echo "  nix-develop     Enter Nix development shell"
	@echo "  verify-determinism Verify build determinism"
	@echo "  all             Run full build pipeline"

restore: ## Restore NuGet dependencies
	@echo "==> Restoring dependencies..."
	dotnet restore $(SOLUTION)

build: restore ## Build the solution
	@echo "==> Building solution..."
	dotnet build $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-restore \
		/p:TreatWarningsAsErrors=true \
		/p:Deterministic=true

test: build ## Run all tests
	@echo "==> Running tests..."
	dotnet test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-build \
		--verbosity normal \
		--logger "trx;LogFileName=test-results.trx"

test-unit: build ## Run unit tests only
	@echo "==> Running unit tests..."
	dotnet test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-build \
		--filter "Category=Unit"

test-integration: build ## Run integration tests only
	@echo "==> Running integration tests..."
	dotnet test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-build \
		--filter "Category=Integration"

test-determinism: build ## Run determinism tests only
	@echo "==> Running determinism tests..."
	dotnet test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-build \
		--filter "Category=Determinism"

test-manifest: build ## Run manifest-based assembly tests only
	@echo "==> Running manifest tests..."
	dotnet test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-build \
		--filter "Category=Manifest"

test-coverage: build ## Run tests with coverage
	@echo "==> Running tests with coverage..."
	dotnet test $(SOLUTION) \
		--configuration $(CONFIGURATION) \
		--no-build \
		--collect:"XPlat Code Coverage" \
		--results-directory ./TestResults

publish: ## Publish the CLI application
	@echo "==> Publishing application..."
	dotnet publish $(CLI_PROJECT) \
		--configuration $(CONFIGURATION) \
		--output $(PUBLISH_DIR) \
		/p:Deterministic=true \
		/p:ContinuousIntegrationBuild=true
	@echo "==> Published to: $(PUBLISH_DIR)"

docker: ## Build Docker image
	@echo "==> Building Docker image..."
	docker build -t sysdocs:latest .
	@echo "==> Docker image built: sysdocs:latest"

docker-nix: ## Build Docker image with Nix (deterministic)
	@echo "==> Building deterministic Docker image with Nix..."
	nix build .#docker
	@echo "==> Loading image into Docker..."
	docker load < result
	@echo "==> Deterministic Docker image built: sysdocs:0.1.0-alpha"

clean: ## Clean build artifacts
	@echo "==> Cleaning build artifacts..."
	@find $(SRC_DIR) -type d -name bin -o -name obj | xargs $(RM_RF) 2>/dev/null || true
	@$(RM_RF) $(PUBLISH_DIR) 2>/dev/null || true
	@$(RM_RF) TestResults 2>/dev/null || true
	@$(RM_RF) result 2>/dev/null || true
	@echo "==> Clean complete"

reports: ## Generate traceability and license compliance reports
	@echo "==> Generating reports..."
	@dotnet run --project $(TESTS_PROJECT) -- --traceability
	@dotnet run --project $(TESTS_PROJECT) -- --license-compliance
	@echo "==> Reports generated in reports/"

report-trace: ## Generate traceability report only
	@echo "==> Generating traceability report..."
	@dotnet run --project $(TESTS_PROJECT) -- --traceability

report-license: ## Generate license compliance report
	@echo "==> Generating license compliance report..."
	@dotnet run --project $(TESTS_PROJECT) -- --license-compliance

sign: sign-assemblies ## Sign all release artifacts (NFR-07)

sign-assemblies: ## Sign .NET assemblies with Authenticode (Windows only)
ifeq ($(OS),Windows_NT)
	@echo "==> Signing assemblies..."
	@$(SCRIPT_RUNNER) scripts/sign-assemblies$(SCRIPT_EXT)
else
	@echo "⚠️  Code signing with Authenticode is only supported on Windows"
	@echo "   Strong-name signing can be enabled in Directory.Build.props"
endif

verify-signatures: ## Verify code signatures on assemblies and executables
ifeq ($(OS),Windows_NT)
	@echo "==> Verifying code signatures..."
	@$(SCRIPT_RUNNER) scripts/verify-signatures$(SCRIPT_EXT)
else
	@echo "⚠️  Authenticode signature verification is only supported on Windows"
	@echo "   Strong-name verification: sn -vf <assembly.dll>"
endif

format: ## Format code using dotnet format
	@echo "==> Formatting code..."
	dotnet format $(SOLUTION)

lint: ## Check code formatting
	@echo "==> Checking code formatting..."
	dotnet format $(SOLUTION) --verify-no-changes

nix-build: ## Build with Nix (deterministic)
	@echo "==> Building with Nix..."
	nix build

nix-develop: ## Enter Nix development shell
	@echo "==> Entering Nix development shell..."
	nix develop

verify-determinism: ## Verify build determinism
	@echo "==> Verifying deterministic builds..."
	@$(SCRIPT_RUNNER) scripts/verify-determinism$(SCRIPT_EXT)

all: restore build test publish ## Run full build pipeline
	@echo "==> Full build pipeline complete"

.DEFAULT_GOAL := help
