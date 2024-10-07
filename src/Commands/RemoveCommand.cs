using System;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace _2fa
{
	internal class RemoveCommand : Command
	{
		JsonSerializerOptions jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		};

		public RemoveCommand() : base("remove", "Removes a 2FA entry")
		{
			var nameArgument = new Argument<string>(
				name: "name",
				description: "Name");

			this.Add(nameArgument);
			this.AddAlias("rm");
			this.SetHandler(ExecuteAsync, nameArgument);
		}

		private Task ExecuteAsync(string name)
		{
			string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			string file = Path.Combine(userPath, ".2fa-cli.json");

			if (!File.Exists(file))
			{
				Console.WriteLine("No entries");
			}
			else
			{
				Config config = JsonSerializer.Deserialize<Config>(File.ReadAllText(file));
				Entry entry = config.Entries.FirstOrDefault(x => x.Name == name);

				if (entry == null)
				{
					Console.WriteLine($"Entry '{name}' not found.");
					return Task.CompletedTask;
				}
				else
				{
					config.Entries.Remove(entry);
					File.WriteAllText(file, JsonSerializer.Serialize(config, jsonOptions));
					Console.WriteLine($"OK");
				}
			}
			return Task.CompletedTask;
		}
	}
}
