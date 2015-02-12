using System;

namespace CellularAutomata.Populations
{	
	
	public interface ICell  // ICell interface, all Cells base classes inherite ICell
	{
		
		ICell GetNext ();  // returns next ICell in scan
		ushort GetState ();  // returns state
		void SetState (ushort state);  // sets state
		CellsArangement GetArangement ();  // return gemometric arangement
		int GetNeighbourhood (int color);  // returns neighbourhood, color is the number of colors
		
	}
	
}
