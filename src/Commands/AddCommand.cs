﻿using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fa
{
	internal class AddCommand : Command
	{
		public AddCommand()
			: base("add", "Adds a new 2FA entry")
		{
			var nameArgument = new Argument<string>("name", "Name");
			var secretArgument = new Argument<string>("secret", "Secret key");

			var typeOption = new Option<string>("--type", () => EntryType.Totp, "OTP type");
			typeOption.FromAmong(EntryType.Totp, EntryType.Hotp);
			typeOption.AddAlias("-t");

			var sizeOption = new Option<int>("--size", () => 6, "OTP size");
			sizeOption.FromAmong(Enumerable.Range(4, 5).Select(x => x.ToString()).ToArray());
			sizeOption.AddAlias("-s");

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

			secret = secret.PadRight(16, '=');
			if (!EntryHelper.IsValidSecret(secret))
			{
				Console.WriteLine("Invalid secret");
				return Task.FromResult(1);
			}

			if (!File.Exists(Config.Path))
			{
				Console.Write("First entry. Enter new password: ");
				password = ConsoleHelper.ReadPassword();
				Console.Write("Confirm password: ");
				string confirm = ConsoleHelper.ReadPassword();

				ConsoleHelper.ClearLine();
				ConsoleHelper.ClearLine();

				if (password == confirm)
				{
					config = new Config();
					config.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
					config.Write();
				}
				else
				{
					Console.WriteLine("Passwords don't match");
					return Task.FromResult(1);
				}
			}
			else
			{
				config = Config.Read();
				password = Environment.GetEnvironmentVariable("_2FA_CLI_PASSWORD");

				if (String.IsNullOrWhiteSpace(password))
				{
					Console.Write("Enter password: ");
					password = ConsoleHelper.ReadPassword();
					ConsoleHelper.ClearLine();
				}

				if (!BCrypt.Net.BCrypt.Verify(password, config.PasswordHash))
				{
					Console.WriteLine("Wrong password");
					return Task.FromResult(1);
				}
			}

			Entry entry = config.Entries.SingleOrDefault(x => x.Name == name);

			if (entry != null)
			{
				Console.WriteLine($"Entry '{name}' already exists.");
				return Task.FromResult(1);
			}
			else
			{
				Entry newEntry = new Entry()
				{
					Name = name,
					Secret = Aes.EncryptString(password, secret),
					Type = type,
					Size = size,
				};

				config.Entries.Add(newEntry);
			}

			config.Write();
			Console.WriteLine($"OK");
			return Task.CompletedTask;
		}
	}
}
