using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OtpNet;

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
				// todo: exit 1
			}
			else
			{
				Console.WriteLine("Enter password: ");
				string password = Console.ReadLine();

				string text = File.ReadAllText(file);
				Config config = JsonSerializer.Deserialize<Config>(text);

				var tt = SHA256.HashData(Encoding.UTF8.GetBytes(password));
				string passhash = Convert.ToBase64String(tt);

				if (passhash == config.PasswordHash)
				{
					Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);

					if (entry != null)
					{
						string secretKey = AesOperation.DecryptString(password, entry.SecretEncrypted);
						var totp = new Totp(Base32Encoding.ToBytes(secretKey));
						Console.WriteLine(totp.ComputeTotp());
					}
					else
					{
						Console.WriteLine($"No '{name}' entry");
					}
				}

			}

			return Task.CompletedTask;
		}
	}
}
