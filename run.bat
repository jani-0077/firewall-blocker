@echo off
echo Running Firewall Blocker Application...
echo.

REM Check if the executable exists
if not exist "bin\Release\net9.0-windows\FirewallBlocker.exe" (
    echo Error: Application not built yet.
    echo Please run build.bat first.
    pause
    exit /b 1
)

REM Run the application
echo Starting Firewall Blocker...
"bin\Release\net9.0-windows\FirewallBlocker.exe"
