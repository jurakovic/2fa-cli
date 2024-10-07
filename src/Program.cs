using System.CommandLine;
using System.Threading.Tasks;

namespace _2fa
{
	internal class Program
	{
		static async Task<int> Main(string[] args)
		{
			var rootCommand = CommandBuilder.CreateRootCommand();
			return await rootCommand.InvokeAsync(args);
		}

		/*
		static async Task Main(string[] args)
		{
			var rootCommand = new RootCommand("2FA CLI tool");

			var getCommand = new Command("get", "Get OTP");

			var getNameArgument = new Argument<string>(
				name: "name",
				description: "Name");

			getCommand.Add(getNameArgument);
			getCommand.SetHandler((name) =>
			{
				Console.WriteLine($"name   = {name}");
			},
			getNameArgument);
			rootCommand.Add(getCommand);

			var listCommand = new Command("list", "List all entries");
			listCommand.SetHandler(() =>
			{
				Console.WriteLine($"list...");
			});
			rootCommand.Add(listCommand);

			await rootCommand.InvokeAsync(args);
		}
		*/
	}
}
