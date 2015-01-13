using System;
using CellularAutomata.Populations.Cells;  // reference CellsArangement, CellsVariety
using CellularAutomata.Populations.Rules;  // reference IRule

namespace CellularAutomata.Populations  // contains all Cell collections and Cells namespace
{

	public interface IPopulation  // all base and abstract classes implement IPopulation
	{

		void Evolve ();  // applies simple rule to members
		States GetStates ();  // returns States structure
		void SetRule (IRule rule);  // sets rule
		IRule GetRule ();  // gets rule
		CellsVariety GetCellsVariety ();  // gets type of cell
		CellsArangement GetCellsArangement ();
		string ToString ();  // returns (type, CellsVariety, IRule) as string
		IPopulation Clone ();  // not IPopulation

	}

}

