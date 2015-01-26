using System;
using System.Threading;
using System.Threading.Tasks;  // using Parallel class
using System.Drawing;
using System.Drawing.Imaging;

namespace CellularAutomata.Outputs
{

	public static class TwoDCubicImageManager
	{

		public static void ReScale (int newScale)
		{
			scale = newScale;
		}

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

			bitmap = new Bitmap (width * scale, height * scale);

			Update (values);

		}  // end InitOneDCubicBitmap, public static void method

		public static void Update (int[] values)
		{

			int width = bitmap.Size.Width / scale;

			Parallel.For (0, values.Length, i => {
				int x = i % width;
				int y = i / width;
				Color color = GetColor (values [i]);
				for (int ie = 0; ie < Math.Pow (scale, 2); ie++) {
					bitmap.SetPixel ((x * scale) + (ie % scale), (y * scale) + (ie / scale), color);
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
				color = Color.FromArgb (255, 255, 255);
				break;
			case 1:
			case 2:
			case 4:
			case 8:
				color = Color.FromArgb (127, 127, 127);
				break;
			case 3:
			case 5:
			case 6:
			case 9:
			case 10:
			case 12:
				color = Color.FromArgb (63, 63, 63);
				break;
			case 7:
			case 11:
			case 13:
			case 14:
				color = Color.FromArgb (31, 31, 31);
				break;
			default:
				color = Color.FromArgb (15, 15, 15);
				break;
			}  // end switch statement

			// success!
			return color;

		}

		private static int scale = 4;

	}

}

