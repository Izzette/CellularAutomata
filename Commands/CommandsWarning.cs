using System;

namespace CellularAutomata.Commands
{

	public static class CommandsWarning
	{

		public static void NotImplemented (string command, string method, string enumerationName)
		{
			At (command, method);
			Console.WriteLine (" NotImplemented: Enumeration {0} not recognized !", enumerationName);
			Again ();
		}

		public static void CommandNotFound ()
		{
			CommandNotFound ("<MISSING>");
		}

		public static void CommandNotFound (string command)
		{
			At ("Interpreter", "Excecute");
			Console.WriteLine (" CommandNotFound: Did not recognize Command {0} !", command);
			Again ();
		}

		public static void MethodNotFound (string command)
		{
			MethodNotFound (command, "<MISSING>");
		}

		public static void MethodNotFound (string command, string method)
		{
			At (command);
			Console.WriteLine (" MethodNotFound: Did not recognize method {0} !", method);
			Again ();
		}

		public static void OptionNotFound (string command, string method)
		{
			OptionNotFound (command, method, "<MISSING>");
		}

		public static void OptionNotFound (string command, string method, string optionName)
		{
			At (command, method);
			Console.WriteLine (" OptionNotFound: Did not recognize option -{0} !", optionName);
			Again ();
		}

		public static void OptionArgumentNotValid (string command, string method, string optionName)
		{
			OptionArgumentNotValid (command, method, optionName, "<MISSING>");
		}

		public static void OptionArgumentNotValid (string command, string method, string optionName, string argument)
		{
			At (command, method);
			Console.WriteLine (" OptionArgumentNotValid: Option-argument {0}:{1} not valid !", optionName, argument);
			Again ();
		}

		public static void ArgumentNotValid (string command, string method)
		{
			ArgumentNotValid (command, method, "<MISSING>");
		}

		public static void ArgumentNotValid (string command, string method, string argument)
		{
			At (command, method);
			Console.WriteLine (" ArgumentNotValid: Argument {0} not valid !", argument);
		}

		private static void At (string command)
		{
			Console.Write (" At {0}", command);
		}

		private static void At (string command, string method)
		{
			Console.Write (" At {0} {1}", command, method);
		}

		private static void Again ()
		{
			Console.WriteLine (" Command failed: Please try again.");
		}

	}

}
