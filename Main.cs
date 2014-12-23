
using System;
using System.Drawing;
using CellularAutomata.Cells;
using CellularAutomata.Populations;


namespace CellularAutomata
{
	
	class MainClass
	{
		
		public static void Main ()
		{
			
			Console.WriteLine ("\nWelcome to CellularAutomata!");
			Console.WriteLine ("This program allows you to use any of the 256 rules.\n");

			Console.WriteLine ("Please type the width and height for this session and press enter.");

			Console.Write ("Width: ");
			ulong width = Convert.ToUInt64 (Console.ReadLine ());

			Console.Write ("Height: ");
			ulong height = Convert.ToUInt64 (Console.ReadLine ());
			
			Console.Write ("Base: ");
			ulong space = Convert.ToUInt64 (Console.ReadLine ());

			Population population = Population.BuildECA (width);
			Rule rule = new Rule (0, space);

			do {
				
				Console.Write ("Rule: ");
				string line = Console.ReadLine ();
				string[] cmds = line.Split (' ');

				if ("new" == cmds[0].ToLower ()) {
					
					try {
						
						rule.number = Convert.ToUInt64 (cmds[1]);
						population = Population.BuildECA (rule, width);
						
					} catch (System.NullReferenceException) {
						
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
						
					} catch (System.OverflowException) {
						
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
						
					}
					
				} else if ("reset" == cmds[0].ToLower ()) {
					
					Console.Write ("\n");
					rule.number = 0;
					MainClass.Main ();
				
				} else if ("quit" == cmds[0].ToLower ()) {
					
					Console.Write ("\n Exiting \n");
					rule.number = 0;
					
				} else {
					
					try {
						
						rule.number = Convert.ToUInt64 (cmds[0]);
						
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
				
				if ("save" == cmds[cmds.Length - 1].ToLower ()) {
					
					Bitmap bitmap = new Bitmap (Convert.ToInt32 (width), Convert.ToInt32 (height));
					
					for (int y = 0; y < Convert.ToInt32 (height); y++) {
						
						population = new Population (rule, Population.Evolve(population.GetRoot (), rule, width), width);
						ICell current = population.GetRoot ();
						
						for (int x = 0; x < Convert.ToInt32 (width); x++) {
							
							Color color;
							
							ulong state = current.GetState ();
							
							switch (state) {
								
								case 0:
								color = Color.Red;
								break;
								
								case 1:
								color = Color.Yellow;
								break;
								
								case 2:
								color = Color.Green;
								break;
								
								case 3:
								color = Color.Cyan;
								break;
								
								case 4:
								color = Color.Blue;
								break;
								
								default:
								color = Color.Magenta;
								break;
								
							}
							
							bitmap.SetPixel (x, y, color);
							
							current = current.GetNext ();
						
						}
						
					}
					
					bitmap.Save("automata.bmp");
					
				} else if ("quiet" == cmds[cmds.Length - 1].ToLower ()) {
					
					for (ulong i = 0; i < height; i++) {
						
						population = new Population (rule, Population.Evolve(population.GetRoot (), rule, width), width);
						
					}
					
				} else {
					
					for (ulong i = 0; i < height; i++) {
						
						population = new Population (rule, Population.Evolve(population.GetRoot (), rule, width), width);
						
						foreach (ICell cell in population) {
							
							ulong state = cell.GetState ();
							
							switch (state) {
								
								case 0:
								Console.BackgroundColor = ConsoleColor.Red;
								Console.ForegroundColor = ConsoleColor.Black;
								break;
								
								case 1:
								Console.BackgroundColor = ConsoleColor.Yellow;
								Console.ForegroundColor = ConsoleColor.Black;
								break;
								
								case 2:
								Console.BackgroundColor = ConsoleColor.Green;
								Console.ForegroundColor = ConsoleColor.Black;
								break;
								
								case 3:
								Console.BackgroundColor = ConsoleColor.Cyan;
								Console.ForegroundColor = ConsoleColor.Black;
								break;
								
								case 4:
								Console.BackgroundColor = ConsoleColor.Blue;
								Console.ForegroundColor = ConsoleColor.Black;
								break;
								
								default:
								Console.BackgroundColor = ConsoleColor.Magenta;
								Console.ForegroundColor = ConsoleColor.Black;
								break;
								
							}
							
							Console.Write (state);
							Console.ResetColor ();
							
						}
						
						Console.Write ("\n");
						
					}
					
				}
				
				Console.Write ("\n");
				
			} while (true);
		
		}
	
	}

}
