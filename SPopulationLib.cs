using System;
using System.Threading;
using System.Threading.Tasks;  // for Parallel class
using CellularAutomata.Rules;  // reference IRuleLib
using CellularAutomata.Populations; // reference VPopulationLib, IPopulationLib
using CellularAutomata.Populations.Cells;  // reference ICellLib, GCellLib, VNCellLib

namespace CellularAutomata.Populations  // Contains cell collections and Cells namespace
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

		public Simple (Variety variety, int[] size, int[] states)  // public constructor for custom states, initailization dependancy SetRule (IRule rule)
		{

			this.variety = variety;
			this.size = size;
			this.states = states;

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
