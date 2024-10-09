using System.Collections.Generic;

namespace _2fa
{
	internal class Config
	{
		public string PasswordHash { get; set; }
		public List<Entry> Entries { get; set; } = new List<Entry>();
	}
}
