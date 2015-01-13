using System;
using System.Numerics;  // reference System.Numerics
using CellularAutomata.Populations;
using CellularAutomata.Populations.Cells;  // reference ICell
using CellularAutomata.Populations.Rules;  // reference IRule

namespace CellularAutomata.Populations.Rules  // contains rules
{	
	
	public class Absolute : IRule
	{

		private int color;
		private BigInteger number;
		private string rule;

		private Absolute (int color, BigInteger number, string rule)  // private constructor for clone
		{

			this.color = color;
			this.number = number;
			this.rule = rule;

		}

		public Absolute () { ; }

		// Intialization Dependancy
		// string rule = "[color,number]"
		// inherit IRule
		public void Parse (string rule)
		{

			string[] phrases = rule.Split (new char [3] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);  // split into { "color", "number" }

			try {  // safing convert

				this.color = Convert.ToInt32 (phrases [0]);
				this.number = BigInteger.Parse (phrases [1]);

			} catch (FormatException) {

				throw new ArgumentException ("Waring: rule not in correct format! Should be (<k>,<n>)");

			}

			this.rule = rule;  // only if no exceptions

		}

		public int Implement (ICell cell)  // inherit IRule
		{

			int neighbourhood = cell.GetNeighbourhood (this.color);  // gets neighbourhood

			BigInteger color = new BigInteger (this.color);

			BigInteger placeValue = BigInteger.Pow (color, neighbourhood + 1);  // records upper place value
			BigInteger remaining = BigInteger.Remainder (this.number, placeValue);  // remaining value to be eliminated

			placeValue = BigInteger.Divide (placeValue, color);  // lower place value
			remaining = BigInteger.Divide (remaining, placeValue);  // final answer as BigInteger

			int state = (int)remaining;  // convert to int

			return state;  // return state

		}

		public new string ToString ()  // inherit IRule
		{

			return this.rule;

		}

		public IRule Clone ()  // inherit IRule
		{

			return (new Absolute (this.color, this.number, this.rule));

		}
		
	}
	
}
