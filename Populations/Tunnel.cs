using System;
using System.Threading.Tasks;
using CellularAutomata.Populations;
using System.Numerics;

namespace CellularAutomata.Populations
{
	public class Tunnel : IPopulation
	{

		public Tunnel (int[] sizes)
		{
			int[] values = new int [sizes [0] * sizes [1]];
			Random random = new Random ();
			for (int i = 0; i < values.Length; i++) {
				if (
					((sizes [0] * 2) / 5 < i % sizes [0])
				    &&
					((sizes [0] * 3) / 5 > i % sizes [0])
					&&
					((sizes [1] * 2) / 5 < i / sizes [0])
					&&
					((sizes [1] * 3) / 5 > i / sizes [0])
					) {
					values [i] = 0;
				} else {
					values [i] = random.Next (0, 64);
				}
			}
			this.states = new States (CellsArangement.Tunnel, values, sizes);
			this.items = Hexagonal.Build (this.states.Sizes, this.states.Values);
		}

		public void Evolve ()
		{
			Random random = new Random ();
			for (int g = 0; g < 2; g++) {
				Parallel.For (0, this.Length, i => {
//					if (0 == i % this.states.Sizes [0]) {
//						this.items [i].SetState (random.Next (0, 64));
//					} else if (0 == (i + 1) % this.states.Sizes [0]) {
//						this.items [i].SetState (0);
//					}
					this.states.Values [i] = this.Implement (this.items [i], ref random);
				});
				Parallel.For (0, this.Length, i => {
					this.items [i].SetState (this.states.Values [i]);  // set items -> states
				});
			}
		}
		public States GetStates ()
		{
			return this.states;
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
			return new Tunnel (this.states.Sizes);
		}

		private States states;
		private Hexagonal[] items;

		private int Length {  // no set
			get { return items.Length; }
			set { ; }
		}

		private int Implement (Hexagonal cell, ref Random random)
		{
			Hexagonal[] neighbours = cell.GetNeighbours ();
			int numberNeighbours = 0;
			int collide = 0;
			int[] newStates = new int [6];
			for (int i = 0; i < 6; i++) {
				int placeValue = (int)Math.Pow (2, ((i + 3) % 6));
				int isNeighbour = (int)(neighbours [i].GetState () % (placeValue * 2)) / placeValue;
				if (0 < isNeighbour) {
					if (0 < newStates [i]) {
						collide++;
					}
					numberNeighbours++;
					newStates [(i + 3) % 6] = 1;
				} else {
					newStates [(i + 3) % 6] = 0;
				}
			}
			if (0 < collide) {
				if (0 == numberNeighbours - (collide * 2)) {
					int side = random.Next (0, 2);
					for (int i = 0; i < 3; i++) {
						if (0 < side) {
							newStates = new int [6] {
								newStates [1],
								newStates [2],
								newStates [3],
								newStates [4],
								newStates [5],
								newStates [0]
							};
						} else {
							newStates = new int [6] {
								newStates [5],
								newStates [0],
								newStates [1],
								newStates [2],
								newStates [3],
								newStates [4]
							};
						}
					}
				} else if (3 == numberNeighbours) {
					int straight = 0;
					int observe = 0;
					for (int i = 0; i < 6; i++) {
						if (0 < newStates [i]) {
							if (0 < newStates [(i + 3) % 6]) {
								straight = i % 3;
							} else {
								observe = i;
							}
						}
					}
					newStates [straight] = 0;
					newStates [straight + 3] = 0;
					int side = random.Next (0, 2);
					if (0 < side) {
						int arange = ((observe + 3) - straight) % 3;
						switch (arange) {
						case 1:
							newStates [(observe + 3) % 6] = 1;
							newStates [(observe + 4) % 6] = 1;
							break;
						case 2:
							newStates [(observe + 2) % 6] = 1;
							newStates [(observe + 3) % 6] = 1;
							break;
						}
					} else {
						if ((observe % 3) == (straight + 1) % 3) {
							newStates [(straight + 5) % 6] = 1;
							newStates [straight + 2] = 1;
						} else {
							newStates [straight + 1] = 1;
							newStates [(straight + 4) % 6] = 1;
						}
					}
				}
			} else if (3 == numberNeighbours) {
				int side = random.Next (0, 2);
				if (0 < side) {
					newStates = new int [6] {
						newStates [1],
						newStates [2],
						newStates [3],
						newStates [4],
						newStates [5],
						newStates [0]
					};
				}
			}
			int newState = 0;
			for (int i = 0; i < 6; i++) {
				newState += newStates [i] * (int)Math.Pow (2, i);
			}
			return newState;
		}
	
	}

}

