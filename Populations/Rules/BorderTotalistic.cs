using System;

namespace CellularAutomata.Populations
{
	public class BorderTotalistic : IRule
	{

		public BorderTotalistic () { ; }

		// initalization dependacy, interprets rule as string
		public void Parse (string code)
		{

			// split into { "color", "number" }
			string[] phrases = code.Split (new char [3] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);

			try {

				this.color = Convert.ToInt32 (phrases [0]);
				this.number = Convert.ToInt64 (phrases [1]);

			} catch (IndexOutOfRangeException) {

				throw new FormatException ();

			}

			this.code = code;  // only if no exceptions

		}  // end Parse, public void method

		// implements rule, returns new state value
		public int Implement (ICell cell)
		{

			long state;

			// works with any neighbourhood arangement
			long oldState = (long)(cell.GetState ());
			long total = (long)(cell.GetNeighbourhood (1)) - oldState;
			total = (total * this.color) + oldState;

			// records upper place value
			long placeValue = (long)Math.Pow (this.color, total + 1);
			// remaining value to be eliminated
			state = this.number % placeValue;
			// assign lower place value
			placeValue = placeValue / color;
			state = state / placeValue;

			return (int)state;

		} // end Implement, public virtual int method

		// returns properly formated rule
		public new string ToString ()
		{

			return "bt" + this.code;

		}

		public virtual IRule Clone ()
		{
			return (new BorderTotalistic (this.color, this.number, this.code));
		}

		private int color;
		private long number;
		private string code;

		private BorderTotalistic (int color, long number, string code)
		{
			this.color = color;
			this.number = number;
			this.code = code;
		}
	
	} // end BorderToatlistic

} // end CellularAutomata.Populations, namespace

