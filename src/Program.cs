using System;
using System.CommandLine;
using System.Linq;
using System.Threading.Tasks;

namespace _2fa
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var rootCommand = new RootCommand("2FA CLI tool");

			var addCommand = new Command("add", "Add new entry");

			var kindOption = new Option<string>(
				name: "--kind",
				description: "Kind.",
				getDefaultValue: () => "totp");
			kindOption.FromAmong("totp", "hotp");
			kindOption.AddAlias("-k");

			var sizeOption = new Option<int>(
				name: "--size",
				description: "Size.",
				getDefaultValue: () => 6);
			sizeOption.FromAmong(Enumerable.Range(4, 5).Select(x => x.ToString()).ToArray());
			sizeOption.AddAlias("-s");

			var secretArgument = new Argument<string>(
				name: "secret",
				description: "Secret key");

			var nameArgument = new Argument<string>(
				name: "name",
				description: "Name");

			addCommand.Add(nameArgument);
			addCommand.Add(secretArgument);
			addCommand.Add(kindOption);
			addCommand.Add(sizeOption);

			addCommand.SetHandler((name, secret, kind, size) =>
			{
				Console.WriteLine($"name   = {name}");
				Console.WriteLine($"secret = {secret}");
				Console.WriteLine($"--kind = {kind}");
				Console.WriteLine($"--size = {size}");
			},
			nameArgument, secretArgument, kindOption, sizeOption);
			rootCommand.Add(addCommand);

			var getCommand = new Command("get", "Get OTP");
			getCommand.Add(nameArgument);
			getCommand.SetHandler((name) =>
			{
				Console.WriteLine($"name   = {name}");
			},
			nameArgument);
			rootCommand.Add(getCommand);

			var listCommand = new Command("list", "List all entries");
			listCommand.SetHandler(() =>
			{
				Console.WriteLine($"list...");
			});
			rootCommand.Add(listCommand);

			await rootCommand.InvokeAsync(args);
		}
	}
}
