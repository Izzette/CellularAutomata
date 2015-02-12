using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{
	public class Hexagonal : ICell
	{

		public static CellsArangement Arangement {
			get { return CellsArangement.TwoDHexagonal; }
		}

		public static Hexagonal[] Build (int[] sizes, ushort[] values)
		{
			if ((0 != (sizes [0]) % 2) || (0 != (sizes [1]) % 2)) {
				throw new ArgumentException ();
			}
			Hexagonal[,] cells = new Hexagonal[sizes [0], sizes [1]];
			Hexagonal[] items = new Hexagonal[cells.Length];
			int numberOfJs = (sizes [1] / sizes [0]) + 1;
			for (int i = 0; i < sizes [0]; i++) {
				for (int j = 0; j < sizes [1]; j++) {
					int number = ((i + (numberOfJs * sizes [0]) - j + (j / 2)) % sizes [0]) + (j * sizes [0]);
					cells [i, j] = new Hexagonal (values [number]);
					items [number] = cells [i, j];
				}
			}
			for (int i = 0; i < sizes [0]; i++) {
				for (int j = 0; j < sizes [1]; j++) {
					int left = (i + sizes [0] - 1) % sizes [0];
					int right = (i + 1) % sizes [0];
					int up = (j + sizes [1] - 1) % sizes [1];
					int down = (j + 1) % sizes [1];
					cells [i, j].neighbours [0] = cells [left, j];
					cells [i, j].neighbours [1] = cells [left, up];
					cells [i, j].neighbours [2] = cells [i, up];
					cells [i, j].neighbours [3] = cells [right, j];
					cells [i, j].neighbours [4] = cells [right, down];
					cells [i, j].neighbours [5] = cells [i, down];
				}
			}
			return items;
		}

		public Hexagonal[] GetNeighbours ()
		{
			return this.neighbours;
		}

		public ICell GetNext ()
		{
			return this.next;
		}

		public ushort GetState ()
		{
			return this.state;
		}

		public void SetState (ushort state)
		{
			this.state = state;
		}

		public CellsArangement GetArangement ()
		{
			return Hexagonal.Arangement;
		}

		public int GetNeighbourhood (int color)
		{
			int neighbourhood = this.state;
			for (int i = 0; i < this.neighbours.Length; i++) {
				neighbourhood += this.neighbours [i].state * (int)Math.Pow (color, i + 1);
			}
			return neighbourhood;
		}

		private ushort state;
		private Hexagonal[] neighbours;
		private Hexagonal next;

		private Hexagonal (ushort state)
		{
			this.state = state;
			this.neighbours = new Hexagonal[6];
			for (int i = 0; i < 2; i++) {
				this.neighbours [i * 3] = this;
			}
		}

	}

}

