using System;
using CellularAutomata.Populations.Cells;  // reference ICell

namespace CellularAutomata.Populations.Cells  // contains Cell Classes
{
	
	public class General : ICell  // one dimensional, one neighbour, one state
	{

		public static ICell[] Build (int[] size, out int[] states) // overload, constructs simple initial condition -> out states
		{

			if (3 > size [0]) { // safing length, prevent degenerate case

				throw new ArgumentException ("new int (3) is not greater than or equal to parameter (int[] size) [0] !");

			} else if (1 != size.Length) {  // safing number of dimensions

				throw new ArgumentException ("new int (1) does not have the same value as parameter (int[] size).Length !");

			}

			states = new int [size [0]];  // initialize states

			states [0] = 1;  // set seed to 1

			for (int i = 1; i < size [0]; i++) {  // start at 1 because states

				states [i] = 0;  // set all other states to 0

			}

			return General.Build (size, states);  // return overload. exit

		}

		public static ICell[] Build (int[] size, int[] states)  // constructs network; only size [0] is used
		{
			
			if (1 != size.Length) {  // safing number of dimensions

				throw new ArgumentException ("new int (1) does not have the same value as parameter (int[] size).Length !");

			} else if (3 > size [0]) { // safing length, prevent degenerate case

				throw new ArgumentException ("new int (3) is not greater than or equal to parameter (int[] size) [0] !");

			} else if (size [0] != states.Length) {  // safing size and states matchup

				throw new ArgumentException ("parameter (int[] size) [0] does not have the same value as parameter (int[] states).Length !");

			}

			General[] items = new General [size [0]];  // create current to reference cell to be processed
			items [0] = new General (states [0]);  //  create root from specified states

			for (int i = 1; i < size [0]; i++) {  // starts at 1 because root has already been created

				items [i] = items [i - 1].AddNeighbour (states [i]);  // set new neighbour at specified state, save to next value of items

			}

			return items;  // return cells in array

		}
		
		private int state; // color as int

		// scan order right, this, left
		private General right;  // graphical right
		private General left;  // graphical left

		private General next; // next cell in line

		public General (int state)  // Constructor, sets this state to state
		{
			
			this.state = state;  // set state

			// bite around, degenerative case
			this.left = this;
			this.right = this;

			// this.next not initalized for safety

		}

		public ICell GetNext ()  // inherited from ICell
		{
			
			return this.next;  // returns next in scan; exit;
			
		}

		public int GetState ()  // inherited from ICell
		{
			
			return this.state;  // returns state. exit
			
		}

		public void SetState (int state)  // inherited from ICell
		{
			
			this.state = state;  // set state

		}
		
		public int GetNeighbourhood (int color)  // search right, this, left
		{
			
			int[] neighboursState = new int [3] {  // collection of states, for code reuse
				
				this.right.state,
				this.state,
				this.left.state
				
			};
			
			int neighbourhood = 0;  // neighbourhood number
			
			for (int i = 0; i < neighboursState.Length; i++) {  // loops through states {right, this, left}
				
				neighbourhood += neighboursState [i] * (int)Math.Pow (color, i);  // asign digit value to neighbourhood
				
			}

			return neighbourhood;  // return neighbourhood. exit
			
		}
		
		public General AddNeighbour (int state)  // Adds neighbour with passed state, returns cell added
		{

			General cell = new General (state);  // construct new cell

			General tempRight = this.right;  // temporary reference holder

			// doubly link this <=> cell
			this.right = cell;
			cell.left = this;

			// doubly link cell <=> tempRight
			cell.right = tempRight;
			tempRight.left = cell;

			this.next = cell; // will be next cell
			
			return cell;  // return the next cell to be processed (the new cell). exit

		}

	}

}
