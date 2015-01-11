using System;
using CellularAutomata.Populations;  // reference Variety
using CellularAutomata.Populations.Rules;  // reference IRule

namespace CellularAutomata.Populations  // contains all Cell collections and Cells namespace
{

	public interface IPopulation : ICloneable  // all base and abstract classes inherite IPopulation
	{

		void Evolve ();  // applies simple rule to members
		int[] GetStates (out int[] size);  // out states array, out size array
		void SetRule (IRule rule);  // sets rule
		IRule GetRule ();  // gets rule
		Variety GetVariety ();  // gets variety

		new object Clone ();  // inherit from ICloneable

	}

}

