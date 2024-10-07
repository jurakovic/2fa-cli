using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fa
{
	internal class RemoveCommand : Command
	{
		public RemoveCommand()
			: base("remove", "Removes a 2FA entry")
		{
			var nameArgument = new Argument<string>("name", "Name");

			this.Add(nameArgument);
			this.AddAlias("rm");
			this.SetHandler(ExecuteAsync, nameArgument);
		}

		private Task ExecuteAsync(string name)
		{
			if (!File.Exists(Config.Path))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Config config = Config.Read();
				Entry entry = config.Entries.SingleOrDefault(x => x.Name == name);

				if (entry == null)
				{
					Console.WriteLine($"Entry '{name}' not found.");
				}
				else
				{
					config.Entries.Remove(entry);
					config.Write();
					Console.WriteLine($"OK");
				}
			}
			return Task.CompletedTask;
		}
	}
}
