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
	}
}
