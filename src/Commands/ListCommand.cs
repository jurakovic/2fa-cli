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
			string userPath = Environment.GetEnvironmentVariable("USERPROFILE");
			string file = Path.Combine(userPath, ".2fa-cli.json");

			if (!File.Exists(file))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				string text = File.ReadAllText(file);
				Config config = JsonSerializer.Deserialize<Config>(text);

				foreach (Entry e in config.Entries.OrderBy(x => x.Name))
					Console.WriteLine(e.Name);
			}

			return Task.CompletedTask;
		}
	}
}
