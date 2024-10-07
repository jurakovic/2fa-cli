namespace _2fa
{
	internal class Entry
	{
		public string Name { get; init; }
		public string Secret { get; init; }
		public string Type { get; init; }
		public int Size { get; init; }
		public long Counter { get; set; }
	}
}
