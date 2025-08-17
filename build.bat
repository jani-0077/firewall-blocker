@echo off
echo Building Firewall Blocker Application...
echo.
echo Choose build type:
echo 1. Regular build (multiple files)
echo 2. Single executable (self-contained)
echo.
set /p choice="Enter choice (1 or 2): "

if "%choice%"=="1" (
    echo.
    echo Building regular application...
    REM Try to use dotnet from Program Files first
    if exist "C:\Program Files\dotnet\dotnet.exe" (
        echo Using .NET SDK from Program Files...
        "C:\Program Files\dotnet\dotnet.exe" build --configuration Release
        goto :end
    )

    REM Try to use dotnet from PATH
    where dotnet >nul 2>&1
    if %errorlevel% == 0 (
        echo Using .NET SDK from PATH...
        dotnet build --configuration Release
        goto :end
    )

    REM Try to use msbuild
    where msbuild >nul 2>&1
    if %errorlevel% == 0 (
        echo Using MSBuild...
        msbuild FirewallBlocker.csproj /p:Configuration=Release
        goto :end
    )
) else if "%choice%"=="2" (
    echo.
    echo Publishing single executable...
    REM Try to use dotnet from Program Files first
    if exist "C:\Program Files\dotnet\dotnet.exe" (
        echo Using .NET SDK from Program Files...
        "C:\Program Files\dotnet\dotnet.exe" publish --configuration Release --output "publish-clean"
        if %errorlevel%==0 (
            echo.
            echo Single executable created successfully!
            echo Location: publish-clean\FirewallBlocker.exe
            echo Size: 120MB (self-contained)
            echo.
            echo You can now copy this single file to any Windows machine.
            echo No .NET installation required on the target machine.
        )
        goto :end
    )

    REM Try to use dotnet from PATH
    where dotnet >nul 2>&1
    if %errorlevel% == 0 (
        echo Using .NET SDK from PATH...
        dotnet publish --configuration Release --output "publish-clean"
        if %errorlevel%==0 (
            echo.
            echo Single executable created successfully!
            echo Location: publish-clean\FirewallBlocker.exe
            echo Size: 120MB (self-contained)
            echo.
            echo You can now copy this single file to any Windows machine.
            echo No .NET installation required on the target machine.
        )
        goto :end
    )
) else (
    echo Invalid choice. Please run the script again.
    pause
    exit /b 1
)

REM If neither is found, show error
echo Error: Neither .NET SDK nor MSBuild found on this system.
echo.
echo Please install one of the following:
echo 1. .NET 9.0 SDK from https://dotnet.microsoft.com/download/dotnet/9.0
echo 2. Visual Studio Build Tools from https://visualstudio.microsoft.com/downloads/#build-tools-for-visual-studio-2022
echo.
echo After installation, restart your terminal and run this script again.
pause

:end
echo.
echo Build completed!
pause
