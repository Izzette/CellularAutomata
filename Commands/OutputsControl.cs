using System;
using System.IO;
using System.Security.AccessControl;
using CellularAutomata.Commands;
using CellularAutomata.Populations;
using CellularAutomata.Outputs;

namespace CellularAutomata.Commands
{
	public static class OutputsControl
	{

		public static void Excecute (string method, Option[] options, string[] arguments)
		{
			switch (method) {
			case "rescale":
				if (2 > arguments.Length) {
					CommandsWarning.ArgumentNotValid (Command, method);
					return;
				}
				switch (arguments [0]) {
				case "one":
					try {
						OneDCubicImageManager.ReScale (Convert.ToInt32 (arguments [1]));
					} catch (FormatException) {
						CommandsWarning.ArgumentNotValid (Command, method, "<NEW SCALE>");
						return;
					}
					break;
				case "two":
					try {
						TwoDCubicImageManager.ReScale (Convert.ToInt32 (arguments [1]));
					} catch (FormatException) {
						CommandsWarning.ArgumentNotValid (Command, method, "<NEW SCALE>");
						return;
					}
					break;
				default:
					CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
					return;
				}
				break;
			default:
				CommandsWarning.MethodNotFound (Command, method);
				return;
			}
		}

		public static void Init (IPopulation population, int maxGeneration, string filename, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			States states = population.GetStates ();

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
			case OutputsFormat.Png:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Init (states.Values, maxGeneration);
					break;
				case CellsArangement.TwoDCubic:
					TwoDCubicImageManager.Init (states.Sizes, states.Values);
					SaveImage (population, filename, format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statement
				break;
			case OutputsFormat.BitmapSection:
				switch (cellsArangement) {
				case CellsArangement.TwoDCubic:
				case CellsArangement.OneDCubic:
					int[] sectionValues = new int [states.Sizes [0]];
					for (int i = 0; i < sectionValues.Length; i++) {
						sectionValues [i] = states.Values [i];
					}
					OneDCubicImageManager.Init (sectionValues, maxGeneration);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			default:
				throw new ArgumentException ();
			}  // end switch (format) statment

		}  // end Init, public static void method

		// collection name contains CellsVariety, IPopulation type, IRule as string
		public static void Update (IPopulation population, int currentGeneration, string filename, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			States states = population.GetStates ();

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
			case OutputsFormat.Png:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Update (states.Values, currentGeneration);
					break;
				case CellsArangement.TwoDCubic:
					TwoDCubicImageManager.Update (states.Values);
					SaveImage (population, filename, format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statment
				break;
			case OutputsFormat.BitmapSection:
				switch (cellsArangement) {
					case CellsArangement.TwoDCubic:
					case CellsArangement.OneDCubic:
					int[] sectionValues = new int [states.Sizes [0]];
					for (int i = 0; i < sectionValues.Length; i++) {
						sectionValues [i] = states.Values [i];
					}
					OneDCubicImageManager.Update (sectionValues, currentGeneration);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			default:
				throw new ArgumentException ();
			}  // end switch (format) statment

		}  // end Update, public static void method

		public static void Final (IPopulation population, string fileName, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			if (OutputsFormat.BitmapSection == format) {
				string subDirName = population.ToString ();
				DirectoryInfo subDirectory = bin.CreateSubdirectory (subDirName);
				OneDCubicImageManager.Save (subDirectory.ToString () + "/" + fileName + "sect", format);
				return;
			}

			CellsArangement cellsArangement = population.GetCellsArangement ();

			switch (cellsArangement) {
			case CellsArangement.OneDCubic:
				switch (format) {
				case OutputsFormat.Gif:
				case OutputsFormat.Bitmap:
				case OutputsFormat.Png:
					SaveImage (population, fileName, format);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			case CellsArangement.TwoDCubic:
				return;
			default:
				throw new ArgumentException ();
			}

		}

		private static void SaveImage (IPopulation population, string fileName, OutputsFormat format)
		{

			string subDirName = population.ToString ();

			CellsArangement cellsArangement = population.GetCellsArangement ();

			DirectoryInfo subDirectory = bin.CreateSubdirectory (subDirName);

			bool success;

			switch (cellsArangement) {
			case CellsArangement.OneDCubic:
				success = OneDCubicImageManager.Save (subDirectory.ToString () + "/" + fileName, format);
				break;
			case CellsArangement.TwoDCubic:
				success = TwoDCubicImageManager.Save (subDirectory.ToString () + "/" + fileName, format);
				break;
			default:
				throw new ArgumentException ();
			}  // end switch (cellsArangement) statment

			if (!success) {
				throw new IOException ();
			}

		}  // end SaveBitmap, public static void method

		private static string Command {
			get { return "OutputsControl"; }
			set { ; }
		}

		private static DirectoryInfo bin = Directory.CreateDirectory ("bin");
	
	}  // end OutputsControl, public static class

}  // end CellularAutomata.Commands namespace

