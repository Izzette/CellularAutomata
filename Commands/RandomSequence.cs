using System;
using System.Threading;
using System.Threading.Tasks;
using CellularAutomata.Populations;

namespace CellularAutomata.Commands
{

	public static class RandomSequence
	{

		public static bool IsInitialized {
			get { return isInitialized; }
			set { ; }
		}

		public static void Init ()
		{

			Init (2500, 10000, 17, 24);

		}

		public static void Init (int minSize, int maxSize, int minGeneration, int maxGeneration)
		{

			Random random = new Random ();

			int length = 32 * random.Next (minSize, maxSize);
			int[] sizes = new int [1] {length};
			int[] values = new int [length];

			uint[] numbers = new uint [(int)Math.Ceiling ((float)length / 32F)];

			Parallel.For (0, numbers.Length, i => {
				numbers [i] = (uint)random.Next ();
			});

			for (int i = 0; i < length; i++) {
				numbers [i / 32] = (uint)Math.DivRem ((int)numbers [i / 32], 2, out values [i]);
			}

			States states = new States (CellsArangement.OneDCubic, values, sizes);

			population = new Simple (CellsVariety.NextGeneral, states.Sizes, states.Values);
			IRule rule = new Absolute ();
			rule.Parse ("(2,1436965290)");
			population.SetRule (rule);

			int generations = random.Next (minGeneration, maxGeneration);

			for (int i = 0; i < generations; i++) {
				population.Evolve ();
			}

			isInitialized = true;

		}

		public static int[] GetSequence (int color, int length)
		{

			if (!isInitialized) {
				Init ();
			}

			int[] values = new int [length];

			int[] popValues = population.GetStates ().Values;
			int popLength = popValues.Length;

			uint numerical = 0;
			int digit = (int)(Math.Log (UInt32.MaxValue, color) - Math.Log (color, 2));
			int startIndex = 0;

			for (int i = 0; i < length; i++) {
				if (popLength <= startIndex + 32) {
					startIndex = 0;
					for (int ie = 0; ie < 4; ie++) {
						population.Evolve ();
					}
					popValues = population.GetStates ().Values;
				}
				if (0 == i % digit) {
					numerical = 0;
					startIndex += 32;
					for (int ie = 0; ie < 32; ie++) {
						numerical = numerical * 2;
						numerical += (uint)popValues [ie + startIndex];
					}
				}
				values [i] = (int)(numerical % (uint)color);
				numerical = numerical / (uint)color;
			}

			return values;
		
		}

		private static IPopulation population;
		private static bool isInitialized = false;
	
	}

}

