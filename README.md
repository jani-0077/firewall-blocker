# Firewall Blocker - Windows Context Menu Integration

A Windows desktop application that integrates with Windows Explorer to add a "Block in Firewall" context menu option for `.exe` files. This allows users to quickly create Windows Firewall rules to block inbound or outbound connections for any executable file.

## Features

- **Context Menu Integration**: Adds "Block in Firewall" option to right-click context menu for `.exe` files
- **Dual Direction Support**: Choose between blocking inbound or outbound connections
- **Automatic Firewall Rules**: Creates Windows Firewall rules using `netsh advfirewall` commands
- **Administrator Privileges**: Handles UAC elevation automatically for firewall operations
- **Easy Installation**: Simple GUI installer for context menu integration
- **Clean Uninstallation**: Removes all registry entries when uninstalling
- **Single Executable**: Self-contained application with embedded .NET runtime

## Requirements

- Windows 10/11 (64-bit)
- Administrator privileges for firewall operations
- **No .NET installation required** (when using single executable)

## Building the Application

### Prerequisites
- .NET 9.0 SDK installed on your system

### Build Commands

#### Option 1: Using the provided scripts (Recommended)
```bash
# Windows Batch - Choose build type
build.bat

# PowerShell - Choose build type
.\build.ps1
```

**Build Options:**
- **Option 1**: Regular build (multiple files, smaller size)
- **Option 2**: Single executable (self-contained, portable)

#### Option 2: Using .NET CLI directly
```bash
# Regular build
dotnet build --configuration Release

# Single executable (self-contained)
dotnet publish --configuration Release --output publish-clean
```

#### Option 3: Using MSBuild
```bash
msbuild FirewallBlocker.csproj /p:Configuration=Release
```

### Build Output

#### Regular Build
```
bin\Release\net9.0-windows\FirewallBlocker.exe
```

#### Single Executable (Recommended for Distribution)
```
publish-clean\FirewallBlocker.exe (120MB)
```

**Single Executable Benefits:**
- ✅ **Portable**: Copy to any Windows machine
- ✅ **No Dependencies**: .NET runtime embedded
- ✅ **Easy Distribution**: Single file to share
- ✅ **Custom Icon**: Firewall icon embedded
- ✅ **Administrator Manifest**: UAC elevation built-in

## Installation and Usage

### Step 1: Install Context Menu Integration
1. **Run as Administrator**: Right-click `FirewallBlocker.exe` and select "Run as administrator"
2. **Install Context Menu**: Click the "Install Context Menu" button
3. **Confirmation**: You'll see a success message when installation is complete

### Step 2: Use the Context Menu
1. **Navigate to any `.exe` file** in Windows Explorer
2. **Right-click** on the executable file
3. **Select "Block in Firewall"** from the context menu
4. **Choose direction**:
   - **Inbound**: Blocks incoming connections to the application
   - **Outbound**: Blocks outgoing connections from the application

### Step 3: Firewall Rule Creation
- The application automatically creates a Windows Firewall rule
- Rule name format: `Block [Direction] [Filename]`
- Rules are enabled immediately and take effect right away

## How It Works

### Context Menu Registration
The application modifies the Windows Registry to add the context menu entry:
```
HKEY_CLASSES_ROOT\exefile\shell\BlockInFirewall\
├── shell\inbound\command
└── shell\outbound\command
```

### Firewall Rule Creation
Uses `netsh advfirewall` commands to create rules:
```bash
# Inbound rule
netsh advfirewall firewall add rule name="Block Inbound [FILENAME]" dir=in program="C:\Path\to\file.exe" action=block

# Outbound rule
netsh advfirewall firewall add rule name="Block Outbound [FILENAME]" dir=out program="C:\Path\to\file.exe" action=block
```

### Command Line Mode
The application can also run in command-line mode when invoked from the context menu:
```bash
FirewallBlocker.exe "C:\Path\to\file.exe" in   # Block inbound
FirewallBlocker.exe "C:\Path\to\file.exe" out  # Block outbound
```

## Project Structure

