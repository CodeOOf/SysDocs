#!/usr/bin/env pwsh
# Verify code signatures on assemblies and executables

param(
    [string]$SearchPath = "src\**\bin\Release\**\*",
    [switch]$Verbose
)

$ErrorActionPreference = "Stop"

Write-Host "🔍 SysDocs Signature Verification Script" -ForegroundColor Cyan
Write-Host "=========================================" -ForegroundColor Cyan
Write-Host ""

# Find files to verify
$files = Get-ChildItem -Path $SearchPath -Include @("*.dll", "*.exe") -Recurse -ErrorAction SilentlyContinue
if ($files.Count -eq 0) {
    Write-Host "⚠️  No files found matching pattern: $SearchPath" -ForegroundColor Yellow
    exit 0
}

Write-Host "📦 Found $($files.Count) files to verify:" -ForegroundColor Green
if ($Verbose) {
    $files | ForEach-Object { Write-Host "   - $($_.Name)" -ForegroundColor Gray }
}
Write-Host ""

$verifiedCount = 0
$unsignedCount = 0
$invalidCount = 0
$errors = @()

foreach ($file in $files) {
    if ($Verbose) {
        Write-Host "🔍 Verifying: $($file.Name)..." -ForegroundColor Cyan
    }
    
    # Check strong-name signature for .dll files
    if ($file.Extension -eq ".dll") {
        $snResult = & sn -vf "$($file.FullName)" 2>&1
        $snValid = $snResult -match "is valid"
        
        if ($snValid) {
            if ($Verbose) {
                Write-Host "   ✅ Strong-name signature valid" -ForegroundColor Green
            }
        } else {
            if ($Verbose) {
                Write-Host "   ⚠️  Not strong-name signed" -ForegroundColor Yellow
            }
        }
    }
    
    # Check Authenticode signature (Windows only)
    if ($IsWindows -or $env:OS -eq "Windows_NT") {
        try {
            $signature = Get-AuthenticodeSignature -FilePath $file.FullName -ErrorAction Stop
            
            if ($signature.Status -eq "Valid") {
                if ($Verbose) {
                    Write-Host "   ✅ Authenticode signature valid" -ForegroundColor Green
                    Write-Host "      Signer: $($signature.SignerCertificate.Subject)" -ForegroundColor Gray
                    Write-Host "      Timestamp: $($signature.TimeStamperCertificate.NotBefore)" -ForegroundColor Gray
                }
                $verifiedCount++
            } elseif ($signature.Status -eq "NotSigned") {
                if ($Verbose) {
                    Write-Host "   ⚠️  Not Authenticode signed" -ForegroundColor Yellow
                }
                $unsignedCount++
                $errors += "UNSIGNED: $($file.Name)"
            } else {
                if ($Verbose) {
                    Write-Host "   ❌ Invalid signature: $($signature.Status)" -ForegroundColor Red
                }
                $invalidCount++
                $errors += "INVALID: $($file.Name) - $($signature.Status)"
            }
        } catch {
            if ($Verbose) {
                Write-Host "   ❌ Error verifying signature: $_" -ForegroundColor Red
            }
            $invalidCount++
            $errors += "ERROR: $($file.Name) - $_"
        }
    } else {
        Write-Host "⚠️  Authenticode verification only supported on Windows" -ForegroundColor Yellow
        break
    }
}

Write-Host ""
Write-Host "📊 Verification Summary:" -ForegroundColor Cyan
Write-Host "   ✅ Valid signatures: $verifiedCount" -ForegroundColor Green
Write-Host "   ⚠️  Unsigned: $unsignedCount" -ForegroundColor Yellow
Write-Host "   ❌ Invalid: $invalidCount" -ForegroundColor Red
Write-Host ""

if ($errors.Count -gt 0) {
    Write-Host "❌ Issues found:" -ForegroundColor Red
    $errors | ForEach-Object { Write-Host "   - $_" -ForegroundColor Yellow }
    Write-Host ""
    Write-Host "💡 Run signing script to fix: .\scripts\sign-assemblies.ps1" -ForegroundColor Cyan
    exit 1
}

Write-Host "✅ All signatures verified successfully!" -ForegroundColor Green
exit 0
