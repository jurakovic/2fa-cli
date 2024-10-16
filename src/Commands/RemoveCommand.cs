﻿using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _2fa
{
	internal class RemoveCommand : Command
	{
		public RemoveCommand()
			: base("remove", "Removes an existing entry")
		{
			var serviceArgument = new Argument<string>("service", "The name of the organization or service provider");

			this.Add(serviceArgument);
			this.AddAlias("rm");
			this.SetHandler(ExecuteAsync, serviceArgument);
		}

		private Task ExecuteAsync(string service)
		{
			if (!File.Exists(ConfigHelper.FilePath))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Config config = ConfigHelper.Read();
				Entry entry = config.Entries.SingleOrDefault(x => x.Service.ToUpper() == service.ToUpper());

				if (entry == null)
				{
					Console.WriteLine($"Entry '{service}' not found.");
				}
				else
				{
					config.Entries.Remove(entry);
					ConfigHelper.Write(config);
					Console.WriteLine($"Entry '{service}' removed.");
				}
			}
			return Task.CompletedTask;
		}
	}
}
