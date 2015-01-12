using System;
using CellularAutomata.Populations.Cells;  // reference Arangements

namespace CellularAutomata.Populations
{

	public struct States
	{

		private Arangements arangement;
		public Arangements Arangement {
			get { return this.arangement; }
			set { ; }
		}
		private int[] values;
		public int[] Values {
			get { return this.values; }
			set { ; }
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
		public States (Arangements arangement, int[] values, int[] sizes)
		{

			int length = 1;

			foreach (int n in sizes) {
				length = length * n;
			}

			if (length != values.Length) {

				throw new ArgumentException ("values and sizes do not match");

			}

			this.arangement = arangement;
			this.values = values;
			this.sizes = sizes;

		} // end States, public constructor

	}  // end States, public struct

// end CellularAutomata.Populations, namespace
}
