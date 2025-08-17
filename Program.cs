using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FirewallBlocker
{
    /// <summary>
    /// Main program class that handles both GUI mode and command line mode
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Check if running from context menu (with file path argument)
            if (args.Length > 0 && File.Exists(args[0]))
            {
                string filePath = args[0];
                string direction = args.Length > 1 ? args[1] : string.Empty;
                
                // Handle context menu action
                HandleContextMenuAction(filePath, direction);
                return;
            }

            // Run in GUI mode for installation/configuration
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Handles the context menu action when user selects "Block in Firewall"
        /// </summary>
        /// <param name="filePath">Path to the executable file</param>
        /// <param name="direction">Direction: "in" for inbound, "out" for outbound</param>
        private static void HandleContextMenuAction(string filePath, string direction)
        {
            try
            {
                string fileName = Path.GetFileName(filePath);
                
                if (direction.Equals("in", StringComparison.OrdinalIgnoreCase))
                {
                    // Create inbound firewall rule
                    CreateFirewallRule(filePath, "in", $"Block Inbound {fileName}");
                    MessageBox.Show($"Successfully blocked inbound connections for {fileName}", 
                        "Firewall Rule Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (direction.Equals("out", StringComparison.OrdinalIgnoreCase))
                {
                    // Create outbound firewall rule
                    CreateFirewallRule(filePath, "out", $"Block Outbound {fileName}");
                    MessageBox.Show($"Successfully blocked outbound connections for {fileName}", 
                        "Firewall Rule Created", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Invalid direction specified", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating firewall rule: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Creates a Windows Firewall rule using netsh command
        /// </summary>
        /// <param name="filePath">Path to the executable file</param>
        /// <param name="direction">Direction: "in" for inbound, "out" for outbound</param>
        /// <param name="ruleName">Name for the firewall rule</param>
        private static void CreateFirewallRule(string filePath, string direction, string ruleName)
        {
            // Build the netsh command
            string command = $"netsh advfirewall firewall add rule name=\"{ruleName}\" dir={direction} program=\"{filePath}\" action=block";
            
            // Create process start info
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Verb = "runas" // Run as administrator
            };

            // Execute the command
            using (Process process = Process.Start(startInfo))
            {
                process.WaitForExit();
                
                if (process.ExitCode != 0)
                {
                    string errorOutput = process.StandardError.ReadToEnd();
                    throw new Exception($"Failed to create firewall rule. Exit code: {process.ExitCode}. Error: {errorOutput}");
                }
            }
        }
    }
}

