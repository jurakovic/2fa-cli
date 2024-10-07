using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using System;
using System.Collections.Generic;
//using System.CommandLine;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2fa
{
	internal class Program
	{
		public static async Task<int> Main()
		{
			return await new CliApplicationBuilder()
					.AddCommandsFromThisAssembly()
					.Build()
					.RunAsync();
		}


		//static async Task Main(string[] args)
		//{
		//	var rootCommand = new RootCommand("2FA CLI tool");
		//
		//	var addCommand = new Command("add", "Add new entry");
		//
		//	var typeOption = new Option<string>(
		//		name: "--type",
		//		description: "OTP type",
		//		getDefaultValue: () => "totp");
		//	typeOption.FromAmong("totp", "hotp");
		//	typeOption.AddAlias("-t");
		//
		//	var sizeOption = new Option<int>(
		//		name: "--length",
		//		description: "OTP length",
		//		getDefaultValue: () => 6);
		//	sizeOption.FromAmong(Enumerable.Range(4, 5).Select(x => x.ToString()).ToArray());
		//	sizeOption.AddAlias("-l");
		//
		//	var secretArgument = new Argument<string>(
		//		name: "secret",
		//		description: "Secret key");
		//
		//	var nameArgument = new Argument<string>(
		//		name: "name",
		//		description: "Name");
		//
		//	addCommand.Add(nameArgument);
		//	addCommand.Add(secretArgument);
		//	addCommand.Add(typeOption);
		//	addCommand.Add(sizeOption);
		//
		//	addCommand.SetHandler((name, secret, type, size) =>
		//	{
		//		Console.WriteLine($"name   = {name}");
		//		Console.WriteLine($"secret = {secret}");
		//		Console.WriteLine($"--type = {type}");
		//		Console.WriteLine($"--size = {size}");
		//
		//		Config config;
		//		string password;
		//
		//		string userPath = Environment.GetEnvironmentVariable("USERPROFILE");
		//		string file = Path.Combine(userPath, ".2fa-cli.json");
		//		if (File.Exists(file))
		//		{
		//			string text = File.ReadAllText(file);
		//			config = JsonSerializer.Deserialize<Config>(text);
		//
		//			Console.WriteLine("Enter password: ");
		//			password = Console.ReadLine();
		//
		//			var tt = SHA256.HashData(Encoding.UTF8.GetBytes(password));
		//			string passhash = Convert.ToBase64String(tt);
		//
		//			if (passhash != config.PasswordHash)
		//			{
		//				Console.WriteLine("Wrong password");
		//				// todo: exit 1
		//			}
		//
		//			//Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);
		//			//var decry = AesOperation.DecryptString(password, entry.SecretEncrypted);
		//		}
		//		else
		//		{
		//			Console.WriteLine("Missing password");
		//			Console.WriteLine("Enter new password: ");
		//			password = Console.ReadLine();
		//
		//			config = new Config();
		//			var tt = SHA256.HashData(Encoding.UTF8.GetBytes(password));
		//			config.PasswordHash = Convert.ToBase64String(tt);
		//
		//			File.WriteAllText(file, JsonSerializer.Serialize(config, new JsonSerializerOptions
		//			{
		//				WriteIndented = true
		//			}));
		//		}
		//
		//		Entry newEntry = new Entry()
		//		{
		//			Name = name,
		//			SecretEncrypted = AesOperation.EncryptString(password, secret),
		//			Type = EntryType.Totp, // todo
		//			Size = size,
		//		};
		//
		//		if (config.Entries == null)
		//		{
		//			config.Entries = new List<Entry>() { newEntry };
		//		}
		//		else
		//		{
		//			config.Entries.Add(newEntry);
		//		}
		//
		//		File.WriteAllText(file, JsonSerializer.Serialize(config, new JsonSerializerOptions
		//		{
		//			WriteIndented = true
		//		}));
		//	},
		//	nameArgument, secretArgument, typeOption, sizeOption);
		//	rootCommand.Add(addCommand);
		//
		//	var getCommand = new Command("get", "Get OTP");
		//	getCommand.Add(nameArgument);
		//	getCommand.SetHandler((name) =>
		//	{
		//		Console.WriteLine($"name   = {name}");
		//	},
		//	nameArgument);
		//	rootCommand.Add(getCommand);
		//
		//	var listCommand = new Command("list", "List all entries");
		//	listCommand.SetHandler(() =>
		//	{
		//		Console.WriteLine($"list...");
		//	});
		//	rootCommand.Add(listCommand);
		//
		//	await rootCommand.InvokeAsync(args);
		//}
	}

	[Command("add", Description = "Add new entry")]
	public class AddCommand : ICommand
	{
		// Order: 0
		[CommandParameter(0, Description = "Name")]
		public required string Name { get; init; }

		[CommandParameter(1, Description = "Secret key")]
		public required string Secret { get; init; }

		[CommandOption("type", 't', Description = "OTP type")]
		public string Type { get; init; } = "totp";

		[CommandOption("length", 'l', Description = "OTP length", Validators =)]
		public int Length { get; init; } = 6;

		public ValueTask ExecuteAsync(IConsole console)
		{
			console.Output.WriteLine($"name     = {Name}");
			console.Output.WriteLine($"secret   = {Secret}");
			console.Output.WriteLine($"--type   = {Type}");
			console.Output.WriteLine($"--length = {Length}");

			// If the execution is not meant to be asynchronous,
			// return an empty task at the end of the method.
			return default;
		}

		//var addCommand = new Command("add", "Add new entry");
		//
		//var typeOption = new Option<string>(
		//	name: "--type",
		//	description: "OTP type",
		//	getDefaultValue: () => "totp");
		//typeOption.FromAmong("totp", "hotp");
		//	typeOption.AddAlias("-t");
		//
		//	var sizeOption = new Option<int>(
		//		name: "--length",
		//		description: "OTP length",
		//		getDefaultValue: () => 6);
		//sizeOption.FromAmong(Enumerable.Range(4, 5).Select(x => x.ToString()).ToArray());
		//	sizeOption.AddAlias("-l");
		//
		//	var secretArgument = new Argument<string>(
		//		name: "secret",
		//		description: "Secret key");
		//
		//var nameArgument = new Argument<string>(
		//	name: "name",
		//	description: "Name");



	}

	internal class Config
	{
		public string PasswordHash { get; set; }
		public List<Entry> Entries { get; set; }
	}

	internal class Entry
	{
		public string Name { get; init; }
		public string SecretEncrypted { get; init; }
		//[JsonIgnore]
		//public string Secret { get; init; }
		public EntryType Type { get; init; }
		public int Size { get; init; }
	}

	enum EntryType
	{
		Totp,
		Hotp
	}

	public class AesOperation
	{
		// src: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

		public static string EncryptString(string key, string plainText)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				//aes.Key = Encoding.UTF8.GetBytes(key);
				aes.Key = DeriveKeyFromPassword(key);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
						{
							streamWriter.Write(plainText);
						}

						array = memoryStream.ToArray();
					}
				}
			}

			return Convert.ToBase64String(array);
		}

		public static string DecryptString(string key, string cipherText)
		{
			byte[] iv = new byte[16];
			byte[] buffer = Convert.FromBase64String(cipherText);

			using (Aes aes = Aes.Create())
			{
				//aes.Key = Encoding.UTF8.GetBytes(key);
				aes.Key = DeriveKeyFromPassword(key);
				aes.IV = iv;
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
		}

		private static byte[] DeriveKeyFromPassword(string password)
		{
			// src: https://code-maze.com/csharp-string-encryption-decryption/
			var emptySalt = Array.Empty<byte>();
			var iterations = 1000;
			var desiredKeyLength = 16; // 16 bytes equal 128 bits.
			var hashMethod = HashAlgorithmName.SHA384;
			return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password), emptySalt, iterations, hashMethod, desiredKeyLength);
		}
	}
}
