using System;
using System.Threading;
using System.Threading.Tasks;  // using Parallel class
using System.Drawing;
using System.Drawing.Imaging;
using CellularAutomata.Outputs.Vectors;

namespace CellularAutomata.Outputs
{

	public static class TunnelImageManager
	{

		public static void ReScale (int newScale)
		{
			blockScale = newScale;
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

			bitmap = new Bitmap ((width * blockScale) / blockSize, (height * blockScale) / blockSize);

			Update (values);

		}  // end InitOneDCubicBitmap, public static void method

		public static void Update (int[] values)
		{

			Clean ();

			int[] sizes = new int[2] {
				(bitmap.Width * blockSize) / blockScale,
				(bitmap.Height * blockSize) / blockScale
				};

			Draw (values, sizes);

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
				bitmap = new Bitmap (bitmap.Width, bitmap.Height);
			} catch {
				// failure!
				return false;
			}

			// success!
			return true;

		}

		private static Bitmap bitmap;
		private static int blockSize = 20;
		private static int blockScale = 20;

		private static void Draw (int[] states, int[] sizes)
		{

			DensityVector[,] blocks = Blocks.Multiple (states, 6, sizes, blockSize);

			int width = blocks.GetLength (0);
			Parallel.For (0, blocks.Length, i => {
				int x = i % width;
				int y = i / width;
				int[] center = ComputeCenter (x, y);
				DrawVectorOrigin (blocks [x, y], center);
				DrawVectorDot (blocks [x, y], center);
			});

		}

		private static int[] ComputeCenter (int x, int y)
		{

			int[] result = new int [2];
			result [0] = (x * blockScale) + (blockScale / 2);
			result [1] = (y * blockScale) + (blockScale / 2);
			return result;

		}

		private static void DrawVectorOrigin (DensityVector vector, int[] center)
		{

			int size = (int)(Math.Sqrt (vector.D) * (double)blockScale);
			int[] corner = new int [2] { center [0] - (size / 2), center [1] - (size / 2) };

			Parallel.For (0, size * size, i => {
				int px = (i % size) + corner [0];
				int py = (i / size) + corner [1];
				bitmap.SetPixel (px, py, Color.Black);
			});

		}

		private static void DrawVectorDot (DensityVector vector, int[] center)
		{

			double max = ((double)blockSize * 0.5D) - 1D;
			DensityVector scaledVector = (double)blockScale * vector;
			int size = (int)Math.Min (Math.Sqrt(DensityVector.Magnitude (vector)) * (double)blockScale, max);
			
			int[] coords = new int [2] {
				(int)(Math.Sign (scaledVector.X) * -1D * Math.Min (2D * Math.Abs (scaledVector.X), max * 0.5D)) + center [0],
				(int)(Math.Sign (scaledVector.Y) * -1D * Math.Min (2D * Math.Abs (scaledVector.Y), max * 0.5D)) + center [1]
			};

			int[] corner = new int [2] { coords [0] - (size / 2), coords [1] - (size / 2) };

			Parallel.For (0, size * size, i => {
				int px = (i % size) + corner [0];
				int py = (i / size) + corner [1];
				bitmap.SetPixel (px, py, Color.Red);
			});

		}

		private static void Clean ()
		{

			Parallel.For (0, bitmap.Width * bitmap.Height, i => {
				int x = i % bitmap.Width;
				int y = i / bitmap.Width;
				bitmap.SetPixel (x, y, Color.White);
			});

		}

	}

}