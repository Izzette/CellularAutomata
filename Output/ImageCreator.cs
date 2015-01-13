using System;
using System.IO;
using System.Threading.Tasks;  // using Parallel class
using System.Drawing;  // reference System.Drawing
using System.Drawing.Imaging;

namespace CellularAutomata.Outputs
{

	// creates images
	public static class ImageCreator
	{

		// selects color
		private static Color GetColor (int state)
		{

			// color object to be returned
			Color color;

			// intializes color from Color enum
			// state 0 is white
			// state (1,2,3,4,5,6) -> (R,Y,G,C,B,M) rainbow
			// default black
			switch (state) {
				case 0:
				color = Color.White;
				break;
				case 1:
				color = Color.Red;
				break;
				case 2:
				color = Color.Yellow;
				break;
				case 3:
				color = Color.Green;
				break;
				case 4:
				color = Color.Cyan;
				break;
				case 5:
				color = Color.Blue;
				break;
				case 6:
				color = Color.Magenta;
				break;
				default:
				color = Color.Black;
				break;
			}  // end switch statement

			// success!
			return color;

		}  // end GetColor, private static Color method

		// creates new general img
		public static Image General (int[] values, int[] size)
		{

			// image width
			int width = values.Length;

			// bitmap to be returned
			Bitmap bitmap = new Bitmap (width, 1);

			// Parallel through each pixel
			Parallel.For (0, width, i => {

				// select color
				Color color = GetColor (values [i]);

				// encode pixel
				bitmap.SetPixel (i, 0, color);

			}); // end Parallel.For call

			// success!
			return bitmap;

		} // end General, public static Image method for new img
		
		// update general img
		// adds next generation to lastImg and returns the new image
		public static Image General (Image lastImg, int generation, int[] values)
		{

			// references to the last images size and dimensions
			Size lastSize = lastImg.Size;
			int lastWidth = lastSize.Width;
			int lastHeight = lastSize.Height;

			// Check for size compatability
			if ((values.Length != lastWidth) && (generation != lastHeight)) {

				// Exit
				throw new FormatException ("Image lastImg size is incompatable");

			}

			// new dimensions and size
			int width = lastWidth;
			int height = lastHeight + 1;

			// bitmap object that will be returned
			Bitmap workingBmp = new Bitmap (width, height);

			// bitmap object for lastImg to transcribe onto WorkingBmp
			Bitmap lastBmp = new Bitmap (lastImg);

			// Parallel through existing generations
			Parallel.For (0, lastWidth * lastHeight, i => {

				// find pixel position
				int x = i % lastWidth;
				int y = i / lastWidth;

				// get color
				Color color = lastBmp.GetPixel (x, y);

				// set pixel on returned bitmap object
				workingBmp.SetPixel (x, y, color);

			});  // end Parallel.For call

			// Parallel through last line of image
			Parallel.For (0, width, i => {

				// select color
				Color color = GetColor (values [i]);

				// Encode pixel
				workingBmp.SetPixel (i, height - 1, color);

			});  // end Parallel.For call

			// success
			return workingBmp;

		}  // end General, static method Image for update img

	}  // end ImageCreator, static class

}  // end CellularAutomata.Outputs, namespace

