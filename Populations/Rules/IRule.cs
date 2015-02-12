using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations  // contains rules
{

	public interface IRule  // rule interface, all base and abstract classes inherit
	{

		void Parse (string code);  // initalization dependacy, interprets rule as string
		ushort Implement (ICell cell);  // implements rule, returns new state value
		string ToString ();  // returns properly formated rule
		IRule Clone ();

	}

}

