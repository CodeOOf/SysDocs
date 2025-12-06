# Build script for SysDocs
# Usage: ./build.ps1 [command]
# Commands: restore, build, test, publish, docker, clean

param(
    [Parameter(Position=0)]
    [string]$Command = "build",
    
    [Parameter()]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$ProjectRoot = $PSScriptRoot
$SrcDir = Join-Path $ProjectRoot "src"
$SolutionFile = Join-Path $SrcDir "SysDocs.sln"

function Write-Header {
    param([string]$Message)
    Write-Host "`n========================================" -ForegroundColor Cyan
    Write-Host " $Message" -ForegroundColor Cyan
    Write-Host "========================================`n" -ForegroundColor Cyan
}

function Invoke-Restore {
    Write-Header "Restoring dependencies"
    dotnet restore $SolutionFile
}

function Invoke-Build {
    Write-Header "Building solution"
    dotnet build $SolutionFile `
        --configuration $Configuration `
        --no-restore `
        /p:TreatWarningsAsErrors=true `
        /p:Deterministic=true
}

function Invoke-Test {
    Write-Header "Running tests"
    dotnet test $SolutionFile `
        --configuration $Configuration `
        --no-build `
        --verbosity normal `
        --logger "trx;LogFileName=test-results.trx"
}

function Invoke-Publish {
    Write-Header "Publishing application"
    $PublishDir = Join-Path $ProjectRoot "publish"
    
    dotnet publish (Join-Path $SrcDir "SysDocs.Cli\SysDocs.Cli.csproj") `
        --configuration $Configuration `
        --output $PublishDir `
        /p:Deterministic=true `
        /p:ContinuousIntegrationBuild=true
    
    Write-Host "`nPublished to: $PublishDir" -ForegroundColor Green
}

function Invoke-Docker {
    Write-Header "Building Docker image"
    docker build -t sysdocs:latest $ProjectRoot
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "`nDocker image built successfully: sysdocs:latest" -ForegroundColor Green
    }
}

function Invoke-Clean {
    Write-Header "Cleaning build artifacts"
    
    Get-ChildItem -Path $SrcDir -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force
    
    $PublishDir = Join-Path $ProjectRoot "publish"
    if (Test-Path $PublishDir) {
        Remove-Item $PublishDir -Recurse -Force
    }
    
    $TestResults = Join-Path $SrcDir "TestResults"
    if (Test-Path $TestResults) {
        Remove-Item $TestResults -Recurse -Force
    }
    
    Write-Host "Clean complete" -ForegroundColor Green
}

function Invoke-All {
    Invoke-Restore
    Invoke-Build
    Invoke-Test
    Invoke-Publish
}

# Main execution
try {
    switch ($Command.ToLower()) {
        "restore" { Invoke-Restore }
        "build" { 
            Invoke-Restore
            Invoke-Build 
        }
        "test" {
            Invoke-Restore
            Invoke-Build
            Invoke-Test
        }
        "publish" { Invoke-Publish }
        "docker" { Invoke-Docker }
        "clean" { Invoke-Clean }
        "all" { Invoke-All }
        default {
            Write-Host "Unknown command: $Command" -ForegroundColor Red
            Write-Host "`nAvailable commands:" -ForegroundColor Yellow
            Write-Host "  restore  - Restore NuGet packages"
            Write-Host "  build    - Build the solution"
            Write-Host "  test     - Run tests"
            Write-Host "  publish  - Publish the application"
            Write-Host "  docker   - Build Docker image"
            Write-Host "  clean    - Clean build artifacts"
            Write-Host "  all      - Restore, build, test, and publish"
            exit 1
        }
    }
    
    Write-Host "`n✓ $Command completed successfully" -ForegroundColor Green
}
catch {
    Write-Host "`n✗ $Command failed: $_" -ForegroundColor Red
    exit 1
}
