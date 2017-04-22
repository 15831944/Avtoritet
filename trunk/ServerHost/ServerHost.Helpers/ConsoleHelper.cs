using System;

namespace ServerHost.Helpers
{
	public static class ConsoleHelper
	{
		public static void Info(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[{0:g}] ", DateTime.Now);
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("INFO: {0}", text);
			Console.ResetColor();
		}

		public static void Trace(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[{0:g}] ", DateTime.Now);
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("TRACE: {0}", text);
			Console.ResetColor();
		}

		public static void Error(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[{0:g}] ", DateTime.Now);
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("ERROR: {0}", text);
			Console.ResetColor();
		}

		public static void Debug(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[{0:g}] ", DateTime.Now);
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("DEBUG: {0}", text);
			Console.ResetColor();
		}

		public static void Warning(string text)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write("[{0:g}] ", DateTime.Now);
			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("WARNING: {0}", text);
			Console.ResetColor();
		}
	}
}
