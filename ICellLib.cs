using System;

namespace CellularAutomata.Cells
{	
	
	public interface ICell
	{
		
		ICell GetNext ();
		int GetState ();
		void SetState (int state);
		int GetNeighbourhood (int place);
		
	}
	
}
