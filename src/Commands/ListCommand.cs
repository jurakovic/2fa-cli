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
			: base("list", "Lists all 2FA entries")
		{
			this.SetHandler(ExecuteAsync);
		}

		private Task ExecuteAsync()
		{
			if (!File.Exists(Config.FilePath))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Config config = Config.Read();

				foreach (Entry e in config.Entries.OrderBy(x => x.Service))
					Console.WriteLine(e.Service);
			}

			return Task.CompletedTask;
		}
	}
}
