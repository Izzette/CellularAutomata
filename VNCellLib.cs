using System;
using CellularAutomata.Cells;

namespace CellularAutomata.Cells
{
	
	public class VNCell : ICell
	{

		private int state;

		private VNCell left;
		private VNCell up;
		private VNCell right;
		private VNCell down;
		private VNCell next;

		public VNCell (int state)
		{

			this.state = state;
			this.left = this;
			this.up = this;
			this.right = this;
			this.down = this;

		}

		public ICell GetNext ()
		{

			return this.next;

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

			int[] neighboursState = new int[5] {
				
				this.state,
				this.left.state,
				this.up.state,
				this.right.state,
				this.down.state

			};

			int neighbourhood = 0;

			for (int i = 0; i < 5; i++) {

				neighbourhood += neighboursState [i] * (int)Math.Pow (place, i);

			}

			return neighbourhood;

		}

		public VNCell AddNeighbour (ref VNCell cell, bool edge)
		{
		
			VNCell up = this.up;
			VNCell right = this.right;
			VNCell down = this.down;
		
			switch (edge) {

			case false:

				cell.right = right;
				right.left = cell;
				
				this.right = cell;
				cell.left = this;

				up.right.down = cell;
				cell.up = up.right;

				down.right.up = cell;
				cell.down = down.right;

				this.next = cell;
			
				break;

			default:
				
				cell.down = right.down;
				right.down.up = cell;
				
				cell.up = right;
				right.down = cell;
				
				this.next = cell;

				break;

			}

			return cell;

		}

	}

}
