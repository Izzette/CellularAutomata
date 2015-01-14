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

		public static void Init (IPopulation population, int maxGeneration, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			States states = population.GetStates ();

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Init (states.Values, maxGeneration);
					break;
				case CellsArangement.TwoDCubic:
					TwoDCubicImageManager.Init (states.Sizes, states.Values);
					SaveImage (population, "InitalCondition", format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statement
				break;
			default:
				throw new ArgumentException ();
			}  // end switch (format) statment

		}  // end Init, public static void method

		// collection name contains CellsVariety, IPopulation type, IRule as string
		public static void Update (IPopulation population, int currentGeneration, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			States states = population.GetStates ();

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Update (states.Values, currentGeneration);
					break;
				case CellsArangement.TwoDCubic:
					TwoDCubicImageManager.Update (states.Values);
					SaveImage (population, currentGeneration.ToString (), format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statment
				break;
			case OutputsFormat.Console:
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

			CellsArangement cellsArangement = population.GetCellsArangement ();

			switch (cellsArangement) {
			case CellsArangement.OneDCubic:
				switch (format) {
				case OutputsFormat.Gif:
				case OutputsFormat.Bitmap:
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

