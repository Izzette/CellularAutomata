using System;
using CellularAutomata.Commands;

namespace CellularAutomata
{

	public class Program
	{

		public static void Main (string[] arguments)
		{

			Console.WriteLine ("CellularAutomata!");

			ApplicationCommands command;

			do {

				Console.Write (" >> ");

				string line = Console.ReadLine ();

				command = Interpreter.Parse (line);

				if (ApplicationCommands.Reset == command) {

					Main (arguments);

				}

			} while (ApplicationCommands.Continue == command);

		}
	
	}

}
