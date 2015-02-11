using System;
using System.Collections.Generic;
using CellularAutomata.Commands;

namespace CellularAutomata
{

	public class Program
	{

		public static void Main (string[] arguments)
		{

			Console.WriteLine (" Welcome to CellularAutomata!");

			history = new LinkedList<string> ();

			ApplicationCommand command;

			do {

				Console.Write (" >> ");

				history.AddFirst ("");
				LinkedListNode<string> current = history.First;

//				do {
//					if (Console.KeyAvailable) {
//						ConsoleKeyInfo cki = Console.ReadKey ();
//						if ((ConsoleKey.UpArrow == cki.Key) && (current != history.Last)) {
//							current = current.Next;
//						} else if ((ConsoleKey.DownArrow == cki.Key) && (current != history.First)) {
//							current = current.Previous;
//						} else if (ConsoleKey.Enter == cki.Key) {
//							break;
//						} else if ((ConsoleKey.Backspace == cki.Key) && (String.Empty != current.Value)) {
//							current.Value = current.Value.Substring (0, current.Value.Length - 1);
//						} else {
//							current.Value += cki.ToString ();
//						}
//					}
//				} while (true);

				current.Value = Console.ReadLine ();

				command = Interpreter.Excecute (current.Value);

			} while (ApplicationCommand.Continue == command);

		}

		private static LinkedList<string> history;
	
	}

}
