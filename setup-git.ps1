# Firewall Blocker - Git Setup Script (PowerShell)
Write-Host "========================================" -ForegroundColor Green
Write-Host "Firewall Blocker - Git Setup Script" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""

# Function to check if command exists
function Test-Command($cmdname) {
    return [bool](Get-Command -Name $cmdname -ErrorAction SilentlyContinue)
}

# Check if Git is available
if (Test-Command "git") {
    Write-Host "✓ Git is available" -ForegroundColor Green
} else {
    Write-Host "✗ Git is not available in PATH" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please install Git first:" -ForegroundColor Yellow
    Write-Host "1. Download from: https://git-scm.com/download/win" -ForegroundColor Cyan
    Write-Host "2. Run the installer with default settings" -ForegroundColor Cyan
    Write-Host "3. Restart your terminal/PowerShell" -ForegroundColor Cyan
    Write-Host "4. Run this script again" -ForegroundColor Cyan
    Write-Host ""
    Read-Host "Press Enter to continue"
    exit
}

Write-Host ""
Write-Host "Setting up Git repository..." -ForegroundColor Yellow
Write-Host ""

# Check if Git is already initialized
if (Test-Path ".git") {
    Write-Host "Repository already initialized. Checking status..." -ForegroundColor Yellow
    git status
    Write-Host ""
    Write-Host "To push to remote repository:" -ForegroundColor Cyan
    Write-Host "1. Create repository on GitHub/GitLab/Azure DevOps" -ForegroundColor White
    Write-Host "2. Run: git remote add origin YOUR_REPOSITORY_URL" -ForegroundColor White
    Write-Host "3. Run: git push -u origin main" -ForegroundColor White
    Write-Host ""
    Read-Host "Press Enter to continue"
    exit
}

# Initialize Git repository
Write-Host "Initializing Git repository..." -ForegroundColor Yellow
git init

# Configure Git (will prompt for user input)
Write-Host ""
Write-Host "Configuring Git (first time setup)..." -ForegroundColor Yellow
Write-Host ""

$gitUsername = Read-Host "Enter your Git username"
$gitEmail = Read-Host "Enter your Git email"

git config user.name $gitUsername
git config user.email $gitEmail

Write-Host ""
Write-Host "Git configuration set:" -ForegroundColor Green
Write-Host "Username: $gitUsername" -ForegroundColor Cyan
Write-Host "Email: $gitEmail" -ForegroundColor Cyan
Write-Host ""

# Add all files
Write-Host "Adding files to Git..." -ForegroundColor Yellow
git add .

# Make initial commit
Write-Host ""
Write-Host "Making initial commit..." -ForegroundColor Yellow
git commit -m "Initial commit: Firewall Blocker application with context menu integration"

# Show status
Write-Host ""
Write-Host "Repository status:" -ForegroundColor Yellow
git status

Write-Host ""
Write-Host "========================================" -ForegroundColor Green
Write-Host "Git repository setup complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. Create a repository on your Git provider:" -ForegroundColor White
Write-Host "   - GitHub: https://github.com/new" -ForegroundColor Yellow
Write-Host "   - GitLab: https://gitlab.com/projects/new" -ForegroundColor Yellow
Write-Host "   - Azure DevOps: Create new repository" -ForegroundColor Yellow
Write-Host ""
Write-Host "2. Copy the repository URL" -ForegroundColor White
Write-Host ""
Write-Host "3. Add remote origin:" -ForegroundColor White
Write-Host "   git remote add origin YOUR_REPOSITORY_URL" -ForegroundColor Cyan
Write-Host ""
Write-Host "4. Push to remote:" -ForegroundColor White
Write-Host "   git push -u origin main" -ForegroundColor Cyan
Write-Host ""
Write-Host "5. Verify on your Git provider website" -ForegroundColor White
Write-Host ""
Write-Host "Repository information:" -ForegroundColor Cyan
Write-Host "- Local path: $(Get-Location)" -ForegroundColor White
Write-Host "- Git config: $env:USERPROFILE\.gitconfig" -ForegroundColor White
Write-Host "- Remote: Not set yet" -ForegroundColor White
Write-Host ""
Read-Host "Press Enter to continue"
