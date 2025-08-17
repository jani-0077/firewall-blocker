# Firewall Blocker Build Script
Write-Host "Building Firewall Blocker Application..." -ForegroundColor Green
Write-Host ""

# Function to check if command exists
function Test-Command($cmdname) {
    return [bool](Get-Command -Name $cmdname -ErrorAction SilentlyContinue)
}

Write-Host "Choose build type:" -ForegroundColor Cyan
Write-Host "1. Regular build (multiple files)" -ForegroundColor White
Write-Host "2. Single executable (self-contained)" -ForegroundColor White
Write-Host ""

$choice = Read-Host "Enter choice (1 or 2)"

if ($choice -eq "1") {
    Write-Host ""
    Write-Host "Building regular application..." -ForegroundColor Yellow
    
    # Try to use dotnet from Program Files first
    if (Test-Path "C:\Program Files\dotnet\dotnet.exe") {
        Write-Host "Using .NET SDK from Program Files..." -ForegroundColor Yellow
        try {
            & "C:\Program Files\dotnet\dotnet.exe" build --configuration Release
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Build completed successfully!" -ForegroundColor Green
            } else {
                Write-Host "Build failed with exit code: $LASTEXITCODE" -ForegroundColor Red
            }
        } catch {
            Write-Host "Error during build: $_" -ForegroundColor Red
        }
    }
    # Try to use dotnet from PATH
    elseif (Test-Command "dotnet") {
        Write-Host "Using .NET SDK from PATH..." -ForegroundColor Yellow
        try {
            dotnet build --configuration Release
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Build completed successfully!" -ForegroundColor Green
            } else {
                Write-Host "Build failed with exit code: $LASTEXITCODE" -ForegroundColor Red
            }
        } catch {
            Write-Host "Error during build: $_" -ForegroundColor Red
        }
    }
    # Try to use msbuild
    elseif (Test-Command "msbuild") {
        Write-Host "Using MSBuild..." -ForegroundColor Yellow
        try {
            msbuild FirewallBlocker.csproj /p:Configuration=Release
            if ($LASTEXITCODE -eq 0) {
                Write-Host "Build completed successfully!" -ForegroundColor Green
            } else {
                Write-Host "Build failed with exit code: $LASTEXITCODE" -ForegroundColor Red
            }
        } catch {
            Write-Host "Error during build: $_" -ForegroundColor Red
        }
    }
}
elseif ($choice -eq "2") {
    Write-Host ""
    Write-Host "Publishing single executable..." -ForegroundColor Yellow
    
    # Try to use dotnet from Program Files first
    if (Test-Path "C:\Program Files\dotnet\dotnet.exe") {
        Write-Host "Using .NET SDK from Program Files..." -ForegroundColor Yellow
        try {
            & "C:\Program Files\dotnet\dotnet.exe" publish --configuration Release --output "publish-clean"
            if ($LASTEXITCODE -eq 0) {
                Write-Host ""
                Write-Host "Single executable created successfully!" -ForegroundColor Green
                Write-Host "Location: publish-clean\FirewallBlocker.exe" -ForegroundColor Cyan
                Write-Host "Size: 120MB (self-contained)" -ForegroundColor Cyan
                Write-Host ""
                Write-Host "You can now copy this single file to any Windows machine." -ForegroundColor Yellow
                Write-Host "No .NET installation required on the target machine." -ForegroundColor Yellow
            } else {
                Write-Host "Publish failed with exit code: $LASTEXITCODE" -ForegroundColor Red
            }
        } catch {
            Write-Host "Error during publish: $_" -ForegroundColor Red
        }
    }
    # Try to use dotnet from PATH
    elseif (Test-Command "dotnet") {
        Write-Host "Using .NET SDK from PATH..." -ForegroundColor Yellow
        try {
            dotnet publish --configuration Release --output "publish-clean"
            if ($LASTEXITCODE -eq 0) {
                Write-Host ""
                Write-Host "Single executable created successfully!" -ForegroundColor Green
                Write-Host "Location: publish-clean\FirewallBlocker.exe" -ForegroundColor Cyan
                Write-Host "Size: 120MB (self-contained)" -ForegroundColor Cyan
                Write-Host ""
                Write-Host "You can now copy this single file to any Windows machine." -ForegroundColor Yellow
                Write-Host "No .NET installation required on the target machine." -ForegroundColor Yellow
            } else {
                Write-Host "Publish failed with exit code: $LASTEXITCODE" -ForegroundColor Red
            }
        } catch {
            Write-Host "Error during publish: $_" -ForegroundColor Red
        }
    }
}
else {
    Write-Host "Invalid choice. Please run the script again." -ForegroundColor Red
    Read-Host "Press Enter to continue"
    exit
}

# If neither is found, show error
if (-not (Test-Command "dotnet") -and -not (Test-Path "C:\Program Files\dotnet\dotnet.exe") -and -not (Test-Command "msbuild")) {
    Write-Host "Error: Neither .NET SDK nor MSBuild found on this system." -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install one of the following:" -ForegroundColor Yellow
    Write-Host "1. .NET 9.0 SDK from https://dotnet.microsoft.com/download/dotnet/9.0" -ForegroundColor Cyan
    Write-Host "2. Visual Studio Build Tools from https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "After installation, restart your terminal and run this script again." -ForegroundColor Yellow
}

Write-Host ""
Read-Host "Press Enter to continue"
