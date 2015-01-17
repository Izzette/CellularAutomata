using System;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
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

			Random random = new Random ();

			int length = random.Next (1000, 10000);
			int[] sizes = new int [1] {length};
			int[] values = new int [length];

			int[] numbers = new int [(length / 32) + 1];

			Parallel.For (0, numbers.Length, i => {
				numbers [i] = random.Next ();
			});

			for (int i = 0; i < length; i++) {
				int currentNumber = numbers [i / 32];
				currentNumber = Math.DivRem (currentNumber, 2, out values [i]);
			}

			States states = new States (CellsArangement.OneDCubic, values, sizes);

			population = new Simple (CellsVariety.NextGeneral, states.Sizes, states.Values);
			IRule rule = new Absolute ();
			rule.Parse ("(2,1436965290)");
			population.SetRule (rule);

			int generations = random.Next (100, 200);

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

			BigInteger bigInt = BigInteger.Zero;

			for (int i = 0; i < length; i++) {
				if (0 == i % popLength) {
					for (int ie = 0; ie < 4; ie++) {
						population.Evolve ();
					}
					popValues = population.GetStates ().Values;
					bigInt = BigInteger.Zero;
					for (int ie = 0; ie < popLength; ie++) {
						bigInt += BigInteger.Pow (2, ie) * popValues [ie];
					}
				}
				values [i] = (int)(BigInteger.Remainder (bigInt, BigInteger.Pow (color, (i % popLength) + 1)) / BigInteger.Pow (color, (i % popLength)));
			}

			return values;
		
		}

		private static IPopulation population;
		private static bool isInitialized = false;
	
	}

}

