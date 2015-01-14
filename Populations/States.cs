using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{

	public struct States
	{

		private CellsArangement arangement;
		public CellsArangement Arangement {
			get { return this.arangement; }
			set { ; }
		}
		private int[] values;
		public int[] Values {
			get { return this.values; }
			set { this.values = value; }
		}
		private int[] sizes;
		public int[] Sizes {
			get { return this.sizes; }
			set { ; }
		}

		public int Length {
			get { return values.Length; }
			set { ; }
		}

		// constructor
		public States (CellsArangement arangement, int[] values, int[] sizes)
		{

			int length = 1;

			foreach (int n in sizes) {
				length = length * n;
			}

			if (length != values.Length) {

				int[] tempValues = new int [length];

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
