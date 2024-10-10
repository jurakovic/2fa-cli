using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fa
{
	internal class ListCommand : Command
	{
		public ListCommand()
			: base("list", "Lists all entries")
		{
			this.SetHandler(ExecuteAsync);
		}

		private Task ExecuteAsync()
		{
			if (!File.Exists(ConfigHelper.FilePath))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Config config = ConfigHelper.Read();

				if (config.Entries.Any())
				{
					foreach (Entry e in config.Entries.OrderBy(x => x.Service))
						Console.WriteLine(e.Service);
				}
				else
				{
					Console.WriteLine("No entries");
				}
			}

			return Task.CompletedTask;
		}
	}
}
