using System;
using System.IO;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;

namespace FirewallBlocker
{
    /// <summary>
    /// Main form for the Firewall Blocker application
    /// Provides installation and uninstallation of context menu integration
    /// </summary>
    public partial class MainForm : Form
    {
        private const string ContextMenuKey = @"HKEY_CLASSES_ROOT\exefile\shell\BlockInFirewall";
        private const string InboundKey = @"HKEY_CLASSES_ROOT\exefile\shell\BlockInFirewall\shell\inbound";
        private const string OutboundKey = @"HKEY_CLASSES_ROOT\exefile\shell\BlockInFirewall\shell\outbound";

        public MainForm()
        {
            InitializeComponent();
            SetApplicationIcon();
            CheckInstallationStatus();
        }

        /// <summary>
        /// Set the application icon from the absolute path
        /// </summary>
        private void SetApplicationIcon()
        {
            try
            {
                string iconPath = @"G:\firewallblocker\firewall.ico";
                if (File.Exists(iconPath))
                {
                    this.Icon = new System.Drawing.Icon(iconPath);
                    // Also set the application icon for the taskbar
                    Application.OpenForms[0].Icon = this.Icon;
                }
            }
            catch (Exception ex)
            {
                // Log the error but continue without custom icon
                System.Diagnostics.Debug.WriteLine($"Failed to load icon: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialize the form components
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "Firewall Blocker - Context Menu Installer";
            this.Size = new System.Drawing.Size(500, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Create title label
            Label titleLabel = new Label
            {
                Text = "Firewall Blocker Context Menu Installer",
                Font = new System.Drawing.Font("Segoe UI", 14, System.Drawing.FontStyle.Bold),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            // Create description label
            Label descLabel = new Label
            {
                Text = "This application integrates with Windows Explorer to add a 'Block in Firewall' context menu option for .exe files.",
                Font = new System.Drawing.Font("Segoe UI", 9),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            // Create status label
            Label statusLabel = new Label
            {
                Name = "statusLabel",
                Text = "Checking installation status...",
                Font = new System.Drawing.Font("Segoe UI", 9),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30
            };

            // Create install button
            Button installButton = new Button
            {
                Name = "installButton",
                Text = "Install Context Menu",
                Font = new System.Drawing.Font("Segoe UI", 10),
                Size = new System.Drawing.Size(150, 40),
                Location = new System.Drawing.Point(50, 180)
            };
            installButton.Click += InstallButton_Click;

            // Create uninstall button
            Button uninstallButton = new Button
            {
                Name = "uninstallButton",
                Text = "Uninstall Context Menu",
                Font = new System.Drawing.Font("Segoe UI", 10),
                Size = new System.Drawing.Size(150, 40),
                Location = new System.Drawing.Point(250, 180)
            };
            uninstallButton.Click += UninstallButton_Click;

            // Create info text box
            TextBox infoTextBox = new TextBox
            {
                Name = "infoTextBox",
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Size = new System.Drawing.Size(450, 80),
                Location = new System.Drawing.Point(25, 240),
                Text = "After installation, right-click on any .exe file in Windows Explorer to see the 'Block in Firewall' option with Inbound/Outbound submenu options."
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] 
            { 
                titleLabel, 
                descLabel, 
                statusLabel, 
                installButton, 
                uninstallButton, 
                infoTextBox 
            });
        }

        /// <summary>
        /// Check if the context menu is already installed
        /// </summary>
        private void CheckInstallationStatus()
        {
            bool isInstalled = IsContextMenuInstalled();
            bool isAdmin = IsRunningAsAdministrator();

            Label statusLabel = (Label)Controls["statusLabel"];
            Button installButton = (Button)Controls["installButton"];
            Button uninstallButton = (Button)Controls["uninstallButton"];

            if (isInstalled)
            {
                statusLabel.Text = "✓ Context menu is installed";
                statusLabel.ForeColor = System.Drawing.Color.Green;
                installButton.Enabled = false;
                uninstallButton.Enabled = true;
            }
            else
            {
                statusLabel.Text = "✗ Context menu is not installed";
                statusLabel.ForeColor = System.Drawing.Color.Red;
                installButton.Enabled = true;
                uninstallButton.Enabled = false;
            }

            if (!isAdmin)
            {
                statusLabel.Text += " (Run as Administrator required)";
                statusLabel.ForeColor = System.Drawing.Color.Orange;
                installButton.Enabled = false;
                uninstallButton.Enabled = false;
            }
        }

        /// <summary>
        /// Check if the context menu is currently installed
        /// </summary>
        /// <returns>True if installed, false otherwise</returns>
        private bool IsContextMenuInstalled()
        {
            try
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"exefile\shell\BlockInFirewall"))
                {
                    return key != null;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the application is running with administrator privileges
        /// </summary>
        /// <returns>True if running as administrator, false otherwise</returns>
        private bool IsRunningAsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// Install the context menu integration
        /// </summary>
        private void InstallButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRunningAsAdministrator())
                {
                    MessageBox.Show("This application must be run as Administrator to install the context menu.", 
                        "Administrator Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the path to the current executable
                string exePath = Application.ExecutablePath;

                // Create the main context menu key
                using (RegistryKey mainKey = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\BlockInFirewall"))
                {
                    mainKey.SetValue("MUIVerb", "Block in Firewall");
                    mainKey.SetValue("SubCommands", "");
                }

                // Create the inbound submenu
                using (RegistryKey inboundKey = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\BlockInFirewall\shell\inbound"))
                {
                    inboundKey.SetValue("MUIVerb", "Inbound");
                    inboundKey.SetValue("", "Block Inbound Connections");
                }

                // Create the inbound command
                using (RegistryKey inboundCommand = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\BlockInFirewall\shell\inbound\command"))
                {
                    inboundCommand.SetValue("", $"\"{exePath}\" \"%1\" in");
                }

                // Create the outbound submenu
                using (RegistryKey outboundKey = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\BlockInFirewall\shell\outbound"))
                {
                    outboundKey.SetValue("MUIVerb", "Outbound");
                    outboundKey.SetValue("", "Block Outbound Connections");
                }

                // Create the outbound command
                using (RegistryKey outboundCommand = Registry.ClassesRoot.CreateSubKey(@"exefile\shell\BlockInFirewall\shell\outbound\command"))
                {
                    outboundCommand.SetValue("", $"\"{exePath}\" \"%1\" out");
                }

                MessageBox.Show("Context menu integration installed successfully!\n\n" +
                    "Right-click on any .exe file in Windows Explorer to see the 'Block in Firewall' option.", 
                    "Installation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CheckInstallationStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error installing context menu: {ex.Message}", 
                    "Installation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Uninstall the context menu integration
        /// </summary>
        private void UninstallButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsRunningAsAdministrator())
                {
                    MessageBox.Show("This application must be run as Administrator to uninstall the context menu.", 
                        "Administrator Required", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Remove the entire BlockInFirewall key and all subkeys
                Registry.ClassesRoot.DeleteSubKeyTree(@"exefile\shell\BlockInFirewall", false);

                MessageBox.Show("Context menu integration uninstalled successfully!", 
                    "Uninstallation Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CheckInstallationStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uninstalling context menu: {ex.Message}", 
                    "Uninstallation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

