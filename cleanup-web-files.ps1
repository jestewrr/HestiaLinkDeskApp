# Optional Cleanup Script
# This script removes old web application files that are no longer needed
# BACKUP YOUR PROJECT BEFORE RUNNING THIS!

# Navigate to project directory
Set-Location "c:\Users\Jester\source\repos\HestiaIT13Final"

Write-Host "=== HestiaLink Desktop - Optional Cleanup ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "This will DELETE old web application files." -ForegroundColor Yellow
Write-Host "Your desktop app will still work perfectly after this." -ForegroundColor Green
Write-Host ""
Write-Host "Files to be removed:" -ForegroundColor Yellow
Write-Host "  - HestiaLink.csproj (old web project)" -ForegroundColor Gray
Write-Host "  - HestiaLink.sln (old web solution)" -ForegroundColor Gray
Write-Host "  - Components/Program.cs (web server startup)" -ForegroundColor Gray
Write-Host "  - Components/App.razor (web root component)" -ForegroundColor Gray
Write-Host "  - appsettings.json (web configuration)" -ForegroundColor Gray
Write-Host "  - appsettings.Development.json (web dev config)" -ForegroundColor Gray
Write-Host "  - Platforms/Android/ folder" -ForegroundColor Gray
Write-Host "  - Platforms/iOS/ folder" -ForegroundColor Gray
Write-Host "  - Platforms/MacCatalyst/ folder" -ForegroundColor Gray
Write-Host "  - Platforms/Tizen/ folder" -ForegroundColor Gray
Write-Host ""

$confirmation = Read-Host "Do you want to proceed with cleanup? (yes/no)"

if ($confirmation -eq 'yes') {
    Write-Host ""
    Write-Host "Starting cleanup..." -ForegroundColor Cyan
    
    # Remove old web project files
    if (Test-Path "HestiaLink.csproj") {
        Remove-Item "HestiaLink.csproj" -Force
        Write-Host "✓ Removed HestiaLink.csproj" -ForegroundColor Green
    }
    
    if (Test-Path "HestiaLink.sln") {
        Remove-Item "HestiaLink.sln" -Force
        Write-Host "✓ Removed HestiaLink.sln" -ForegroundColor Green
    }
    
    if (Test-Path "Components\Program.cs") {
        Remove-Item "Components\Program.cs" -Force
        Write-Host "✓ Removed Components\Program.cs" -ForegroundColor Green
    }
    
    if (Test-Path "Components\App.razor") {
        Remove-Item "Components\App.razor" -Force
        Write-Host "✓ Removed Components\App.razor" -ForegroundColor Green
    }
    
    if (Test-Path "appsettings.json") {
        Remove-Item "appsettings.json" -Force
        Write-Host "✓ Removed appsettings.json" -ForegroundColor Green
    }
    
    if (Test-Path "appsettings.Development.json") {
        Remove-Item "appsettings.Development.json" -Force
        Write-Host "✓ Removed appsettings.Development.json" -ForegroundColor Green
    }
    
    # Remove mobile platform folders
    if (Test-Path "Platforms\Android") {
        Remove-Item "Platforms\Android" -Recurse -Force
        Write-Host "✓ Removed Platforms\Android\" -ForegroundColor Green
    }
    
    if (Test-Path "Platforms\iOS") {
        Remove-Item "Platforms\iOS" -Recurse -Force
        Write-Host "✓ Removed Platforms\iOS\" -ForegroundColor Green
    }
    
    if (Test-Path "Platforms\MacCatalyst") {
        Remove-Item "Platforms\MacCatalyst" -Recurse -Force
        Write-Host "✓ Removed Platforms\MacCatalyst\" -ForegroundColor Green
    }
    
    if (Test-Path "Platforms\Tizen") {
        Remove-Item "Platforms\Tizen" -Recurse -Force
        Write-Host "✓ Removed Platforms\Tizen\" -ForegroundColor Green
    }
    
    Write-Host ""
    Write-Host "=== Cleanup Complete! ===" -ForegroundColor Green
    Write-Host ""
    Write-Host "Your desktop application (HestiaIT13Final) is unaffected." -ForegroundColor Cyan
    Write-Host "You can still run your app normally with F5 in Visual Studio." -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "Cleanup cancelled. No files were deleted." -ForegroundColor Yellow
    Write-Host ""
}

Write-Host "Press any key to exit..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
