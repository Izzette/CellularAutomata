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

			Console.Write (population.GetStates ());

			do {

				Console.Write ("\n");

				Console.Write ("Rule: ");
				rule.number = BigInteger.Parse (Console.ReadLine ());

				if (0 == rule.number) {

					break;

				}

				string states = population.Evolve (rule, gen);

				string[] frames = states.Split ("\\g");

				int g = 0;

				foreach (string f in frames) {

					Bitmap bitmap = new Bitmap (width * 4, height * 4);

					int y = 0;
					int x = 0;

					string[] rows = f.Split ("\n");

					foreach (string r in rows) {

						string[] cells = r.Split ("");

						foreach (string c in cells) {

							Color color;

							switch (state) {

								case "0":
								color = Color.MediumVioletRed;
								break;

								case "1":
								color = Color.LightPink;
								break;

								case "2":
								color = Color.Orange;
								break;

								case "3":
								color = Color.Lavender;
								break;

								case "4":
								color = Color.Yellow;
								break;

								case "5":
								color = Color.Green;
								break;

								default:
								color = Color.Blue;
								break;

							}

							for (int ie = 0; ie < 4; ie++) {

								for (int iee = 0; iee < 4; iee++) {

									bitmap.SetPixel ((4 * x) + ie, (4 * y) + iee, color);

								}

							}

						}

						y++;

					}

					string filename = "imgbin/g" + Convert.ToString (g).PadLeft(Math.Log10 (gen)) + ".gif";

					bitmap.Save (filename, ImageFormat.Gif);

					g++;

				}

				Console.Write ("\n");

			} while (true);

			Console.Write ("\n");
		}

	}

}
