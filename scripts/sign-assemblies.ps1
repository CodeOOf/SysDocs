#!/usr/bin/env pwsh
# Sign .NET assemblies with Authenticode
# Requirements: SignTool (Windows SDK) or AzureSignTool

param(
    [string]$CertificatePath = "",
    [string]$CertificatePassword = "",
    [string]$SearchPath = "src\**\bin\Release\**\*.dll",
    [string]$TimestampServer = "http://timestamp.digicert.com",
    [switch]$UseAzureKeyVault,
    [string]$AzureKeyVaultUrl = $env:AZURE_KEY_VAULT_URI,
    [string]$AzureClientId = $env:AZURE_CLIENT_ID,
    [string]$AzureClientSecret = $env:AZURE_CLIENT_SECRET,
    [string]$AzureTenantId = $env:AZURE_TENANT_ID,
    [string]$AzureCertificateName = $env:AZURE_CERTIFICATE_NAME
)

$ErrorActionPreference = "Stop"

Write-Host "🔐 SysDocs Assembly Signing Script" -ForegroundColor Cyan
Write-Host "===================================" -ForegroundColor Cyan
Write-Host ""

# Find assemblies to sign
$assemblies = Get-ChildItem -Path $SearchPath -Recurse -ErrorAction SilentlyContinue
if ($assemblies.Count -eq 0) {
    Write-Host "⚠️  No assemblies found matching pattern: $SearchPath" -ForegroundColor Yellow
    exit 0
}

Write-Host "📦 Found $($assemblies.Count) assemblies to sign:" -ForegroundColor Green
$assemblies | ForEach-Object { Write-Host "   - $($_.Name)" -ForegroundColor Gray }
Write-Host ""

# Determine signing method
if ($UseAzureKeyVault) {
    Write-Host "☁️  Using Azure Key Vault signing..." -ForegroundColor Cyan
    
    # Validate Azure parameters
    if (-not $AzureKeyVaultUrl -or -not $AzureClientId -or -not $AzureClientSecret -or -not $AzureTenantId -or -not $AzureCertificateName) {
        Write-Host "❌ Missing Azure Key Vault configuration!" -ForegroundColor Red
        Write-Host "Required: AZURE_KEY_VAULT_URI, AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, AZURE_TENANT_ID, AZURE_CERTIFICATE_NAME" -ForegroundColor Yellow
        exit 1
    }
    
    # Check for AzureSignTool
    $azureSignTool = Get-Command AzureSignTool -ErrorAction SilentlyContinue
    if (-not $azureSignTool) {
        Write-Host "⚙️  Installing AzureSignTool..." -ForegroundColor Yellow
        dotnet tool install --global AzureSignTool --version 5.0.0
        $azureSignTool = Get-Command AzureSignTool -ErrorAction SilentlyContinue
    }
    
    if (-not $azureSignTool) {
        Write-Host "❌ Failed to install AzureSignTool" -ForegroundColor Red
        exit 1
    }
    
    # Sign each assembly
    $signedCount = 0
    foreach ($assembly in $assemblies) {
        Write-Host "🔏 Signing: $($assembly.Name)..." -ForegroundColor Cyan
        
        & AzureSignTool sign `
            -kvu "$AzureKeyVaultUrl" `
            -kvi "$AzureClientId" `
            -kvt "$AzureTenantId" `
            -kvs "$AzureClientSecret" `
            -kvc "$AzureCertificateName" `
            -fd SHA256 `
            -tr "$TimestampServer" `
            -td SHA256 `
            -d "SysDocs" `
            -du "https://github.com/CodeOOf/SysDocs" `
            "$($assembly.FullName)"
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✅ Signed successfully" -ForegroundColor Green
            $signedCount++
        } else {
            Write-Host "   ❌ Failed to sign" -ForegroundColor Red
        }
    }
} else {
    Write-Host "🔑 Using certificate file signing..." -ForegroundColor Cyan
    
    # Validate certificate parameters
    if (-not $CertificatePath -and -not $env:CODE_SIGNING_CERT_PATH) {
        Write-Host "❌ No certificate specified!" -ForegroundColor Red
        Write-Host "Usage: .\sign-assemblies.ps1 -CertificatePath <path> -CertificatePassword <password>" -ForegroundColor Yellow
        Write-Host "   or: Set CODE_SIGNING_CERT_PATH and CODE_SIGNING_CERT_PASSWORD environment variables" -ForegroundColor Yellow
        exit 1
    }
    
    $certPath = if ($CertificatePath) { $CertificatePath } else { $env:CODE_SIGNING_CERT_PATH }
    $certPassword = if ($CertificatePassword) { $CertificatePassword } else { $env:CODE_SIGNING_CERT_PASSWORD }
    
    if (-not (Test-Path $certPath)) {
        Write-Host "❌ Certificate not found: $certPath" -ForegroundColor Red
        exit 1
    }
    
    # Check for SignTool
    $signTool = Get-Command signtool -ErrorAction SilentlyContinue
    if (-not $signTool) {
        Write-Host "❌ SignTool not found!" -ForegroundColor Red
        Write-Host "Install Windows SDK: https://developer.microsoft.com/en-us/windows/downloads/windows-sdk/" -ForegroundColor Yellow
        exit 1
    }
    
    # Sign each assembly
    $signedCount = 0
    foreach ($assembly in $assemblies) {
        Write-Host "🔏 Signing: $($assembly.Name)..." -ForegroundColor Cyan
        
        if ($certPassword) {
            & signtool sign /f "$certPath" /p "$certPassword" `
                /fd SHA256 /tr "$TimestampServer" /td SHA256 `
                /d "SysDocs" /du "https://github.com/CodeOOf/SysDocs" `
                "$($assembly.FullName)"
        } else {
            & signtool sign /f "$certPath" `
                /fd SHA256 /tr "$TimestampServer" /td SHA256 `
                /d "SysDocs" /du "https://github.com/CodeOOf/SysDocs" `
                "$($assembly.FullName)"
        }
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "   ✅ Signed successfully" -ForegroundColor Green
            $signedCount++
        } else {
            Write-Host "   ❌ Failed to sign" -ForegroundColor Red
        }
    }
}

Write-Host ""
Write-Host "📊 Summary: $signedCount of $($assemblies.Count) assemblies signed successfully" -ForegroundColor Cyan
Write-Host ""

if ($signedCount -lt $assemblies.Count) {
    Write-Host "⚠️  Some assemblies failed to sign!" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ All assemblies signed successfully!" -ForegroundColor Green
exit 0
