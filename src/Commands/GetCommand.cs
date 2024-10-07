using OtpNet;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2fa
{
	internal class GetCommand : Command
	{
		public GetCommand() : base("get", "Gets new OTP")
		{
			var nameArgument = new Argument<string>(
				name: "name",
				description: "Name");

			this.Add(nameArgument);
			this.SetHandler(ExecuteAsync, nameArgument);
		}

		private Task ExecuteAsync(string name)
		{
			string userPath = Environment.GetEnvironmentVariable("USERPROFILE");
			string file = Path.Combine(userPath, ".2fa-cli.json");

			if (!File.Exists(file))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Console.Write("Enter password: ");
				string password = ConsoleHelper.ReadPassword();
				ConsoleHelper.ClearLine();

				string text = File.ReadAllText(file);
				Config config = JsonSerializer.Deserialize<Config>(text);

				if (BCrypt.Net.BCrypt.Verify(password, config.PasswordHash))
				{
					Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);

					if (entry != null)
					{
						string secretKey = Aes.DecryptString(password, entry.Secret);
						var totp = new Totp(Base32Encoding.ToBytes(secretKey));
						Console.WriteLine(totp.ComputeTotp());
					}
					else
					{
						Console.WriteLine($"No '{name}' entry");
					}
				}
				else
				{
					Console.WriteLine($"Wrong password");
					return Task.FromResult(1);
				}
			}

			return Task.CompletedTask;
		}
	}
}
