using System;

namespace CellularAutomata.Fluids.Hexagonal
{

	public class Cell
	{

		public byte State {
			get { return this.state; }
			set { this.state = value; }
		}

		public Cell (byte state)
		{
			this.state = state;
			this.neighbours = new Cell [6];
		}

		private byte state;
		private Cell[] neighbours;

		public static void Init ()
		{
			digits = new byte [8];
			digits [0] = (byte)1;
			for (int i = 1; i < digits.Length; i++) {
				digits [i] = (byte)2 * digits [i - 1];
			}

			rule = new byte [256, 2];
			for (byte state = 0; state < (byte)rule.GetLength (0); state++) {
				byte[] states = new byte [8];
				for (int ie = 0; ie < states.Length; ie++) {
					states [ie] = (state & digits [ie]) >> ie;
				}
			}
		}

		private static byte Presence (byte state, int place)
		{

		}

		private static byte Receive (Cell cell)
		{
			return (
				(cell.neighbours [0].state & digits [0])
				| (cell.neighbours [1].state & digits [1])
				| (cell.neighbours [2].state & digits [2])
				| (cell.neighbours [3].state & digits [3])
				| (cell.neighbours [4].state & digits [4])
				| (cell.neighbours [5].state & digits [5])
				| (cell.state & digits [6])
				| (cell.state & digits [7])
				);
		}

		private static byte Interact (byte state)
		{

		}

		private static byte[] digits;
		private static byte[] rule;
	
	}

}

