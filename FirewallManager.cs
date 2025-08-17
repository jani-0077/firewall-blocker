using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FirewallBlocker
{
    /// <summary>
    /// Manages Windows Firewall rules using netsh commands
    /// Provides methods for creating, removing, and checking firewall rules
    /// </summary>
    public static class FirewallManager
    {
        /// <summary>
        /// Creates a Windows Firewall rule to block connections for a specific executable
        /// </summary>
        /// <param name="filePath">Path to the executable file</param>
        /// <param name="direction">Direction: "in" for inbound, "out" for outbound</param>
        /// <param name="ruleName">Name for the firewall rule</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool CreateBlockRule(string filePath, string direction, string ruleName)
        {
            try
            {
                // Validate parameters
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new ArgumentException("Invalid file path provided");
                }

                if (string.IsNullOrEmpty(direction) || (direction != "in" && direction != "out"))
                {
                    throw new ArgumentException("Direction must be 'in' or 'out'");
                }

                if (string.IsNullOrEmpty(ruleName))
                {
                    throw new ArgumentException("Rule name cannot be empty");
                }

                // Build the netsh command
                string command = $"netsh advfirewall firewall add rule name=\"{ruleName}\" dir={direction} program=\"{filePath}\" action=block enable=yes";
                
                // Execute the command
                return ExecuteNetshCommand(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create firewall rule: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Removes a Windows Firewall rule by name
        /// </summary>
        /// <param name="ruleName">Name of the rule to remove</param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RemoveRule(string ruleName)
        {
            try
            {
                if (string.IsNullOrEmpty(ruleName))
                {
                    throw new ArgumentException("Rule name cannot be empty");
                }

                string command = $"netsh advfirewall firewall delete rule name=\"{ruleName}\"";
                return ExecuteNetshCommand(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to remove firewall rule: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a firewall rule exists
        /// </summary>
        /// <param name="ruleName">Name of the rule to check</param>
        /// <returns>True if rule exists, false otherwise</returns>
        public static bool RuleExists(string ruleName)
        {
            try
            {
                if (string.IsNullOrEmpty(ruleName))
                {
                    return false;
                }

                string command = $"netsh advfirewall firewall show rule name=\"{ruleName}\"";
                string output = ExecuteNetshCommandWithOutput(command);
                
                // Check if the output contains rule information
                return !string.IsNullOrEmpty(output) && !output.Contains("No rules match the specified criteria");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Lists all firewall rules
        /// </summary>
        /// <returns>String containing all firewall rules</returns>
        public static string ListAllRules()
        {
            try
            {
                string command = "netsh advfirewall firewall show rule";
                return ExecuteNetshCommandWithOutput(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to list firewall rules: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lists firewall rules for a specific executable
        /// </summary>
        /// <param name="filePath">Path to the executable file</param>
        /// <returns>String containing matching firewall rules</returns>
        public static string ListRulesForExecutable(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new ArgumentException("Invalid file path provided");
                }

                string command = $"netsh advfirewall firewall show rule program=\"{filePath}\"";
                return ExecuteNetshCommandWithOutput(command);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to list rules for executable: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Executes a netsh command and returns success/failure
        /// </summary>
        /// <param name="command">The netsh command to execute</param>
        /// <returns>True if successful, false otherwise</returns>
        private static bool ExecuteNetshCommand(string command)
        {
            try
            {
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

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes a netsh command and returns the output
        /// </summary>
        /// <param name="command">The netsh command to execute</param>
        /// <returns>Output from the command</returns>
        private static string ExecuteNetshCommandWithOutput(string command)
        {
            try
            {
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

                using (Process process = Process.Start(startInfo))
                {
                    process.WaitForExit();
                    
                    if (process.ExitCode != 0)
                    {
                        string errorOutput = process.StandardError.ReadToEnd();
                        throw new Exception($"Command failed with exit code {process.ExitCode}: {errorOutput}");
                    }

                    return process.StandardOutput.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to execute netsh command: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generates a unique rule name for firewall rules
        /// </summary>
        /// <param name="fileName">Name of the executable file</param>
        /// <param name="direction">Direction: "in" for inbound, "out" for outbound</param>
        /// <returns>Unique rule name</returns>
        public static string GenerateRuleName(string fileName, string direction)
        {
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return $"Block_{direction}_{baseName}_{timestamp}";
        }

        /// <summary>
        /// Validates if the current user has administrator privileges
        /// </summary>
        /// <returns>True if running as administrator, false otherwise</returns>
        public static bool IsRunningAsAdministrator()
        {
            try
            {
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }
    }
}

