using System;
using System.Drawing;
using System.Numerics;
using CellularAutomata.Cells;
using CellularAutomata.Populations;

namespace CellularAutomata
{

	class MainClass
	{

		public static void Main ()
		{

			Console.WriteLine ("\nWelcome to CellularAutomata!");

			Console.WriteLine ("Please type the width and height for this session and press enter.");

			Console.Write ("Width: ");
			int width = Convert.ToInt32 (Console.ReadLine ());

			Console.Write ("Height: ");
			int height = Convert.ToInt32 (Console.ReadLine ());

			int[] size = new int[2] { width, height };

			Console.Write ("Generations: ");
			int gen = Convert.ToInt32 (Console.ReadLine ());

			Console.Write ("Base: ");
			BigInteger place = BigInteger.Parse (Console.ReadLine ());

			Rule rule = new Rule (0, place);
			Population population = Population.BuildVNA (rule, size);

			
			ICell orig = population.GetRoot ();

			for (int i = 0; i < (width * height); i++) {

				if (0 == i % width) {

					Console.Write ("\n");

				}

				int state = orig.GetState ();

				Console.Write (state);

				orig = orig.GetNext ();

			}

			Console.Write ("\n\n");

			do {

				Console.Write ("\n");

				Console.Write ("Rule: ");
				rule.number = BigInteger.Parse (Console.ReadLine ());

				if (0 == rule.number) {

					break;

				}

				for (int g = 0; g < gen; g++) {

					ICell current = population.GetRoot ();
					Population pop = Population.BuildVNA (rule, size);
					ICell newC = pop.GetRoot ();

					for (int i = 0; i < (width * height); i++) {

						int neighbourhood = current.GetNeighbourhood (rule.place);
						newC.SetState (Population.Implement (neighbourhood, rule));

						current = current.GetNext ();
						newC = newC.GetNext ();

					}

					population = new Population (rule, pop.GetRoot (), size);
					current = population.GetRoot ();

					for (int i = 0; i < (width * height); i++) {

						if (0 == i % width) {

							Console.Write ("\n");
						
						}

						int state = current.GetState ();

						switch (state) {

						case 0:

							Console.BackgroundColor = ConsoleColor.Magenta;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						case 1:

							Console.BackgroundColor = ConsoleColor.Red;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						case 2:
						
							Console.BackgroundColor = ConsoleColor.Yellow;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						case 3:
						
							Console.BackgroundColor = ConsoleColor.Gray;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						case 4:
						
							Console.BackgroundColor = ConsoleColor.Green;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						case 5:
						
							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						default:
						
							Console.BackgroundColor = ConsoleColor.Blue;
							Console.ForegroundColor = ConsoleColor.Black;

							break;

						}

						Console.Write (state);

						Console.ResetColor ();

						current = current.GetNext ();

					}

					Console.Write ("\n\n");

				}

				Console.Write ("\n");

			} while (true);

			Console.Write ("\n");
		}

	}

}
