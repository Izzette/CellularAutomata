using System;
using System.Threading;
using System.Threading.Tasks;  // using Parallel class
using System.Drawing;
using System.Drawing.Imaging;

namespace CellularAutomata.Outputs
{

	public static class TwoDCubicImageManager
	{

		public static void Init (int[] sizes, int[] values)
		{

			int width;
			int height;

			try {
				width = sizes [0];
				height = sizes [1];
			} catch (IndexOutOfRangeException) {
				throw new ArgumentException ();
			}

			bitmap = new Bitmap (width * 4, height * 4);

			Update (values);

		}  // end InitOneDCubicBitmap, public static void method

		public static void Update (int[] values)
		{

			int width = bitmap.Size.Width / 4;

			Parallel.For (0, values.Length, i => {
				int x = i % width;
				int y = i / width;
				Color color = GetColor (values [i]);
				for (int ie = 0; ie < 16; ie++) {
					bitmap.SetPixel ((x * 4) + (ie % 4), (y * 4) + (ie / 4), color);
				}
			});

		}

		// return type bool success or failure
		public static bool Save (string path, OutputsFormat format)
		{

			ImageFormat imageFormat;

			switch (format) {
			case OutputsFormat.Bitmap:
				imageFormat = ImageFormat.Bmp;
				path += ".bmp";
				break;
			case OutputsFormat.Gif:
				imageFormat = ImageFormat.Gif;
				path += ".gif";
				break;
			default:
				throw new ArgumentException ();
			}

			try {
				bitmap.Save (path, imageFormat);
			} catch {
				// failure!
				return false;
			}

			// success!
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

		}
	}

}

