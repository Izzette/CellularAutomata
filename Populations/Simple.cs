using System;
using System.Threading;
using System.Threading.Tasks;
using CellularAutomata.Populations; // reference Variety, IPopulation
using CellularAutomata.Populations.Cells;  // reference ICell, General, VonNeumann
using CellularAutomata.Populations.Rules;  // reference IRule

namespace CellularAutomata.Populations  // Contains cell collections, Cells namespace, Rules namespace
{

	public class Simple : IPopulation  // inherits IPopulation
	{

		private int Length {  // no set
			get { return items.Length; }
			set { ; }
		}

		private IRule rule;  // get rule
		private Variety variety;  // kind of network, get variety
		private ICell[] items;  // not accessable

		// no property needed because IPopulation inheritance, GetStates (out int[] states, out int[] size)
		private int[] size;
		private int[] states;

		private Simple (Variety variety, ICell[] items, IRule rule, int[] size, int[] states)  // private constructor for Clone () method, passes all instance variables
		{

			this.variety = variety;
			this.items = items;
			this.rule = rule;
			this.size = size;
			this.states = states;

		}

		public Simple (Variety variety, int[] size)  // public constructor for simple IC, initailization dependancy SetRule (IRule rule)
		{

			this.variety = variety;
			this.size = size;

			switch (variety) {

			case Variety.General:  // General Cells

				this.items = General.Build (size, out this.states);  // out init this.states

				break;

			case Variety.VonNeumann:  // Von Neumann Cells

				this.items = VonNeumann.Build (size, out this.states);    // out init this.states

				break;

			}
			
		}

		// public constructor for custom states
		// states can be shorter than, but not longer than length
		// initailization dependancy SetRule (IRule rule)
		public Simple (Variety variety, int[] size, int[] states)
		{

			this.variety = variety;
			this.size = size;

			int length = 1;

			foreach (int n in size) {

				length = length * n;

			}

			this.states = new int [length];

			int index = 0;

			for ( ; index < states.Length; index++) {

				this.states [index] = states [index];

			}

			for ( ; index < this.states.Length; index++) {

				this.states [index] = 0;

			}

			switch (variety) {

			case Variety.General:  // General Cells

				this.items = General.Build (size, this.states);

				break;

			case Variety.VonNeumann:  // Von Neumann Cells

				this.items = VonNeumann.Build (size, this.states);

				break;

			}

		}

		public void Evolve ()  // inherited from IPopulation, evolves once, parallel ineffective on Mono
		{
			
			Parallel.For(0, this.Length, i => {

				this.states[i] = this.rule.Implement (this.items[i]);
            
            });
            
			Parallel.For (0, this.Length, i => {
								
				this.items [i].SetState (this.states [i]);  // set items -> states
				
			});
			
		}

		public int[] GetStates (out int[] size)  // inherited from IPopulation
		{

			size = this.size;  // out

			return this.states;

		}

		public void SetRule (IRule rule)  // inherited from IPopulation
		{

			this.rule = rule;

		}

		public IRule GetRule ()  // inherited from IPopulation
		{

			return this.rule;

		}

		public Variety GetVariety ()  // inherit IPopulation
		{

			return this.variety;

		}

		public object Clone ()  // inherited from IPopulation inhertied from ICloneable
		{
		
			return (new Simple (this.variety, this.items, this.rule, this.size, this.states));

		}
		
	}
	
}
