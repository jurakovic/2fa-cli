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
		JsonSerializerOptions jsonOptions = new JsonSerializerOptions { WriteIndented = true };

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

			var sizeOption = new Option<int>(
				name: "--size",
				description: "OTP size",
				getDefaultValue: () => 6);
			sizeOption.FromAmong(Enumerable.Range(4, 5).Select(x => x.ToString()).ToArray());
			sizeOption.AddAlias("-l");

			this.Add(nameArgument);
			this.Add(secretArgument);
			this.Add(typeOption);
			this.Add(sizeOption);
			this.SetHandler(ExecuteAsync, nameArgument, secretArgument, typeOption, sizeOption);
		}

		private Task ExecuteAsync(string name, string secret, string type, int size)
		{
			Config config;
			string password;

			string userPath = Environment.GetEnvironmentVariable("USERPROFILE");
			string file = Path.Combine(userPath, ".2fa-cli.json");

			if (File.Exists(file))
			{
				config = JsonSerializer.Deserialize<Config>(File.ReadAllText(file));

				Console.Write("Enter password: ");
				password = ConsoleHelper.GetConsolePassword();
				ConsoleHelper.ClearLine();

				if (!BCrypt.Net.BCrypt.Verify(password, config.PasswordHash))
					Console.WriteLine("Wrong password");
			}
			else
			{
				Console.Write("First entry. Enter new password: ");
				password = ConsoleHelper.GetConsolePassword();
				Console.Write("Confirm password: ");
				string confirm = ConsoleHelper.GetConsolePassword();

				ConsoleHelper.ClearLine();
				ConsoleHelper.ClearLine();

				if (password == confirm)
				{
					config = new Config();
					config.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);

					File.WriteAllText(file, JsonSerializer.Serialize(config, jsonOptions));
				}
				else
				{
					Console.WriteLine("Passwords don't match");
					return Task.CompletedTask;
				}
			}

			Entry newEntry = new Entry()
			{
				Name = name,
				Secret = Aes.EncryptString(password, secret.PadRight(16, '=')),
				Type = EntryType.Totp, // todo
				Size = size,
			};

			if (config.Entries == null)
			{
				config.Entries = new List<Entry>() { newEntry };
			}
			else
			{
				Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);

				if (entry == null)
				{
					config.Entries.Add(newEntry);
				}
				else
				{
					Console.WriteLine($"'{name}' already exists.");
					return Task.CompletedTask;
				}
			}

			File.WriteAllText(file, JsonSerializer.Serialize(config, jsonOptions));
			Console.WriteLine($"OK");
			return Task.CompletedTask;
		}
	}
}
