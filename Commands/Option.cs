using System;

namespace CellularAutomata.Commands  // console UI interface
{
	
	public struct Option
	{
		
		private string name;
		public string Name {
			get { return this.name; }
			set { ; }
		}
		private string[] arguments;
		public string[] Arguments {
			get { return this.arguments; }
			set { ; }
		}
		
		public Option (string phrase)
		{

			string[] elements = phrase.Split (new char [2] { '-', ':' }, StringSplitOptions.RemoveEmptyEntries);

			this.name = elements [0];

			this.arguments = new string [elements.Length - 1];

			for (int i = 1; i < elements.Length; i++) {
				this.arguments [i - 1] = elements [i];
			}
			
		}
		
	}
	
}
