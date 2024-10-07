namespace _2fa
{
	internal class Entry
	{
		public string Service { get; init; }
		public string SecretKey { get; init; }
		public string Type { get; init; }
		public int Digits { get; init; }
		public long Counter { get; set; }
	}
}
