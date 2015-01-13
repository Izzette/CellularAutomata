using System;
using System.IO;
using System.Security.AccessControl;
using System.Drawing;  // reference System.Drawing
using System.Drawing.Imaging;
using CellularAutomata.Commands;  // reference OutputsFormat
using CellularAutomata.Outputs;  // reference ImageCreator
using CellularAutomata.Populations; // reference States
using CellularAutomata.Populations.Cells;  // reference Arrangements

namespace CellularAutomata.Commands
{
	public static class OutputsControl
	{

		private static DirectoryInfo bin = Directory.CreateDirectory ("bin");
		private static Image image;

		private static OutputsFormat DefaultFormat {  // defaults to console output
			get { return OutputsFormat.Console; }
			set { ; }
		}

		// collection name contains CellsVariety, IPopulation type, IRule as string
		public static void Out (int generation, States states, OutputsFormat format)
		{

			// break if quiet
			if (OutputsFormat.Quiet == format) {
				return;
			}

			switch (format) {

			case OutputsFormat.Bitmap:

				switch (states.Arangement) {

				case CellsArangement.OneDCubic:

					if (null != image) {

						Image tempImage = ImageCreator.General (image, generation, states.Values);

						image = null;

						image = tempImage;

						tempImage = null;

					} else {

						// create image with one line
						image = ImageCreator.General (states.Values, states.Sizes);

					}  // end if statment

					break;

				}  // end switch (states.Sizes.Length) statement

				break;

			}  // end switch (format) statement

		}  // end Out, public static void method

		public static void Save (string collection, string name)
		{

			DirectoryInfo subDirectory = bin.CreateSubdirectory (collection);

			Bitmap bitmap = new Bitmap (image);

			bitmap.Save (subDirectory.ToString () + "/" + name + ".bmp", ImageFormat.Bmp);

		}

	}  // end OutputsControl

}  // end CellularAutomata.Commands namespace

