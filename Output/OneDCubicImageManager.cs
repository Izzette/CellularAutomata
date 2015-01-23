using System;
using System.Threading;
using System.Threading.Tasks;  // using Parallel class
using System.Drawing;  // reference System.Drawing
using System.Drawing.Imaging;

namespace CellularAutomata.Outputs
{

	// creates images
	public static class OneDCubicImageManager
	{

		public static void ReScale (int newScale)
		{
			scale = newScale;
		}

		public static void Init (int[] values, int maxGeneration)
		{

			int width = values.Length;
			int height = (maxGeneration + 1);

			bitmap = new Bitmap (width * scale, height * scale);

			Update (values, 0);

		}  // end InitOneDCubicBitmap, public static void method

		public static void Update (int[] values, int currentGeneration)
		{

			Parallel.For (0, values.Length, i => {
				// select color
				Color color = GetColor (values [i]);
				// encode pixel
				for (int ie = 0; ie < Math.Pow (scale, 2); ie++) {
					bitmap.SetPixel ((i * scale) + (ie % scale), (currentGeneration * scale) + (ie / scale), color);
				}
			}); // end Parallel.For call

		}

		// return type bool success or failure
		public static bool Save (string path, OutputsFormat format)
		{

			ImageFormat imageFormat;

			switch (format) {
			case OutputsFormat.Bitmap:
			case OutputsFormat.BitmapSection:
				imageFormat = ImageFormat.Bmp;
				path += ".bmp";
				break;
			case OutputsFormat.Gif:
				imageFormat = ImageFormat.Gif;
				path += ".gif";
				break;
			case OutputsFormat.Png:
				imageFormat = ImageFormat.Png;
				path += ".png";
				break;
			default:
				throw new ArgumentException ();
			}

			try {
				bitmap.Save (path, imageFormat);
			} catch {
				//failure!
				return false;
			}

			//success!
			return true;

		}

		private static Bitmap bitmap;

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

		private static int scale = 2;

	}  // end OneDCubicImageManager, public static class

}  // end CellularAutomata.Outputs, namespace

