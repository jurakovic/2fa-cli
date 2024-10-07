using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace _2fa
{
	internal class Program
	{
		static async Task<int> Main(string[] args)
		{
			var rootCommand = CommandBuilder.CreateRootCommand();

			var builder = new CommandLineBuilder(rootCommand)
				.UseDefaults()
				.UseExceptionHandler((exception, context) =>
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"Error: {exception.Message}");
					Console.ResetColor();

					context.ExitCode = 1;
				});

			var parser = builder.Build();
			return await parser.InvokeAsync(args);
		}
	}
}
