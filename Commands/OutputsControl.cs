using System;
using System.IO;
using System.Security.AccessControl;
using System.Drawing;  // reference System.Drawing
using System.Drawing.Imaging;
using CellularAutomata.Commands;  // reference OutputsFormat
using CellularAutomata.Populations; // reference States
using CellularAutomata.Populations.Cells;
using CellularAutomata.Outputs;

namespace CellularAutomata.Commands
{
	public static class OutputsControl
	{

		public static void Init (States states, int maxGeneration, OutputsFormat format)
		{

			string method = "Init";

			if (OutputsFormat.Quiet == format) {
				return;
			}

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Bitmap:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Init (states.Values, maxGeneration);
					break;
				default:
					CommandsWarning.NotImplemented (Command, method, cellsArangement.ToString ());
					break;
				}  // end switch (cellsArangement) statement
				break;
			default:
				CommandsWarning.NotImplemented (Command, method, cellsArangement.ToString ());
				break;
			}  // end switch (format) statment

		}  // end Init, public static void method

		// collection name contains CellsVariety, IPopulation type, IRule as string
		public static void Update (States states, int currentGeneration, OutputsFormat format)
		{

			string method = "Update";

			if (OutputsFormat.Quiet == format) {
				return;
			}

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
				case OutputsFormat.Bitmap:
				switch (cellsArangement) {
					case CellsArangement.OneDCubic:
					OneDCubicImageManager.Update (states.Values, currentGeneration);
					break;
					default:
					CommandsWarning.NotImplemented (Command, method, cellsArangement.ToString ());
					break;
				}  // end switch (cellsArangement) statment
				break;
			case OutputsFormat.Console:
				break;
			default:
				CommandsWarning.NotImplemented (Command, method, cellsArangement.ToString ());
				break;
			}  // end switch (format) statment

		}  // end Update, public static void method

		public static void SaveImage (CellsArangement cellsArangement, string subDirName, string fileName, OutputsFormat format)
		{

			string method = "Save";

			if (OutputsFormat.Quiet == format) {
				return;
			}

			DirectoryInfo subDirectory = bin.CreateSubdirectory (subDirName);

			ImageFormat imageFormat;

			switch (format) {
			case OutputsFormat.Bitmap:
				imageFormat = ImageFormat.Bmp;
				break;
			default:
				CommandsWarning.NotImplemented (Command, method, format.ToString ());
				return;
			}  // end switch (format)


			switch (cellsArangement) {
				case CellsArangement.OneDCubic:
				Bitmap bitmap = OneDCubicImageManager.GetBitmap ();
				bitmap.Save (subDirectory.ToString () + "/" + fileName + ".bmp", imageFormat);
				break;
				default:
				CommandsWarning.NotImplemented (Command, method, cellsArangement.ToString ());
				break;
			}  // end switch (cellsArangement) statment

		}  // end SaveBitmap, public static void method

		private static string Command {
			get { return "OutputsControl"; }
			set { ; }
		}

		private static DirectoryInfo bin = Directory.CreateDirectory ("bin");
	
	}  // end OutputsControl, public static class

}  // end CellularAutomata.Commands namespace

