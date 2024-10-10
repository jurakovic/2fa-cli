using System;
using System.Text;

namespace _2fa
{
	internal static class ConsoleHelper
	{
		public static string ReadPassword()
		{
			StringBuilder sb = new StringBuilder();
			while (true)
			{
				ConsoleKeyInfo cki = Console.ReadKey(true);
				if (cki.Key == ConsoleKey.Enter)
				{
					Console.WriteLine();
					break;
				}

				if (cki.Key == ConsoleKey.Backspace)
				{
					if (sb.Length > 0)
						sb.Length--;

					continue;
				}

				sb.Append(cki.KeyChar);
			}

			return sb.ToString();
		}

		public static void ClearLine()
		{
			Console.SetCursorPosition(0, Console.CursorTop - 1);
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write(" ");
			Console.SetCursorPosition(0, Console.CursorTop);
		}
	}
}
