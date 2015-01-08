using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using CellularAutomata.Rules;
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
			int place = Convert.ToInt32 (Console.ReadLine ());

			Rule rule = new Rule (0, place);
			Population population = Population.BuildVNA (rule, size);

			do {

				Console.Write ("\n");

				Console.Write ("Rule: ");
				rule.number = BigInteger.Parse (Console.ReadLine ());

				if (0 == rule.number) {

					break;

				}

				string states = population.Evolve (rule, gen);

				string[] frames = states.Split (new string[1] {"\\g"}, StringSplitOptions.RemoveEmptyEntries);
				
				int g = 0;

				foreach (string f in frames) {
					
					Console.Write ("\nRule: [{0}, {1}]\nGeneration: {2}\n\n", rule.number, rule.place, g);

					string[] rows = f.Split (new string[1] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

					foreach (string r in rows) {
						
						string[] line = r.Split (new string[1] {"\\s"}, StringSplitOptions.RemoveEmptyEntries);
						
						foreach (string state in line) {
							
							ConsoleColor color;
							ConsoleColor foreground = ConsoleColor.Black;
							
							switch (state) {
								
							case "0":
								
								color = ConsoleColor.White;
								break;
								
							case "1":
								
								color = ConsoleColor.Red;
								break;
								
							case "2":
							
								color = ConsoleColor.Yellow;
								break;
								
							case "3":
							
								color = ConsoleColor.Green;
								break;
								
							case "4":
							
								color = ConsoleColor.Cyan;
								break;
								
							case "5":
							
								color = ConsoleColor.Blue;
								break;
								
							case "6":
							
								color = ConsoleColor.Magenta;
								break;
								
							default:
							
								color = ConsoleColor.Black;
								foreground = ConsoleColor.White;
								break;
								
							}
							
							Console.BackgroundColor = color;
							Console.ForegroundColor = foreground;
							
							Console.Write (state);
							
							Console.ResetColor ();
							
						}
						
						Console.Write ("\n");

					}
					
					g++;

				}
 
				Console.Write ("\n");

			} while (true);

			Console.Write ("\n");
		}

	}

}
