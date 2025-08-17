@echo off
echo ========================================
echo Firewall Blocker - Icon Verification
echo ========================================
echo.
echo Icon file: G:\firewallblocker\firewall.ico
echo Single executable: publish-clean\FirewallBlocker.exe
echo.
echo Testing the icon:
echo.
echo 1. Check if the application is running in taskbar
echo    - Look for the firewall icon in the taskbar
echo    - The icon should be visible when the app is running
echo.
echo 2. Check file explorer icon
echo    - Navigate to publish-clean\FirewallBlocker.exe
echo    - The .exe file should show the firewall icon
echo.
echo 3. Check application window icon
echo    - The app title bar should show the firewall icon
echo.
echo 4. Check taskbar preview
echo    - Hover over the taskbar icon
echo    - The preview should show the firewall icon
echo.
echo If you don't see the icon:
echo - Make sure firewall.ico exists at G:\firewallblocker\firewall.ico
echo - Try refreshing the file explorer (F5)
echo - Restart the application
echo.
echo Current status:
if exist "G:\firewallblocker\firewall.ico" (
    echo ✓ Icon file found
) else (
    echo ✗ Icon file missing
)

if exist "publish-clean\FirewallBlocker.exe" (
    echo ✓ Single executable found
) else (
    echo ✗ Single executable missing
)
echo.
pause

