using System;

namespace CellularAutomata.Cells
{

	public interface ICell
	{
		ICell GetNext ();
		int[] GetPositions ();
		int GetState ();
		void SetState (int state);
		int GetNeighbourhood ();
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
		
		public int GetNeighbourhood ()
		{
			int[] neighboursState = new int[3] {
				this.zero.state,
				this.state,
				this.two.state
			};
			
			int neighbourhood = 0;
			
			for (int i = 0; i < 3; i++) {
				neighbourhood += Convert.ToInt32 (neighboursState [i] * Math.Pow (2, i));
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
