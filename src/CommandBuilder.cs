using System.CommandLine;

namespace _2fa
{
	public static class CommandBuilder
	{
		public static RootCommand CreateRootCommand()
		{
			var rootCommand = new RootCommand("2fa command line tool");

			rootCommand.AddCommand(new AddCommand());

			return rootCommand;
		}
	}
}
