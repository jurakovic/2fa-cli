using System;
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
			var serviceArgument = new Argument<string>("service", "The name of the organization or service provider");

			var secretArgument = new Argument<string>("secret-key", "The base32-encoded secret key used to generate the OTP");

			var typeOption = new Option<string>("--type", () => EntryType.Totp, "The type of OTP");
			typeOption.FromAmong(EntryType.Totp, EntryType.Hotp);
			typeOption.AddAlias("-t");

			var digitsOption = new Option<int>("--digits", () => 6, "The number of digits in the OTP code");
			digitsOption.FromAmong("6", "8");
			digitsOption.AddAlias("-d");

			this.Add(serviceArgument);
			this.Add(secretArgument);
			this.Add(typeOption);
			this.Add(digitsOption);
			this.SetHandler(ExecuteAsync, serviceArgument, secretArgument, typeOption, digitsOption);
		}

		private Task ExecuteAsync(string service, string secret, string type, int digits)
		{
			Config config;
			string password;

			secret = secret.PadRight(16, '=');
			if (!EntryHelper.IsValidSecret(secret))
			{
				Console.WriteLine("Invalid secret");
				return Task.FromResult(1);
			}

			if (!File.Exists(Config.FilePath))
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
					Config.SetPermission();
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

			Entry entry = config.Entries.SingleOrDefault(x => x.Service.ToUpper() == service.ToUpper());

			if (entry != null)
			{
				Console.WriteLine($"Entry '{service}' already exists.");
				return Task.FromResult(1);
			}
			else
			{
				entry = new Entry()
				{
					Service = service,
					SecretKey = Aes.EncryptString(password, secret),
					Type = type,
					Digits = digits,
				};

				config.Entries.Add(entry);
			}

			config.Write();
			Console.WriteLine($"Entry '{service}' added.");
			return Task.CompletedTask;
		}
	}
}
