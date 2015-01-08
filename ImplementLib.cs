using System;
using System.Numerics;
using CellularAutomata.Rules;

namespace CellularAutomata.Rules
{

	public class Implement
	{
	
		public static int Absolute (int neighbourhood, Rule rule)
		{
			
			return (int)((rule.number % BigInteger.Pow (rule.place, neighbourhood + 1)) / BigInteger.Pow (rule.place, neighbourhood));
			
		}
	
	}

}
