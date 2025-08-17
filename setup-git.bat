@echo off
echo ========================================
echo Firewall Blocker - Git Setup Script
echo ========================================
echo.

REM Check if Git is available
where git >nul 2>&1
if %errorlevel% == 0 (
    echo ✓ Git is available
    goto :git_available
) else (
    echo ✗ Git is not available in PATH
    echo.
    echo Please install Git first:
    echo 1. Download from: https://git-scm.com/download/win
    echo 2. Run the installer with default settings
    echo 3. Restart your terminal/PowerShell
    echo 4. Run this script again
    echo.
    pause
    exit /b 1
)

:git_available
echo.
echo Setting up Git repository...
echo.

REM Check if Git is already initialized
if exist ".git" (
    echo Repository already initialized. Checking status...
    git status
    echo.
    echo To push to remote repository:
    echo 1. Create repository on GitHub/GitLab/Azure DevOps
    echo 2. Run: git remote add origin YOUR_REPOSITORY_URL
    echo 3. Run: git push -u origin main
    echo.
    pause
    exit /b 0
)

REM Initialize Git repository
echo Initializing Git repository...
git init

REM Configure Git (will prompt for user input)
echo.
echo Configuring Git (first time setup)...
echo.
set /p git_username="Enter your Git username: "
set /p git_email="Enter your Git email: "

git config user.name "%git_username%"
git config user.email "%git_email%"

echo.
echo Git configuration set:
echo Username: %git_username%
echo Email: %git_email%
echo.

REM Add all files
echo Adding files to Git...
git add .

REM Make initial commit
echo.
echo Making initial commit...
git commit -m "Initial commit: Firewall Blocker application with context menu integration"

REM Show status
echo.
echo Repository status:
git status

echo.
echo ========================================
echo Git repository setup complete!
echo ========================================
echo.
echo Next steps:
echo.
echo 1. Create a repository on your Git provider:
echo    - GitHub: https://github.com/new
echo    - GitLab: https://gitlab.com/projects/new
echo    - Azure DevOps: Create new repository
echo.
echo 2. Copy the repository URL
echo.
echo 3. Add remote origin:
echo    git remote add origin YOUR_REPOSITORY_URL
echo.
echo 4. Push to remote:
echo    git push -u origin main
echo.
echo 5. Verify on your Git provider website
echo.
echo Repository information:
echo - Local path: %CD%
echo - Git config: %USERPROFILE%\.gitconfig
echo - Remote: Not set yet
echo.
pause
