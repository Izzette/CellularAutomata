using System;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations
{
	public class Hexagonal : ICell
	{

		public static CellsArangement Arangement {
			get { return CellsArangement.TwoDHexagonal; }
			set { ; }
		}

		public static Hexagonal[] Build (int[] sizes, int[] values)
		{
			if (0 != (sizes [1]) % 2) {
				throw new ArgumentException ();
			}
			Hexagonal[][] rows = new Hexagonal [sizes [1]][];
			for (int i = 0; i < sizes [1]; i++) {
				int[] rowValues = new int [sizes [0]];
				Array.ConstrainedCopy (values, i * sizes [0], rowValues, 0, sizes [0]);
				rows [i] = ConstructRow (sizes [0], rowValues);
			}
			return AssembleRows (rows);
		}

		public Hexagonal[] GetNeighbours ()
		{
			return this.neighbours;
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

		private static Hexagonal[] ConstructRow (int length, int[] values)
		{
			Hexagonal[] row = new Hexagonal [length];
			row [0] = new Hexagonal (values [0]);
			for (int i = 1; i < row.Length; i++) {
				row [i] = row [i - 1].AddNeighbour (values [i]);
			}
			return row;
		}

		private static Hexagonal[] AssembleRows (Hexagonal[][] rows)
		{
			int rowLength = rows [0].Length;
			int numberRows = rows [1].Length;
			Hexagonal[] items = new Hexagonal [rowLength * numberRows];
			for (int i = 0; i < items.Length; i++) {
				int[] upperIndex = new int [2] {
					(i / rowLength),
					(i % rowLength)
				};
				int[,] lowerIndex = new int [2, 2] {
					{
						((upperIndex [0] + 1) % numberRows),
						((upperIndex [1] + rowLength + 1 - ((upperIndex [0] + 1) % 2)) % rowLength)
					},
					{
						((upperIndex [0] + 1) % numberRows),
						((upperIndex [1] + rowLength - ((upperIndex [0] + 1) % 2)) % rowLength)
					}
				};
				Hexagonal upperCell = rows [upperIndex [0]][upperIndex [1]];
				items [i] = upperCell;
				for (int ie = 0; ie < 2; ie++) {
					Hexagonal addCell = rows [lowerIndex [ie, 0]][lowerIndex [ie, 1]];
					upperCell.neighbours [ie + 4] = addCell;
					addCell.neighbours [ie + 1] = upperCell;
				}
				if ((0 == (i + 1) % rowLength) && (items.Length != i + 1)) {
					upperCell.next = rows [upperIndex [0] + 1] [0];
				}
			}
			return items;
		}

		private int state;
		private Hexagonal[] neighbours;
		private Hexagonal next;

		private Hexagonal (int state)
		{
			this.state = state;
			this.neighbours = new Hexagonal[6];
			for (int i = 0; i < 2; i++) {
				this.neighbours [i * 3] = this;
			}
		}

		private Hexagonal AddNeighbour (int state)
		{
			Hexagonal newCell = new Hexagonal (state);
			Hexagonal tempN3 = this.neighbours [3];
			newCell.neighbours [0] = this;
			this.neighbours [3] = newCell;
			newCell.neighbours [3] = tempN3;
			tempN3.neighbours [0] = newCell;
			this.next = newCell;
			return newCell;
		}

	}

}

