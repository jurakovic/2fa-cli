using OtpNet;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _2fa
{
	internal class GetCommand : Command
	{
		JsonSerializerOptions jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		};

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
			string file = Config.Path;

			if (!File.Exists(file))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				string password = Environment.GetEnvironmentVariable("_2FA_CLI_PASSWORD");

				if (String.IsNullOrWhiteSpace(password))
				{
					Console.Write("Enter password: ");
					password = ConsoleHelper.ReadPassword();
					ConsoleHelper.ClearLine();
				}

				Config config = Config.Read();

				if (BCrypt.Net.BCrypt.Verify(password, config.PasswordHash))
				{
					Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);

					if (entry != null)
					{
						string secretKey = Aes.DecryptString(password, entry.Secret);
						string otp;

						if (entry.Type == EntryType.Totp)
						{
							var totp = new Totp(Base32Encoding.ToBytes(secretKey), totpSize: entry.Size);
							otp = totp.ComputeTotp();
						}
						else
						{
							var hotp = new Hotp(Base32Encoding.ToBytes(secretKey), hotpSize: entry.Size);
							otp = hotp.ComputeHOTP(entry.Counter);
							entry.Counter++;
							File.WriteAllText(file, JsonSerializer.Serialize(config, jsonOptions));
						}

						Console.WriteLine(otp);
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
