using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{

	public class LifeLike : IRule
	{

		public LifeLike () { ; }

		// initalization dependacy, interprets rule as string
		public void Parse (string code)
		{

			// split into { "color", "survive", "birth" }
			string[] phrases = code.Split (new string [3] { "({", "})", "},{" }, StringSplitOptions.RemoveEmptyEntries);

			string[] surviveStrings;
			string[] birthStrings;

			try {
				surviveStrings = phrases [0].Split (new char [1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				birthStrings = phrases [1].Split (new char [1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			} catch (IndexOutOfRangeException) {
				throw new FormatException ();
			}

			this.survive = 0;

			foreach (string s in surviveStrings) {
				int n = Convert.ToInt32 (s);
				this.survive += (int)Math.Pow (2, n);
			}

			this.birth = 0;

			foreach (string s in birthStrings) {
				int n = Convert.ToInt32 (s);
				this.birth += (int)Math.Pow (2, n);
			}

			this.code = code;

		} // end Parse, public void method

		// implements rule, returns new state value
		public int Implement (ICell cell)
		{

			int state;

			// works with any neighbourhood arangement
			int oldState = cell.GetState ();
			int total = cell.GetNeighbourhood (1) - oldState;

			int number;

			if (1 == oldState) {
				number = survive;
			} else {
				number = birth;
			} // end if (1 == oldState) else, statment

			// records upper place value
			int placeValue = (int)Math.Pow (2, total + 1);
			// remaining value to be eliminated
			state = number % placeValue;
			// assign lower place value
			placeValue = placeValue / 2;
			state = state / placeValue;

			return state;

		} // end Implement, public virtual int method

		// returns properly formated rule
		public new string ToString ()
		{

			return "ll" + this.code;

		}

		public virtual IRule Clone ()
		{
			return (new LifeLike (this.survive, this.birth, this.code));
		}

		private int survive;
		private int birth;

		private string code;

		private LifeLike (int survive, int birth, string code)
		{
			this.survive = survive;
			this.birth = birth;
			this.code = code;
		}
	
	}

}

