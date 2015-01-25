using System;
using System.Diagnostics;
using CellularAutomata.Commands;

namespace CellularAutomata.PerfTest
{

	public static class TestRand
	{

		public static void Main (string[] args)
		{

			int iterations = Convert.ToInt32 (args [0]);
			int maxColor = Convert.ToInt32 (args [1]);
			int size = Convert.ToInt32 (args [2]);

			Random random = new Random ();

			Console.WriteLine (" Total iterations: {0}", iterations);

			Console.Write (" Continue [Y,n]: ");
			bool cont = Console.ReadLine ().ToLower ().StartsWith ("y");
			Console.Write ("\n");

			if (!cont) {
				Console.WriteLine (" Exiting");
				return;
			}

			Stopwatch startTime = new Stopwatch ();

			Console.WriteLine (" Start time: {0}\n", DateTime.Now.ToString ());

			float[,] timeData = new float [iterations, 3];

			for (int i = 0; i < iterations; i++) {

				startTime.Start ();

				Stopwatch iterationTime = new Stopwatch ();
				iterationTime.Start ();

				Console.WriteLine (" Iteration: {0}", i);
				Console.WriteLine (" Start iteration time: {0}", DateTime.Now.ToString ());
				Console.WriteLine (" Running for: {0}\n", startTime.ElapsedMilliseconds.ToString ());

				int color = random.Next (2, maxColor);
				timeData [i, 0] = color;

				Stopwatch popTime = new Stopwatch ();
				popTime.Start ();
				Interpreter.Excecute (string.Format ("Pop new -i -s:({0}) p rand {1}", size, color));
				popTime.Stop ();
				timeData [i, 1] = (float)popTime.ElapsedMilliseconds;

				iterationTime.Stop ();
				timeData [i, 2] = (float)iterationTime.ElapsedMilliseconds;

				startTime.Stop ();

			}

			Console.WriteLine (" Testing Done\n Analyzing ...\n");
			Console.WriteLine (" Total time: {0}\n", startTime.ElapsedMilliseconds.ToString ());

			double[] mean = new double [3] { 0D, 0D, 0D };

			for (int x = 0; x < iterations; x++) {
				for (int y = 0; y < 3; y++) {
					mean [y] += (double)timeData [x, y] / (double)iterations;
				}
			}

			Console.WriteLine (" Means:");
			Console.WriteLine (" Rand color: {0}", mean [0]);
			Console.WriteLine (" Pop build time: {0}", mean [1]);
			Console.WriteLine (" Iterations: {0}\n", mean [2]);

			double[] standardDeviation = new double [3] { 0D, 0D, 0D };

			for (int y = 0; y < 3; y++) {

				double sigma = 0;
				for (int x = 0; x < iterations; x++) {
					sigma += Math.Pow(timeData [x, y] - mean [y], 2);
				}

				standardDeviation [y] = Math.Sqrt (sigma / iterations);
			}

			Console.WriteLine (" Standard Deviations:");
			Console.WriteLine (" Rand color: {0}", standardDeviation [0]);
			Console.WriteLine (" Pop build time: {0}", standardDeviation [1]);
			Console.WriteLine (" Iterations: {0}\n", standardDeviation [2]);

			double[] s = new double [3] { 0D, 0D, 0D };

			for (int y = 0; y < 3; y++) {
				double sigma = 0;
				for (int x = 0; x < iterations; x++) {
					sigma += Math.Pow (timeData [x, y] - (mean [y] / timeData [x, 0]), 2);
				}

				s [y] = Math.Sqrt(sigma / (double)(iterations - 1));

			}

			double otherSigma = 0;

			for (int x = 0; x < iterations; x++) {
				double[] individual = new double [3];
				for (int y = 0; y < 3; y++) {
					individual [y] = (double)(timeData [x, y] - (mean [y] / timeData [x, 0])) / s [y];
				}
				otherSigma += individual [0] * individual [1];
			}

			double r = otherSigma / (double)(iterations - 1);

			Console.WriteLine (" Hang correlation between \"Rand color\" and \"Pop build time\": {0}\n", r);

		}

	}

}
