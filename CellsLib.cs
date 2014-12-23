using System;

namespace CellularAutomata.Cells
{

	public interface ICell
	{
		ICell GetNext ();
		ulong[] GetPositions ();
		ulong GetState ();
		void SetState (ulong state);
		ulong GetNeighbourhood (ulong place);
	}
	
	public class ElementaryCell : ICell
	{
		private ulong state;
		public ulong[] positions = new ulong[2] {0, 2};

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

		public ElementaryCell (ulong state)
		{
			this.state = state;
			this.zero = this;
			this.two = this;
		}

		public ICell GetNext ()
		{
			return this.Next;
		}

		public ulong[] GetPositions ()
		{
			return this.positions;
		}

		public ulong GetState ()
		{
			return this.state;
		}

		public void SetState (ulong state)
		{
			this.state = state;
		}
		
		public ulong GetNeighbourhood (ulong place)
		{
			ulong[] neighboursState = new ulong[3] {
				this.zero.state,
				this.state,
				this.two.state
			};
			
			ulong neighbourhood = 0;
			
			for (int i = 0; i < 3; i++) {
				
				neighbourhood += neighboursState [i] * Convert.ToUInt64 (Math.Pow (place, i));
				
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
