using System;
using System.Numerics;

namespace CellularAutomata.Cells
{

	public interface ICell
	{
		
		ICell GetNext ();
		int GetState ();
		void SetState (int state);
		int GetNeighbourhood (BigInteger place);
		
	}
	
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
		
		public int GetNeighbourhood (BigInteger place)
		{
			
			int[] neighboursState = new int[3] {
				
				this.zero.state,
				this.state,
				this.two.state
				
			};
			
			BigInteger neighbourhood = 0;
			
			for (int i = 0; i < 3; i++) {
				
				neighbourhood += neighboursState [i] * BigInteger.Pow (place, i);
				
			}

			return (int)neighbourhood;
			
		}
		
		public ElementaryCell AddNeighbour (ref ElementaryCell cell)
		{
			
			this.two = cell;
			cell.zero = this;
			
			return cell;
			
		}

	}

	public class VNCell : ICell
	{

		private int state;

		private VNCell zero;
		private VNCell one;
		private VNCell three;
		private VNCell four;
		private VNCell next;

		public int Next  {

			get { return this.next.state; }
			set { this.next.state = value; }

		}

		public VNCell (int state)
		{

			this.state = state;
			this.zero = this;
			this.one = this;
			this.three = this;
			this.four = this;
			this.next = this.four;

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

		public int GetNeighbourhood (BigInteger place)
		{

			int[] neighboursState = new int[5] {

				this.zero.state,
				this.one.state,
				this.state,
				this.three.state,
				this.four.state

			};

			BigInteger neighbourhood = 0;

			for (int i = 0; i < 5; i++) {

				neighbourhood += neighboursState [i] * BigInteger.Pow (place, i);

			}

			return (int)neighbourhood;

		}

		public VNCell AddNeighbour (ref VNCell cell, bool edge)
		{

			VNCell tempZero = this.zero;
			VNCell tempOne = this.one;
			VNCell tempThree = this.three;
			VNCell tempFour = this.four;
		
			switch (edge) {

			case false:

				cell.zero = this;
				this.four = cell;

				cell.one = tempOne.four;
				tempOne.four.three = cell;

				cell.three = tempThree.four;
				tempThree.four.one = cell;

				cell.four = tempFour;
				tempFour.zero = cell;

				this.next = cell;
			
				break;

			default:

				cell.one = tempFour;

				cell.three = tempFour.three;

				tempFour.three = cell;

				this.next = cell;

				break;

			}

			return cell;

		}

	}

}
