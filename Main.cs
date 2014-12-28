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
			
			Console.Write ("Base: ");
			BigInteger place = BigInteger.Parse (Console.ReadLine ());

			Population population = Population.BuildECA (width);
			Rule rule = new Rule (0, place);

			do {
				
				Console.Write ("Rule: ");
				string line = Console.ReadLine ();
				string[] cmds = line.Split (' ');

				if ("new" == cmds[0].ToLower ()) {
					
					try {
						
						rule.number = BigInteger.Parse (cmds[1]);
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
						
						rule.number = BigInteger.Parse (cmds[0]);
						
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
					
					Bitmap bitmap = new Bitmap (width, height);
					
					for (int y = 0; y < height; y++) {
						
						population.Evolve (rule, height);
						ICell current = population.GetRoot ();
						
						for (int x = 0; x < width; x++) {
							
							Color color;
							
							int state = current.GetState ();

							switch (state) {
								
								case 0:
								color = Color.MediumVioletRed;
								break;
								
								case 1:
								color = Color.LightPink;
								break;
								
								case 2:
								color = Color.Orange;
								break;
								
								case 3:
								color = Color.Lavender;
								break;
								
								case 4:
								color = Color.Yellow;
								break;
								
								case 5:
								color = Color.Green;
								break;
								
								default:
								color = Color.Blue;
								break;
								
							}
							
							bitmap.SetPixel (x, y, color);
							
							current = current.GetNext ();
						
						}
						
					}
					
					bitmap.Save("automata.bmp");
					
				} else if ("quiet" == cmds[cmds.Length - 1].ToLower ()) {
					
					for (int i = 0; i < height; i++) {
						
						population.Evolve (rule, height);
						
					}
					
				} else {
					
					for (int i = 0; i < height; i++) {
						
						population.Evolve (rule, height);
						
						foreach (ICell cell in population) {
							
							int state = cell.GetState ();

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
							
						}
						
						Console.Write ("\n");
						
					}
					
				}
				
				Console.Write ("\n");
				
			} while (true);
		
		}
	
	}

}
