using System;
using System.Threading;
using System.Threading.Tasks;  // using Parallel class
using System.Drawing;
using System.Drawing.Imaging;

namespace CellularAutomata.Outputs
{

	public static class TwoDHexagonalImageManager
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

			for (int i = 0; i < values.Length; i++) {
				int y = i / width;
				int x = i % width;
				Color color = GetColor (values [i]);
				for (int ie = 0; ie < (int)Math.Pow (scale, 2); ie++) {
					bitmap.SetPixel (
						((x * scale) + (ie % scale) + ((y % 2) * (scale / 2))) % (width * scale),
						(y * scale) + (ie / scale),
						color
						);
				}
			}

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

			if (0 >= state) {
				return Color.Black;
			}

			state--;

			int[] colors = new int [3];
			const int maxState = 6;

			if (maxState <= state) {
				return Color.White;
			}

			for (int i = 0; i < 3; i++) {
				if (i == (3 * state) / maxState) {
					int mag = (255 * (state % (maxState / 3))) / (maxState / 3);
					if (250 < mag) {
						mag = 255;
					}
					colors [i] = 255 - mag;
					colors [(i + 1) % 3] = mag;
					break;
				}
			}

			color = Color.FromArgb (colors [0], colors [1], colors [2]);

			return color;

		}

		private static int scale = 4;

	}

}