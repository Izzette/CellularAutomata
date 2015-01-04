using System;
using System.Numerics;

namespace CellularAutomata.Rules
{	
	
	public struct Rule
	{
		
		public BigInteger number;
		public int place;
		
		public Rule (BigInteger number, int place)
		{
			
			this.number = number;
			this.place = place;
			
		}
		
	}
	
}
