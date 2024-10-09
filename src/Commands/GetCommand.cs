using OtpNet;
using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TextCopy;

namespace _2fa
{
	internal class GetCommand : Command
	{
		public GetCommand()
			: base("get", "Gets the current OTP code for a specified entry")
		{
			var serviceArgument = new Argument<string>("service", "The name of the organization or service provider");

			var noClipboardOption = new Option<bool>("--no-clipboard", () => false, "Disable copying to clipboard");
			noClipboardOption.AddAlias("-nc");

			this.Add(serviceArgument);
			this.Add(noClipboardOption);
			this.SetHandler(ExecuteAsync, serviceArgument, noClipboardOption);
		}

		private Task ExecuteAsync(string service, bool noClipboard)
		{
			if (!File.Exists(ConfigHelper.FilePath))
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

				Config config = ConfigHelper.Read();

				if (BCrypt.Net.BCrypt.Verify(password, config.PasswordHash))
				{
					Entry entry = config.Entries.SingleOrDefault(x => x.Service.ToUpper() == service.ToUpper());

					if (entry != null)
					{
						string secretKey = Aes.DecryptString(password, entry.SecretKey);
						string otp;

						if (entry.Type == EntryType.Totp)
						{
							var totp = new Totp(Base32Encoding.ToBytes(secretKey), totpSize: entry.Digits);
							otp = totp.ComputeTotp();
						}
						else
						{
							var hotp = new Hotp(Base32Encoding.ToBytes(secretKey), hotpSize: entry.Digits);
							otp = hotp.ComputeHOTP(entry.Counter);
							entry.Counter++;
							ConfigHelper.Write(config);
						}

						Console.WriteLine(otp);

						if (!noClipboard)
							ClipboardService.SetText(otp);
					}
					else
					{
						Console.WriteLine($"No '{service}' entry");
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
