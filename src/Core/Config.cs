using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace _2fa
{
	internal class Config
	{
		public string PasswordHash { get; set; }
		public List<Entry> Entries { get; set; } = new List<Entry>();

		private static JsonSerializerOptions JsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			TypeInfoResolver = ConfigJsonContext.Default,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		};

		[JsonIgnore]
		public static string FilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".2fa-cli.json");

		public static Config Read()
		{
			return JsonSerializer.Deserialize<Config>(File.ReadAllText(FilePath), JsonOptions);
		}

		public void Write()
		{
			File.WriteAllText(FilePath, JsonSerializer.Serialize(this, JsonOptions));
		}

		public static void SetPermission()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				FileInfo fileInfo = new FileInfo(FilePath);
				FileSecurity fileSecurity = fileInfo.GetAccessControl();

				fileSecurity.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
				var currentRules = fileSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
				foreach (FileSystemAccessRule rule in currentRules)
					fileSecurity.RemoveAccessRule(rule);

				fileSecurity.AddAccessRule(new FileSystemAccessRule(System.Security.Principal.WindowsIdentity.GetCurrent().Name, FileSystemRights.FullControl, AccessControlType.Allow));
				fileInfo.SetAccessControl(fileSecurity);
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				string chmodCommand = $"chmod 600 {FilePath}";
				ProcessStartInfo processInfo = new ProcessStartInfo("bash", $"-c \"{chmodCommand}\"")
				{
					RedirectStandardOutput = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				using Process process = Process.Start(processInfo);
				process.WaitForExit();
			}
		}
	}
}
