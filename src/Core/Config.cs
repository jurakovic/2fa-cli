using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace _2fa
{
	internal class Config
	{
		JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
		};

		public string PasswordHash { get; set; }
		public List<Entry> Entries { get; set; }

		[JsonIgnore]
		public static string Path => System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".2fa-cli.json");

		public static Config Read()
		{
			return JsonSerializer.Deserialize<Config>(File.ReadAllText(Path));
		}

		public void Write()
		{
			File.WriteAllText(Path, JsonSerializer.Serialize(this, _jsonOptions));
		}
	}
}
