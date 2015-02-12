using System;
using System.Numerics;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations  // contains rules
{	
	
	public class Absolute : IRule
	{

		public Absolute () { ; }

		// Intialization Dependancy
		// string rule = "[color,number]"
		// inherit IRule
		public void Parse (string code)
		{

			string[] phrases = code.Split (new char [3] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);  // split into { "color", "number" }

			try {  // safing convert

				this.color = Convert.ToInt32 (phrases [0]);
				this.number = BigInteger.Parse (phrases [1]);

			} catch (IndexOutOfRangeException) {

				throw new FormatException ();

			}

			this.code = code;  // only if no exceptions

		}

		public ushort Implement (ICell cell)  // inherit IRule
		{

			BigInteger state;

			int neighbourhood = cell.GetNeighbourhood (this.color);  // gets neighbourhood

			BigInteger color = new BigInteger (this.color);
			// records upper place value
			BigInteger placeValue = BigInteger.Pow (color, neighbourhood + 1);
			// remaining value to be eliminated
			state = BigInteger.Remainder (this.number, placeValue);

			placeValue = BigInteger.Divide (placeValue, color);  // lower place value
			state = BigInteger.Divide (state, placeValue);  // final answer as BigInteger

			return (ushort)state;  // return state

		}

		public new string ToString ()  // inherit IRule
		{

			return "a" + this.code;

		}

		public IRule Clone ()  // inherit IRule
		{

			return (new Absolute (this.color, this.number, this.code));

		}
		
		private int color;
		private BigInteger number;
		private string code;

		private Absolute (int color, BigInteger number, string code)  // private constructor for clone
		{

			this.color = color;
			this.number = number;
			this.code = code;

		}

	}
	
}
