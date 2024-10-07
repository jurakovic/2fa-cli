using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2fa
{
	internal class AddCommand : Command
	{
		public AddCommand() : base("add", "Adds a new 2FA entry")
		{
			var secretArgument = new Argument<string>(
				name: "secret",
				description: "Secret key");

			var nameArgument = new Argument<string>(
				name: "name",
				description: "Name");

			var typeOption = new Option<string>(
				name: "--type",
				description: "OTP type",
				getDefaultValue: () => "totp");
			typeOption.FromAmong("totp", "hotp");
			typeOption.AddAlias("-t");

			var lengthOption = new Option<int>(
				name: "--length",
				description: "OTP length",
				getDefaultValue: () => 6);
			lengthOption.FromAmong(Enumerable.Range(4, 5).Select(x => x.ToString()).ToArray());
			lengthOption.AddAlias("-l");

			this.Add(nameArgument);
			this.Add(secretArgument);
			this.Add(typeOption);
			this.Add(lengthOption);
			this.SetHandler(ExecuteAsync, nameArgument, secretArgument, typeOption, lengthOption);
		}

		private Task ExecuteAsync(string name, string secret, string type, int length)
		{
			Console.WriteLine($"name     = {name}");
			Console.WriteLine($"secret   = {secret}");
			Console.WriteLine($"--type   = {type}");
			Console.WriteLine($"--length = {length}");

			Config config;
			string password;

			string userPath = Environment.GetEnvironmentVariable("USERPROFILE");
			string file = Path.Combine(userPath, ".2fa-cli.json");

			if (File.Exists(file))
			{
				string text = File.ReadAllText(file);
				config = JsonSerializer.Deserialize<Config>(text);

				Console.WriteLine("Enter password: ");
				password = Console.ReadLine();

				bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, config.PasswordHash);

				if (!isPasswordValid)
				{
					Console.WriteLine("Wrong password");
					// todo: exit 1
				}

				//Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);
				//var decry = AesOperation.DecryptString(password, entry.SecretEncrypted);
			}
			else
			{
				Console.WriteLine("Missing password");
				Console.WriteLine("Enter new password: ");
				password = Console.ReadLine();

				config = new Config();
				config.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

				File.WriteAllText(file, JsonSerializer.Serialize(config, new JsonSerializerOptions
				{
					WriteIndented = true
				}));
			}

			Entry newEntry = new Entry()
			{
				Name = name,
				SecretEncrypted = AesOperation.EncryptString(password, secret),
				Type = EntryType.Totp, // todo
				Size = length,
			};

			if (config.Entries == null)
			{
				config.Entries = new List<Entry>() { newEntry };
			}
			else
			{
				config.Entries.Add(newEntry);
			}

			File.WriteAllText(file, JsonSerializer.Serialize(config, new JsonSerializerOptions
			{
				WriteIndented = true
			}));
			return Task.CompletedTask;
		}
	}
}
