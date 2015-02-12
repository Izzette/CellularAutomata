using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{

	public class NextGeneral : ICell
	{

		public static CellsArangement Arangement {
			get { return CellsArangement.OneDCubic; }
		}

		public static ICell[] Build (int[] sizes, ushort[] values)  // constructs network; only size [0] is used
		{

			if (1 != sizes.Length) {  // safing number of dimensions

				throw new ArgumentException ("new int (1) does not have the same value as parameter (int[] size).Length !");

			} else if (3 > sizes [0]) { // safing length, prevent degenerate case

				throw new ArgumentException ("new int (3) is not greater than or equal to parameter (int[] size) [0] !");

			}

			NextGeneral[] items = new NextGeneral [sizes [0]];  // create current to reference cell to be processed
			items [0] = new NextGeneral (values [0]);  //  create root from specified states

			for (int i = 1; i < sizes [0]; i++) {  // starts at 1 because root has already been created

				items [i] = items [i - 1].AddNeighbour (values [i]);  // set new neighbour at specified state, save to next value of items

			}

			return items;  // return cells in array

		}

		public NextGeneral (ushort state)
		{

			this.state = state;

			this.left = this;
			this.right = this;

		}

		public ICell GetNext ()  // inherited from ICell
		{

			return this.next;  // returns next in scan; exit;

		}

		public ushort GetState ()  // inherited from ICell
		{

			return this.state;  // returns state. exit

		}

		public void SetState (ushort state)  // inherited from ICell
		{

			this.state = state;  // set state

		}

		// return gemometric arangement
		public CellsArangement GetArangement ()
		{

			return NextGeneral.Arangement;

		}

		public NextGeneral AddNeighbour (ushort state)  // Adds neighbour with passed state, returns cell added
		{

			NextGeneral cell = new NextGeneral (state);  // construct new cell

			NextGeneral tempRight = this.right;  // temporary reference holder

			// doubly link this <=> cell
			this.right = cell;
			cell.left = this;

			// doubly link cell <=> tempRight
			cell.right = tempRight;
			tempRight.left = cell;

			this.next = cell; // will be next cell

			return cell;  // return the next cell to be processed (the new cell). exit

		}

		public int GetNeighbourhood (int color)  // search right, this, left
		{

			int[] neighboursState = new int [5] {  // collection of states, for code reuse

				this.right.right.state,
				this.right.state,
				this.state,
				this.left.state,
				this.left.left.state

			};

			int neighbourhood = 0;  // neighbourhood number

			for (int i = 0; i < neighboursState.Length; i++) {  // loops through states {right, this, left}

				neighbourhood += neighboursState [i] * (int)Math.Pow (color, i);  // asign digit value to neighbourhood

			}

			return neighbourhood;  // return neighbourhood. exit

		}

		private ushort state; // color as int

		// scan order right, this, left
		private NextGeneral right;  // graphical right
		private NextGeneral left;  // graphical left

		private NextGeneral next; // next cell in line
	
	}

}

