using System;

namespace CellularAutomata.Commands
{
	
	public struct Option
	{
		
		public string name;
		public string[] arguments;
		
		public Option (string name, string[] arguments)
		{
			
			this.name = name;
			this.arguments = arguments;
			
		}
		
	}
	
}
