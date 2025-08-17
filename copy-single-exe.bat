@echo off
echo Firewall Blocker - Single Executable Copy Tool
echo ==============================================
echo.

REM Check if the single executable exists
if not exist "publish-clean\FirewallBlocker.exe" (
    echo Error: Single executable not found!
    echo Please run the build process first.
    echo.
    echo To build: dotnet publish --configuration Release --output publish-clean
    pause
    exit /b 1
)

echo Single executable found: publish-clean\FirewallBlocker.exe
echo Size: 120MB (self-contained, no dependencies)
echo.
echo This executable includes:
echo - .NET 9.0 Runtime (embedded)
echo - All required libraries
echo - Custom firewall icon
echo - Administrator manifest
echo.
echo You can copy this single file to any Windows 10/11 machine.
echo No .NET installation required on the target machine.
echo.

REM Ask user where to copy the file
set /p target_path="Enter destination path (or press Enter to copy to Desktop): "

if "%target_path%"=="" (
    set target_path=%USERPROFILE%\Desktop
)

REM Create target directory if it doesn't exist
if not exist "%target_path%" (
    mkdir "%target_path%"
)

REM Copy the file
echo.
echo Copying FirewallBlocker.exe to: %target_path%
copy "publish-clean\FirewallBlocker.exe" "%target_path%\"

if %errorlevel%==0 (
    echo.
    echo Success! FirewallBlocker.exe has been copied to: %target_path%
    echo.
    echo To use:
    echo 1. Right-click the executable and "Run as administrator"
    echo 2. Click "Install Context Menu"
    echo 3. Right-click any .exe file to see "Block in Firewall" option
) else (
    echo.
    echo Error: Failed to copy the file!
    echo Please check the destination path and try again.
)

echo.
pause
