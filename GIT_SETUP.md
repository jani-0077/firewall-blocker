# Git Setup Guide for Firewall Blocker

## Step 1: Install Git

### Option 1: Download from Official Website (Recommended)
1. Go to: https://git-scm.com/download/win
2. Download the latest version for Windows
3. Run the installer with default settings
4. Restart your terminal/PowerShell after installation

### Option 2: Install via Chocolatey (if you have it)
```bash
choco install git
```

### Option 3: Install via Winget (Windows 10/11)
```bash
winget install --id Git.Git -e --source winget
```

## Step 2: Verify Git Installation
After installation, restart your terminal and run:
```bash
git --version
```

## Step 3: Configure Git (First Time Setup)
```bash
# Set your username
git config --global user.name "Your Name"

# Set your email
git config --global user.email "your.email@example.com"

# Verify configuration
git config --list
```

## Step 4: Initialize Git Repository
```bash
# Navigate to your project directory
cd G:\firewallblocker

# Initialize Git repository
git init

# Add all files to Git
git add .

# Make first commit
git commit -m "Initial commit: Firewall Blocker application"

# Check status
git status
```

## Step 5: Connect to Remote Repository

### Option A: GitHub (Recommended)
1. Go to https://github.com
2. Sign in or create account
3. Click "New repository"
4. Name it: `firewall-blocker`
5. Make it Public or Private
6. Don't initialize with README (we already have one)
7. Copy the repository URL

### Option B: GitLab
1. Go to https://gitlab.com
2. Create new project
3. Copy the repository URL

### Option C: Azure DevOps
1. Go to your Azure DevOps organization
2. Create new repository
3. Copy the repository URL

## Step 6: Push to Remote Repository
```bash
# Add remote origin (replace URL with your repository URL)
git remote add origin https://github.com/yourusername/firewall-blocker.git

# Push to remote repository
git push -u origin main

# If your default branch is 'master' instead of 'main':
git push -u origin master
```

## Step 7: Verify Push
1. Go to your repository URL
2. Check if all files are uploaded
3. Verify the README.md is displayed

## Common Git Commands

### Daily Workflow
```bash
# Check status
git status

# Add changes
git add .

# Commit changes
git commit -m "Description of changes"

# Push changes
git push

# Pull latest changes
git pull
```

### Branch Management
```bash
# Create new branch
git checkout -b feature-name

# Switch branches
git checkout branch-name

# List branches
git branch

# Merge branch
git merge branch-name
```

## Troubleshooting

### If you get authentication errors:
1. **GitHub**: Use Personal Access Token instead of password
2. **GitLab**: Use Personal Access Token
3. **Azure DevOps**: Use Personal Access Token

### To generate Personal Access Token:
1. Go to your Git provider's settings
2. Look for "Personal Access Tokens" or "Access Tokens"
3. Generate new token with appropriate permissions
4. Use token as password when prompted

### If you need to change remote URL:
```bash
git remote set-url origin https://github.com/yourusername/new-repo-name.git
```

## Repository Structure
Your repository will contain:
```
firewall-blocker/
├── FirewallBlocker.csproj          # Project file
├── app.manifest                    # Application manifest
├── firewall.ico                    # Custom firewall icon
├── Program.cs                      # Main entry point
├── MainForm.cs                     # Windows Forms GUI
├── FirewallManager.cs              # Firewall utilities
├── build.bat                       # Windows build script
├── build.ps1                       # PowerShell build script
├── copy-single-exe.bat             # Copy tool
├── test-single-exe.bat             # Test script
├── verify-icon.bat                 # Icon verification
├── run.bat                         # Quick run script
├── README.md                       # Project documentation
├── GIT_SETUP.md                    # This file
└── publish-clean/                  # Single executable output
    └── FirewallBlocker.exe         # 120MB self-contained app
```

## Next Steps After Setup
1. **Update README.md** with your repository information
2. **Add .gitignore** file for build outputs
3. **Set up GitHub Actions** for automated builds (optional)
4. **Create releases** for each version
5. **Add issues and project boards** for development tracking

## Support
- **Git Documentation**: https://git-scm.com/doc
- **GitHub Help**: https://help.github.com
- **GitLab Documentation**: https://docs.gitlab.com
- **Azure DevOps**: https://docs.microsoft.com/en-us/azure/devops/