```
FirewallBlocker/
├── FirewallBlocker.csproj          # Project file with single file config
├── app.manifest                    # Application manifest (requires admin)
├── firewall.ico                    # Custom firewall icon
├── Program.cs                      # Main entry point and command-line handler
├── MainForm.cs                     # Windows Forms GUI for installation
├── FirewallManager.cs              # Firewall rule management utilities
├── build.bat                       # Windows batch build script (with choices)
├── build.ps1                       # PowerShell build script (with choices)
├── copy-single-exe.bat             # Copy single executable tool
├── run.bat                         # Quick run script
└── README.md                       # This file
```

## Distribution

### Single Executable Distribution
The single executable (`publish-clean\FirewallBlocker.exe`) is perfect for distribution:

1. **Copy to any Windows machine** - No .NET installation required
2. **Run as administrator** - Right-click → "Run as administrator"
3. **Install context menu** - One-click installation
4. **Use immediately** - Right-click any .exe file

### Copy Tool
Use `copy-single-exe.bat` to easily copy the single executable to your desired location:
```bash
copy-single-exe.bat
```

## Technical Details

### Architecture
- **Windows Forms Application**: Modern .NET 9.0 Windows Forms UI
- **Registry Integration**: Uses `Microsoft.Win32.Registry` for context menu setup
- **Process Management**: Executes `netsh` commands with proper error handling
- **Administrator Privileges**: UAC manifest ensures elevated permissions
- **Single File Publishing**: Self-contained with embedded runtime

### Single File Configuration
```xml
<PublishSingleFile>true</PublishSingleFile>
<SelfContained>true</SelfContained>
<RuntimeIdentifier>win-x64</RuntimeIdentifier>
<PublishReadyToRun>true</PublishReadyToRun>
```

### Security Features
- **Input Validation**: Validates file paths and parameters
- **Error Handling**: Comprehensive exception handling and user feedback
- **Process Isolation**: Secure execution of firewall commands
- **Registry Safety**: Safe registry operations with proper cleanup

## Troubleshooting

### Common Issues

#### "Administrator Required" Error
- **Solution**: Right-click the executable and select "Run as administrator"
- **Reason**: Firewall operations require elevated privileges

#### Context Menu Not Appearing
- **Solution**: Ensure the application was run as administrator during installation
- **Check**: Verify registry keys exist under `HKEY_CLASSES_ROOT\exefile\shell\BlockInFirewall`

#### Firewall Rule Creation Fails
- **Solution**: Check Windows Firewall service is running
- **Verify**: Ensure you have permission to modify firewall rules
- **Logs**: Check Windows Event Viewer for firewall-related errors

#### Build Errors
- **Missing .NET SDK**: Install .NET 9.0 SDK from Microsoft
- **Icon File Missing**: The project has been updated to include the icon file

### Registry Cleanup
If manual cleanup is needed, remove these registry keys:
```
HKEY_CLASSES_ROOT\exefile\shell\BlockInFirewall
```

## Uninstallation

### Using the Application
1. Run `FirewallBlocker.exe` as administrator
2. Click "Uninstall Context Menu"
3. Confirm the uninstallation

### Manual Uninstallation
1. Open Registry Editor as administrator
2. Navigate to `HKEY_CLASSES_ROOT\exefile\shell\`
3. Delete the `BlockInFirewall` key and all subkeys

## Development

### Prerequisites
- Visual Studio 2022 or Visual Studio Code
- .NET 9.0 SDK
- Windows Forms development workload

### Building from Source
```bash
git clone <repository-url>
cd FirewallBlocker
dotnet restore
dotnet build --configuration Release
```

### Publishing Single Executable
```bash
dotnet publish --configuration Release --output publish-clean
```

### Testing
- Test context menu integration with various executable files
- Verify firewall rule creation in Windows Firewall with Advanced Security
- Test both inbound and outbound blocking scenarios
- Test single executable on machines without .NET

## License

This project is provided as-is for educational and personal use. Use at your own risk.

## Contributing

Contributions are welcome! Please ensure:
- Code follows C# best practices
- Proper error handling is implemented
- Security considerations are addressed
- Documentation is updated

## Support

For issues or questions:
1. Check the troubleshooting section above
2. Verify Windows Firewall service is running
3. Ensure administrator privileges are available
4. Check Windows Event Viewer for error logs

---

**Note**: This application modifies Windows Registry and Firewall settings. Always run as administrator and use with caution in production environments.

**Single Executable Advantage**: The self-contained version (120MB) includes everything needed to run on any Windows 10/11 machine without requiring .NET installation.
