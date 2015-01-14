using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{

	public class Totalistic : IRule
	{

		public Totalistic () { ; }

		// initalization dependacy, interprets rule as string
		public void Parse (string code)
		{

			string[] phrases = code.Split (new char [3] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);  // split into { "color", "number" }

			try {

				this.color = Convert.ToInt32 (phrases [0]);
				this.number = Convert.ToInt64 (phrases [1]);

			} catch (IndexOutOfRangeException) {

				throw new FormatException ();

			}

			this.code = code;  // only if no exceptions

		}

		// implements rule, returns new state value
		public int Implement (ICell cell)
		{

			long state;

			int total = cell.GetNeighbourhood (1);

			// records upper place value
			long placeValue = (long)Math.Pow (color, total + 1);
			// remaining value to be eliminated
			state = this.number % placeValue;
			// assign lower place value
			placeValue = placeValue / color;
			state = state / placeValue;

			return (int)state;

		}

		// returns properly formated rule
		public new string ToString ()
		{
		
			return this.code;

		}

		public IRule Clone ()
		{
			return (new Totalistic (this.color, this.number, this.code));
		}

		private int color;
		private long number;
		private string code;

		private Totalistic (int color, long number, string code)
		{
			this.color = color;
			this.number = number;
			this.code = code;
		}
	
	}  // end Totalistic, public class

}  // end CellularAutomata.Populations, namespace

