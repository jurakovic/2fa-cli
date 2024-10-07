using System;
using System.Text;

namespace _2fa
{
	internal static class ConsoleHelper
	{
		public static string GetConsolePassword()
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
					{
						Console.Write("\b\0\b");
						sb.Length--;
					}

					continue;
				}

				//Console.Write('*');
				sb.Append(cki.KeyChar);
			}

			//Console.WriteLine(sb.ToString());
			return sb.ToString();
		}

		public static void ClearLine()
		{
			Console.SetCursorPosition(0, Console.CursorTop - 1);
			for (int i = 0; i < 40; i++)
				Console.Write(" ");
			Console.SetCursorPosition(0, Console.CursorTop);
		}
	}
}
