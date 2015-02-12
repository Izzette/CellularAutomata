using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{

	public struct States
	{

		private CellsArangement arangement;
		public CellsArangement Arangement {
			get { return this.arangement; }
		}
		ushort[] values;
		public ushort[] Values {
			get {
				return values;
			}
		}
		private readonly int[] sizes;
		public int[] Sizes {
			get { return this.sizes; }
		}

		public int Length {
			get { return Values.Length; }
		}

		// constructor
		public States (CellsArangement arangement, ushort[] values, int[] sizes)
		{

			int length = 1;

			foreach (int n in sizes) {
				length = length * n;
			}

			if (length != values.Length) {

				ushort[] tempValues = new ushort[length];

				for (int i = 0; i < tempValues.Length; i++) {

					try {
						tempValues [i] = values [i];
					} catch (IndexOutOfRangeException) {
						tempValues [i] = 0;
					}

				}

				values = tempValues;

			}

			this.arangement = arangement;
			this.values = values;
			this.sizes = sizes;

		} // end States, public constructor

	}  // end States, public struct

// end CellularAutomata.Populations, namespace
}
