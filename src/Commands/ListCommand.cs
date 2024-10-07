using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace _2fa
{
	internal class ListCommand : Command
	{
		public ListCommand() : base("list", "List all entries")
		{
			this.SetHandler(ExecuteAsync);
		}

		private Task ExecuteAsync()
		{
			string file = Config.Path;

			if (!File.Exists(file))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Config config = Config.Read();

				foreach (Entry e in config.Entries.OrderBy(x => x.Name))
					Console.WriteLine(e.Name);
			}

			return Task.CompletedTask;
		}
	}
}
