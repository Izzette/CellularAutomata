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

		public static void Init (int[] sizes, ushort[] values)
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

		public static void Update (ushort[] values)
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

			Color color;

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
			}

			return color;

		}

		private static int scale = 4;

	}

}

