using System;

namespace CellularAutomata.Fluids.Hexagonal
{

	class Cell
	{

		public int State {
			get { return this.state; }
			set { this.state = value; }
		}

		public Cell (int state)
		{
			this.state = state;
			this.neighbours = new Cell [6];
		}

		private int state;
		private Cell[] neighbours;

		public static void Init ()
		{

			random = new Random ();

			digits = new int [8];

			digits [0] = 1;
			for (int i = 1; i < digits.Length; i++) {
				digits [i] = 2 * digits [i - 1];
			}

			rule = new int [256, 2];

			for (int state = 0; state < rule.GetLength (0); state++) {

				bool[] exclusions = new bool [8];
				for (int i = 0; i < exclusions.Length; i++) {
					exclusions [i] = (0 < (state & digits [i]));
				}
				int numberParticles = 0;
				int numberCollisions = 0;
				for (int i = 0; i < 3; i++) {
					if (exclusions [i] && exclusions [i + 3]) {
						numberCollisions++;
						numberParticles += 2;
					} else if (exclusions [i] || exclusions [i + 3]) {
						numberParticles++;
					}
				}
				if (0 == numberCollisions) {
					if (3 == numberParticles) {
						rule [state, 0] = digits [0] | digits [2] | digits [4];
						rule [state, 1] = digits [1] | digits [3] | digits [5];
					} else {
						rule [state, 0] = state;
						rule [state, 1] = state;
					}
				} else if (1 == numberCollisions) {
					if (2 == numberParticles) {
						for (int i = 0; i < 3; i++) {
							if (exclusions [i]) {
								rule [state, 0] = digits [i + 1] | digits [(i + 4) % 6];
								rule [state, 1] = digits [(i + 5) % 6] | digits [i + 2];
								break;
							}
						}
					} else if (3 == numberParticles) {
						for (int i = 0; i < 6; i++) {
							if (exclusions [i] && (!exclusions [(i + 3) % 6])) {
								if (!exclusions [(i + 1) % 6]) {
									rule [state, 0] = digits [i] | digits [(i + 3) % 6] | digits [(i + 4) % 6];
									rule [state, 1] = digits [i] | digits [(i + 1) % 6] | digits [(i + 4) % 6];
								} else {
									rule [state, 0] = digits [i] | digits [(i + 2) % 6] | digits [(i + 5) % 6];
									rule [state, 1] = digits [i] | digits [(i + 2) % 6] | digits [(i + 3) % 6];
								}
								break;
							}
						}
					} else {
						rule [state, 0] = state;
						rule [state, 1] = state;
					}
				} else if (2 == numberCollisions) {
					if (4 == numberParticles) {
						for (int i = 0; i < 3; i++) {
							if (exclusions [i] && exclusions [i + 1]) {
								rule [state, 0] = digits [i + 1] | digits [i + 2] | digits [(i + 4) % 6] | digits [(i + 5) % 6];
								rule [state, 1] = digits [(i + 5) % 6] | digits [i] | digits [i + 2] | digits [i + 3];
								break;
							}
						}
					} else {
						rule [state, 0] = state;
						rule [state, 1] = state;
					}
				} else {
					rule [state, 0] = state;
					rule [state, 1] = state;
				}  // end if numberCollisions

			}  // end for states

		}  // end Init

		public static int Implement (Cell cell)
		{
			return Interact (Receive (cell));
		}

		private static int Receive (Cell cell)
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

		private static int Interact (int state)
		{
			return rule [state, random.Next (0, 2)];
		}

		private static int[] digits;
		private static int[,] rule;
		private static Random random;
	
	}

}

