using System;
using System.IO;
using System.Security.AccessControl;
using System.Drawing;  // reference System.Drawing
using System.Drawing.Imaging;
using CellularAutomata.Commands;  // reference OutputFormats
using CellularAutomata.Outputs;  // reference ImageCreator
using CellularAutomata.Populations; // reference States
using CellularAutomata.Populations.Cells;  // reference Arrangements

namespace CellularAutomata.Commands
{
	public static class OutputsControl
	{

		private static DirectoryInfo bin = Directory.CreateDirectory ("bin");

		private static OutputFormats DefaultFormat {  // defaults to console output
			get { return OutputFormats.Console; }
			set { ; }
		}

		// collection name contains Variety, IPopulation type, IRule as string
		public static void Out (string collection, int generation, States states, OutputFormats format)
		{

			// break if quiet
			if (OutputFormats.Quiet == format) {
				return;
			}

			// subdirectory
			DirectoryInfo subDirectory = bin.CreateSubdirectory (collection + "/");

			switch (format) {

			case OutputFormats.Bitmap:

				// generic image
				Image image;

				switch (states.Arangement) {

				case Arangements.OneDCubic:

					// search for image to add a row to
					FileInfo[] files = subDirectory.GetFiles (Convert.ToString (generation - 1) + ".bmp");

					if (0 != files.Length) {

						FileStream stream = files [0].OpenRead ();

						Image lastImg = Image.FromStream (stream);

						image = ImageCreator.General (lastImg, generation, states.Values);

					} else {

						// create image with one line
						image = ImageCreator.General (generation, states.Values, states.Sizes);

					}  // end if statment

					Bitmap bitmap = new Bitmap (image);
					bitmap.Save (subDirectory.ToString () + Convert.ToString (generation) + ".bmp", ImageFormat.Bmp);

					break;

				}  // end switch (states.Sizes.Length) statement

				break;

			}  // end switch (format) statement

		}

	}

}

