# Verify Deterministic Builds
# This script builds the project twice and compares the output hashes

param(
    [Parameter()]
    [string]$Configuration = "Release"
)

$ErrorActionPreference = "Stop"
$ProjectRoot = Split-Path -Parent $PSScriptRoot
$SrcDir = Join-Path $ProjectRoot "src"
$CliProject = Join-Path $SrcDir "SysDocs.Cli\SysDocs.Cli.csproj"

function Write-Header {
    param([string]$Message)
    Write-Host "`n========================================" -ForegroundColor Cyan
    Write-Host " $Message" -ForegroundColor Cyan
    Write-Host "========================================`n" -ForegroundColor Cyan
}

function Get-DirectoryHash {
    param([string]$Path)
    
    $files = Get-ChildItem -Path $Path -Recurse -File | Sort-Object FullName
    $hasher = [System.Security.Cryptography.SHA256]::Create()
    
    foreach ($file in $files) {
        $bytes = [System.IO.File]::ReadAllBytes($file.FullName)
        $null = $hasher.TransformBlock($bytes, 0, $bytes.Length, $null, 0)
    }
    
    $hasher.TransformFinalBlock([byte[]]::new(0), 0, 0)
    $hash = [System.BitConverter]::ToString($hasher.Hash) -replace '-'
    $hasher.Dispose()
    
    return $hash
}

try {
    Write-Header "Verifying Deterministic Builds"
    
    # First build
    Write-Host "Building first time..." -ForegroundColor Yellow
    $Build1Dir = Join-Path $ProjectRoot "verify-build-1"
    dotnet publish $CliProject `
        --configuration $Configuration `
        --output $Build1Dir `
        /p:Deterministic=true `
        /p:ContinuousIntegrationBuild=true `
        --verbosity quiet
    
    $Hash1 = Get-DirectoryHash -Path $Build1Dir
    Write-Host "Build 1 Hash: $Hash1" -ForegroundColor Cyan
    
    # Clean
    Start-Sleep -Seconds 2
    
    # Second build
    Write-Host "`nBuilding second time..." -ForegroundColor Yellow
    $Build2Dir = Join-Path $ProjectRoot "verify-build-2"
    dotnet publish $CliProject `
        --configuration $Configuration `
        --output $Build2Dir `
        /p:Deterministic=true `
        /p:ContinuousIntegrationBuild=true `
        --verbosity quiet
    
    $Hash2 = Get-DirectoryHash -Path $Build2Dir
    Write-Host "Build 2 Hash: $Hash2" -ForegroundColor Cyan
    
    # Compare
    Write-Host "`nComparing builds..." -ForegroundColor Yellow
    
    if ($Hash1 -eq $Hash2) {
        Write-Host "`n✓ SUCCESS: Builds are deterministic!" -ForegroundColor Green
        Write-Host "  Both builds produced identical outputs" -ForegroundColor Green
        $exitCode = 0
    } else {
        Write-Host "`n✗ FAILURE: Builds are NOT deterministic!" -ForegroundColor Red
        Write-Host "  Build outputs differ between runs" -ForegroundColor Red
        $exitCode = 1
    }
    
    # Cleanup
    Write-Host "`nCleaning up verification builds..." -ForegroundColor Yellow
    Remove-Item -Path $Build1Dir -Recurse -Force
    Remove-Item -Path $Build2Dir -Recurse -Force
    
    exit $exitCode
}
catch {
    Write-Host "`n✗ Verification failed: $_" -ForegroundColor Red
    
    # Cleanup on error
    if (Test-Path $Build1Dir) { Remove-Item -Path $Build1Dir -Recurse -Force }
    if (Test-Path $Build2Dir) { Remove-Item -Path $Build2Dir -Recurse -Force }
    
    exit 1
}
