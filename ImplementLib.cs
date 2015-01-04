using System;
using System.Numerics;
using CellularAutomata.Rules;

namespace CellularAutomata.Rules
{

	public class Implement
	{
	
		public static int Absolute (int neighbourhood, Rule rule)
		{
			
			BigInteger upper = rule.number % BigInteger.Pow (rule.place, neighbourhood + 1);

			BigInteger lower = rule.number % BigInteger.Pow (rule.place, neighbourhood);
			BigInteger final = (upper - lower) / BigInteger.Pow (rule.place, neighbourhood);
				
			return (int)final;
			
		}
	
	}

}
