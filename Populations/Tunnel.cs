using System;
using System.Threading.Tasks;
using CellularAutomata.Populations;
using System.Numerics;

namespace CellularAutomata.Populations
{
	public class Tunnel : IPopulation
	{

		public Tunnel (int[] sizes, double wavelength, double numberOfWaves, int updateToDraw, int scale)
		{
			Init ();
			this.numberOfWaves = numberOfWaves;
			this.wavelength = wavelength;
			this.scale = scale;
			this.updateToDraw = updateToDraw;
			int[] values = new int [sizes [0] * sizes [1]];
			Random random = new Random ();
			double[] center = new double [2] { (double)sizes [0] / 2D, (double)sizes [1] / 2D };
			for (int i = 0; i < values.Length; i++) {
				if (0 == random.Next (0, 16)) {
					double distance = Math.Sqrt (Math.Pow ((double)(i % sizes [0]) - center [0], 2D) + Math.Pow ((double)(i / sizes [0]) - center [1], 2D));
					if (wavelength * numberOfWaves > distance) {
						if ((double)random.Next (Int32.MinValue, Int32.MaxValue) < ((double)Int32.MaxValue * Math.Sin ((distance / wavelength) * 2D * Math.PI))) {
							values [i] = (int)Math.Pow (2, random.Next (0, 6)) + (int)Math.Pow (2, random.Next (0, 6));
						} else {
							values [i] = 0;
						}
					} else if (0 == random.Next (0, 2)) {
						values [i] = (int)Math.Pow (2, random.Next (0, 6));
					} else {
						values [i] = 0;
					}
				} else {
					values [i] = 0;
				}
			}
			this.states = new States (CellsArangement.Tunnel, values, sizes);
			this.items = Hexagonal.Build (this.states.Sizes, this.states.Values);
		}

		public void Evolve ()
		{
			for (int g = 0; g < updateToDraw; g++) {
				int threads = this.states.Sizes [1] * 2;
				Parallel.For (0, threads, p => {
					int other = (Length / threads);
					int number = p * other;
					for (int i = number; i < number + other; i++) {
						this.states.Values [i] = Implement (this.items [i]);
					}
				});
				Parallel.For (0, threads, p => {
					int other = (Length / threads);
					int number = p * other;
					for (int i = number; i < number + other; i++) {
						this.items [i].SetState (this.states.Values [i]);  // set items -> states
					}
				});
			}
		}
		public States GetStates ()
		{
			int[] densities = new int [this.states.Values.Length / (this.scale * this.scale)];
			Parallel.For (0, densities.Length, i => {
				int d = 0;
				int x = (this.scale * i) % this.states.Sizes [0];
				int state = 0;
				for (int v = 0; v < this.scale; v++) {
					int y = this.scale * this.states.Sizes [0] * ((i * this.scale) / (this.states.Sizes [0]));
					for (int w = 0; w < this.scale; w++) {
						state += this.states.Values [x + y];
						state <<= 6;
						y += this.states.Sizes [0];
					}
					x++;
				}
				while (0 < state) {
					int temp;
					state = Math.DivRem (state, 2, out temp);
					d += temp;
				}
				densities [i] = d;
			});
			return new States (this.states.Arangement, densities, new int [2]  {this.states.Sizes [0] / this.scale, this.states.Sizes [1] / this.scale});
		}
		public void SetRule (IRule rule) {
			throw new ArgumentException ();
		} // sets rule
		public IRule GetRule ()
		{
			throw new ArgumentException ();
		}
		public CellsVariety GetCellsVariety ()
		{
			return CellsVariety.Hexagonal;
		}
		public CellsArangement GetCellsArangement ()
		{
			return CellsArangement.Tunnel;
		}
		public new string ToString ()
		{
			return "Tunnel";
		}
		public IPopulation Clone ()
		{
			return new Tunnel (this.states.Sizes, this.wavelength, this.numberOfWaves, this.updateToDraw, this.scale);
		}

		private States states;
		private Hexagonal[] items;
		private double wavelength;
		private double numberOfWaves;
		private int updateToDraw;
		private int scale;

		private int Length {  // no set
			get { return items.Length; }
			set { ; }
		}

		public static void Init ()
		{

			if (null != rule) {
				return;
			}

			random = new Random ();

			digits = new int [8];

			digits [0] = 1;
			for (int i = 1; i < digits.Length; i++) {
				digits [i] = 2 * digits [i - 1];
			}

			rule = new int [64, 2];

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

		public static int Implement (Hexagonal cell)
		{
			return Interact (Receive (cell));
		}

		private static int Receive (Hexagonal cell)
		{
			Hexagonal[] neighbours = cell.GetNeighbours ();
			return (
				(neighbours [0].GetState () & digits [0])
				| (neighbours [1].GetState () & digits [1])
				| (neighbours [2].GetState () & digits [2])
				| (neighbours [3].GetState () & digits [3])
				| (neighbours [4].GetState () & digits [4])
				| (neighbours [5].GetState () & digits [5])
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

