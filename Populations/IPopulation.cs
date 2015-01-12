using System;
using CellularAutomata.Populations.Cells;  // reference Arangements, Variety
using CellularAutomata.Populations.Rules;  // reference IRule

namespace CellularAutomata.Populations  // contains all Cell collections and Cells namespace
{

	public interface IPopulation : ICloneable  // all base and abstract classes inherite IPopulation
	{

		void Evolve ();  // applies simple rule to members
		States GetStates ();  // returns States structure
		void SetRule (IRule rule);  // sets rule
		IRule GetRule ();  // gets rule
		Variety GetVariety ();  // gets variety
		string ToString ();  // returns type, Variety, IRule as string

		new object Clone ();  // inherit from ICloneable

	}

}

