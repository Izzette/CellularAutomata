using System;
using CellularAutomata.Outputs.Vectors;

namespace CellularAutomata.Outputs.Vectors
{

	public static class Blocks
	{

		public static DensityVector[,] Multiple (int[] states, int exclusiveMaxState, int[] sizes, int blockSize)
		{

			if ((0 != sizes [0] % blockSize) || (0 != sizes [1] % blockSize)) {
				throw new ArgumentException ();
			}

			double squareSize = Math.Pow (blockSize, 2D);
			DensityVector factor = DensityVector.Pow (
				new DensityVector (squareSize, squareSize, squareSize * 6D),
				-1D
				);
			int numberWidth = sizes [0] / blockSize;
			int numberHeight = sizes [1] / blockSize;
			DensityVector[,] result = new DensityVector [numberWidth, numberHeight];

			int numberCells = sizes [0] * sizes [1];
			for (int i = 0; i < numberCells; i++) {
				int width = i % sizes [0];
				int height = i / sizes [0];
				result [width / blockSize, height / blockSize] += ComputeSingleVector (states [i], exclusiveMaxState);
			}

			for (int i = 0; i < result.Length; i++) {
				int x = i % numberWidth;
				int y = i / numberHeight;
				result [x, y] *= factor;
			}

			return result;

		}

		private static DensityVector ComputeSingleVector (int state, int maxParticles)
		{

			DensityVector result = DensityVector.Empty;

			int workingState = state;
			for (int i = 0; i < maxParticles; i++) {
				int particle;
				workingState = (int)Math.DivRem (workingState, 2, out particle);
				double ratio = (double)i / (double)maxParticles;
				double angle = Math.PI * 2D * ratio;
				if (0 < particle) {
					result += new DensityVector (1D, angle);
				}
			}

			return result;

		}
	
	}

}

