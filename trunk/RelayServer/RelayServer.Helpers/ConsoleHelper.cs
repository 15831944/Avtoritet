using System;

namespace RelayServer.Helpers
{
	public static class ConsoleHelper
	{
		public static void Info(string text)
		{
			System.Console.ForegroundColor = System.ConsoleColor.Green;
			System.Console.Write("[{0:o}] ", System.DateTime.Now);
			System.Console.ResetColor();
			System.Console.ForegroundColor = System.ConsoleColor.Yellow;
			System.Console.WriteLine("INFO: {0}", text);
			System.Console.ResetColor();
		}

		public static void Trace(string text)
		{
			System.Console.ForegroundColor = System.ConsoleColor.Green;
			System.Console.Write("[{0:o}] ", System.DateTime.Now);
			System.Console.ResetColor();
			System.Console.ForegroundColor = System.ConsoleColor.Green;
			System.Console.WriteLine("TRACE: {0}", text);
			System.Console.ResetColor();
		}

		public static void Error(string text)
		{
			System.Console.ForegroundColor = System.ConsoleColor.Green;
			System.Console.Write("[{0:o}] ", System.DateTime.Now);
			System.Console.ResetColor();
			System.Console.ForegroundColor = System.ConsoleColor.Red;
			System.Console.WriteLine("ERROR: {0}", text);
			System.Console.ResetColor();
		}

		public static void Debug(string text)
		{
			System.Console.ForegroundColor = System.ConsoleColor.Green;
			System.Console.Write("[{0:o}] ", System.DateTime.Now);
			System.Console.ResetColor();
			System.Console.ForegroundColor = System.ConsoleColor.White;
			System.Console.WriteLine("DEBUG: {0}", text);
			System.Console.ResetColor();
		}

		public static void Warning(string text)
		{
			System.Console.ForegroundColor = System.ConsoleColor.Green;
			System.Console.Write("[{0:o}] ", System.DateTime.Now);
			System.Console.ResetColor();
			System.Console.ForegroundColor = System.ConsoleColor.Gray;
			System.Console.WriteLine("WARNING: {0}", text);
			System.Console.ResetColor();
		}
	}
}
