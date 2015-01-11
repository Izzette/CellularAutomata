using System;
using CellularAutomata.Populations.Cells; // reference ICellLib

namespace CellularAutomata.Populations.Cells  // contains Cell classes
{

	public class VonNeumann : ICell  // range 1, Von Neumann neighbourhoods, 2D
	{

		public static ICell[] Build (int[] size, out int[] states)  // overload constructs inital condition -> out states
		{

			if (2 != size.Length) {  // safing number of dimensions

				throw new ArgumentException ("new int (2) does not have the same value as parameter (int[] size).Length !");

			} else if ((3 > size [0]) && (3 > size [1])) { // safing length and height, prevent degenerate case

				throw new ArgumentException ("new int (3) is not greater than or equal to parameter (int[] size) [0] && [1] !");

			}

			states = new int [size [0]];  // initialize states

			states [0] = 1;  // set seed to 1

			for (int i = 1; i < size [0]; i++) {  // start at 1 because states

				states [i] = 0;  // set all other states to 0

			}

			return VonNeumann.Build (size, states);  // return overload. exit


		}

		public static ICell[] Build (int[] size, int[] states)  // constructs network, only size [0 thru 1] are used
		{

			if (2 != size.Length) {  // safing number of dimensions

				throw new ArgumentException ("new int (2) does not have the same value as parameter (int[] size).Length !");

			} else if ((3 > size [0]) || (3 > size [1])) { // safing length and height, prevent degenerate case

				throw new ArgumentException ("new int (3) is not greater than or equal to parameter (int[] size) [0] && [1] !");

			} else if (states.Length != (size [0] * size [1])) {  // safing states size matchup

				throw new ArgumentException ("parameter (int[] size) [0] * [1] does not have the same value as parameter (int[] states).Length !");;

			}

			VonNeumann[] items = new VonNeumann [states.Length];  // construct array for items
			items [0] = new VonNeumann (states [0]);  // create root with specified state

			int index = 2;  // keeps track of total number of cells in system, starts at two because index is incremented at end of nested for loop

			for (int vertical = 0; vertical < size [1]; vertical++) {  // vertical height
			
				for (
					int horizontal = 1;  // start after root or new line
				    (horizontal < size [0] + 1) && (index < states.Length);  // break after new row or after last cell
					horizontal++
					) {

					bool newRow = (horizontal == size [0]);  // true if next cell should start a new row, the row if full
					items [index] = items [index - 1].AddNeighbour (states [index], newRow);  // add neighbour

					index++;  // increment index

				}

			}

			return items;  // return items. exit

		}

		private int state; // this state

		// border neigbhours in scan order, clockwise starting with left, make double links
		private VonNeumann left;
		private VonNeumann up;
		private VonNeumann right;
		private VonNeumann down;

		// next cell reference holder, single links
		private VonNeumann next;

		public VonNeumann (int state)  // constructs with specified state
		{

			this.state = state;  // set specified state

			// in search order, followed by this
			this.left = this;
			this.up = this;
			this.right = this;
			this.down = this;

			// this.next is left null for safing

		}

		public ICell GetNext ()  // inherited from ICell
		{

			return this.next;  // return next cell. exit

		}

		public int GetState ()  // inherited from ICell
		{

			return this.state;  // return state. exit

		}

		public void SetState (int state)  // inherited from ICell
		{

			this.state = state;  // set state. exit

		}

		public int GetNeighbourhood (int color)  // search border neighbours clockwise starting with left, end with this
		{

			int[] neighboursState = new int [5] {  // colection of states for code reuse 
				
				this.state,
				this.left.state,
				this.up.state,
				this.right.state,
				this.down.state

			};

			int neighbourhood = 0;  // neighbourhood number

			for (int i = 0; i < neighboursState.Length; i++) {  // loop through

				neighbourhood += neighboursState [i] * (int)Math.Pow (color, i); // apply place value

			}

			return neighbourhood;  // return neighbourhood number. exit

		}

		public VonNeumann AddNeighbour (int state, bool newRow) // Adds neighbour with passed state, starts new roow if (newRow) returns cell added
		{

			VonNeumann cell = new VonNeumann (state);  // construct new cell

			// temporary reference holders
			VonNeumann tempUp = this.up;  
			VonNeumann tempRight = this.right;
			VonNeumann tempDown = this.down;

			VonNeumann tempRoot = this.right.down;  // temporary reference to root, for newRow
		
			if (newRow) {  // new row

				// does not link to this cell, except singly through this.next;

				// doubly link cell <=> tempRight (under this row root)
				cell.up = tempRight;
				tempRight.down = cell;

				// doubly link cell <=> tempRoot (previously tempRight.down)
				cell.down = tempRoot;
				tempRoot.up = cell;

			} else {  // not a new row

				// doubly link this <=> cell
				this.right = cell;
				cell.left = this;

				// doubly link cell <=> tempUp.right (overhanging cell)
				cell.up = tempUp.right;
				tempUp.right.down = cell;

				// doubly link cell <=> tempRight
				cell.right = tempRight;
				tempRight.left = cell;

				// doubly link cell <=>  tempDown.right (underhanging cell)
				cell.down = tempDown.right;
				tempDown.right.up = cell;

			}

			this.next = cell;  // will be next cell

			return cell;  // return next cell to be processed. exit

		}

	}

}
