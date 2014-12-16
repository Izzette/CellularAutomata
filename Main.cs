using System;
using System.Drawing;
using CellularAutomata;

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
			
			Bitmap bitmap = new Bitmap (width, height);

			Population pop = new Population (width, 0);

			do {
				Console.Write ("Rule: ");
				string cmd = Console.ReadLine ();
				string[] cmds = cmd.Split (new char[1] {' '});

				byte rule;

				if ("new" == cmds[0].ToLower ()) {
					try {
						rule = Convert.ToByte (cmds[1]);
						pop = new Population (width, rule);
					} catch (System.NullReferenceException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					} catch (System.OverflowException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					}
				} else {
					try {
						rule = Convert.ToByte (cmds[0]);
						pop = new Population (width, rule, pop.GetRoot ());
					} catch (System.OverflowException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					} catch (System.FormatException) {
						rule = pop.Rule;
						Console.Write ("{0}", rule);
					}
				}  

				Console.Write ("\n");
				
				if (0 == rule) {
					break;
				}
				
				for (int i = 0; i < height; i++) {
					pop.SaveCells (bitmap, i);
					pop.Evolve (1);
				}
				bitmap.Save ("automaton.bmp");
			} while (true);
		}
	}
}
