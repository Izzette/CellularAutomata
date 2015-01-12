using System;
using CellularAutomata.Populations;
using CellularAutomata.Populations.Cells;  // reference ICell

namespace CellularAutomata.Populations.Rules  // contains rules
{

	public interface IRule : ICloneable  // rule interface, all base and abstract classes inherit
	{

		void Parse (string rule);  // initalization dependacy, interprets rule as string
		int Implement (ICell cell);  // implements rule, returns new state value
		string ToString ();  // returns properly formated rule

		new object Clone (); // inherited from ICloneable

	}

}

