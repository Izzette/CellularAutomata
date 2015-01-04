using System;
using CellularAutomata.Cells;

namespace CellularAutomata.Cells
{
	
	public class ElementaryCell : ICell
	{
		
		private int state;

		private ElementaryCell zero;
		private ElementaryCell two;

		public ElementaryCell Next  {
			
			get { return this.two; }
			set { ; }
			
		}

		public ElementaryCell (int state)
		{
			
			this.state = state;
			this.zero = this;
			this.two = this;
			
		}

		public ICell GetNext ()
		{
			
			return this.Next;
			
		}

		public int GetState ()
		{
			
			return this.state;
			
		}

		public void SetState (int state)
		{
			
			this.state = state;
			
		}
		
		public int GetNeighbourhood (int place)
		{
			
			int[] neighboursState = new int[3] {
				
				this.zero.state,
				this.state,
				this.two.state
				
			};
			
			int neighbourhood = 0;
			
			for (int i = 0; i < 3; i++) {
				
				neighbourhood += neighboursState [i] * (int)Math.Pow(place, i);
				
			}

			return neighbourhood;
			
		}
		
		public ElementaryCell AddNeighbour (ref ElementaryCell cell)
		{
			
			this.two = cell;
			cell.zero = this;
			
			return cell;
			
		}

	}

}
