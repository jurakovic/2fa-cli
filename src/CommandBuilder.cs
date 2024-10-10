using System.CommandLine;

namespace _2fa
{
	public static class CommandBuilder
	{
		public static RootCommand CreateRootCommand()
		{
			var rootCommand = new RootCommand("2FA CLI tool\nFor more information, visit https://github.com/jurakovic/2fa-cli");

			rootCommand.AddCommand(new AddCommand());
			rootCommand.AddCommand(new GetCommand());
			rootCommand.AddCommand(new ListCommand());
			rootCommand.AddCommand(new RemoveCommand());

			return rootCommand;
		}
	}
}
