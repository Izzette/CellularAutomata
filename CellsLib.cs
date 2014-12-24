using System;
using System.Numerics;

namespace CellularAutomata.Cells
{

	public interface ICell
	{
		
		ICell GetNext ();
		int[] GetPositions ();
		int GetState ();
		void SetState (int state);
		int GetNeighbourhood (BigInteger place);
		
	}
	
	public class ElementaryCell : ICell
	{
		
		private int state;
		public int[] positions = new int[2] {0, 2};

		private ElementaryCell zero;
		public ElementaryCell Zero {
			
			get { return this.zero; }
			set { this.zero = value; }
			
		}
		
		private ElementaryCell two;
		public ElementaryCell Two {
			
			get { return this.two; }
			set { this.two = value; }
			
		}
		
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

		public int[] GetPositions ()
		{
			
			return this.positions;
			
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
}
