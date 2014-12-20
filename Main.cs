using System;
using CellularAutomata.Cells;
using CellularAutomata.Populations;

namespace CellularAutomata
{
	class MainClass
	{
		public static void Main ()
		{
			Console.WriteLine ("Welcome to CellularAutomata!");
			Console.WriteLine ("This program allows you to use any of the 256 rules.\n");

			Console.WriteLine ("Please type the width and height for this session and press enter.");

			Console.Write ("Width: ");
			int width = Convert.ToInt32 (Console.ReadLine ());

			Console.Write ("Height: ");
			int height = Convert.ToInt32 (Console.ReadLine ());

			Population population = Population.BuildECA (width);
			Rule rule = new Rule (0, 2);

			do {
				Console.Write ("Rule: ");
				string line = Console.ReadLine ();
				string[] cmds = line.Split (' ');

				if ("new" == cmds[0].ToLower ()) {
					try {
						rule.number = Convert.ToInt32 (cmds[1]);
						population = Population.BuildECA (rule, width);
					} catch (System.NullReferenceException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					} catch (System.OverflowException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					}
				} else {
					try {
						rule.number = Convert.ToInt32 (cmds[0]);
					} catch (System.OverflowException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					} catch (System.FormatException) {
						Console.Write ("{0}", population.GetRule ().number);
					}
				}  

				Console.Write ("\n");

				if (0 == rule.number) {
					break;
				}

				Console.Write ("\n");

				for (int i = 0; i < height; i++) {
					population = new Population (rule, Population.Evolve(population.GetRoot (), rule, width), width);
					foreach (ICell cell in population) {
						int state = cell.GetState ();
						if (0 == state) {
							Console.BackgroundColor = ConsoleColor.White;
							Console.ForegroundColor = ConsoleColor.Black;
						} else {
							Console.BackgroundColor = ConsoleColor.Black;
							Console.ForegroundColor = ConsoleColor.White;
						}
						Console.Write (cell.GetNeighbourhood ());
						Console.ResetColor ();
					}
					Console.Write ("\n");
				}
				Console.Write ("\n");
			} while (true);
		}
	}
}
