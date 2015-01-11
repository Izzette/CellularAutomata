using System;
using CellularAutomata.Populations.Cells;  // reference ICellLib

namespace CellularAutomata.Rules  // contains rules
{

	public interface IRule : ICloneable  // rule interface, all base and abstract classes inherit
	{

		void Parse (string rule);  // initalization dependacy, interprets rule as string
		int Implement (ICell cell);  // implements rule, returns new state value
		string GetRule ();  // returns properly formated rule

		new object Clone (); // inherited from ICloneable

	}

}

