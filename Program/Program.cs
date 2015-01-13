using System;
using CellularAutomata.Commands;

namespace CellularAutomata
{

	public class Program
	{

		public static void Main (string[] arguments)
		{

			Console.WriteLine ("CellularAutomata!");

			ApplicationCommand command;

			do {

				Console.Write (" >> ");

				string line = Console.ReadLine ();

				command = Interpreter.Excecute (line);

			} while (ApplicationCommand.Continue == command);

		}
	
	}

}
