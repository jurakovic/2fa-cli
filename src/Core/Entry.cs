namespace _2fa
{
	internal class Entry
	{
		public string Name { get; init; }
		public string Secret { get; init; }
		public EntryType Type { get; init; }
		public int Size { get; init; }
	}
}
