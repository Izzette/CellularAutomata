using System;
using System.Threading;
using System.Threading.Tasks;
using CellularAutomata.Populations;

namespace CellularAutomata.Populations  // Contains cell collections, Cells namespace, Rules namespace
{

	public class Simple : IPopulation  // inherits IPopulation
	{

		private int Length {  // no set
			get { return items.Length; }
			set { ; }
		}

		private IRule rule;  // get rule
		private CellsVariety cellsVariety;  // kind of network, get cellsVariety
		private ICell[] items;  // not accessable

		// states structure
		private States states;

		// private constructor for Clone () method, passes all instance variables
		private Simple (CellsVariety cellsVariety, ICell[] items, IRule rule, States states)
		{

			this.cellsVariety = cellsVariety;
			this.items = items;
			this.rule = rule;
			this.states = states;

		}

		// public constructor for custom states
		// states can be shorter than, but not longer than length
		// initailization dependancy SetRule (IRule rule)
		public Simple (CellsVariety cellsVariety, int[] sizes, int[] values)
		{

			this.cellsVariety = cellsVariety;

			switch (this.cellsVariety) {

			case CellsVariety.General:  // General Cells

				this.states = new States (General.Arangement, values, sizes);
				this.items = General.Build (this.states.Sizes, this.states.Values);  // out init this.states

				break;

			case CellsVariety.VonNeumann:  // Von Neumann Cells

				this.states = new States (VonNeumann.Arangement, values, sizes);
				this.items = VonNeumann.Build (this.states.Sizes, this.states.Values);// out init this.states

				break;

			case CellsVariety.Moore:

				this.states = new States (Moore.Arangement, values, sizes);
				this.items = Moore.Build (this.states.Sizes, this.states.Sizes);

				break;

			default:

				throw new ArgumentException ();

			}

		}

		public void Evolve ()  // inherited from IPopulation, evolves once, parallel ineffective on Mono
		{
			
			Parallel.For(0, this.Length, i => {

				this.states.Values [i] = this.rule.Implement (this.items [i]);
            
            });
            
			Parallel.For (0, this.Length, i => {
								
				this.items [i].SetState (this.states.Values [i]);  // set items -> states
				
			});
			
		}
		// inherited from IPopulation
		public States GetStates ()
		{

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

		public CellsVariety GetCellsVariety ()  // inherit IPopulation
		{

			return this.cellsVariety;

		}

		public CellsArangement GetCellsArangement ()
		{

			return items [0].GetArangement ();

		}

		public new string ToString ()  // return string with type, CellsVariety, and IRule.  For collection naming
		{

			string collection = String.Empty;

			collection += "Simple_";
			collection += this.cellsVariety.ToString ();
			collection += "_";
			collection += this.rule.ToString ();

			return collection;

		}

		public IPopulation Clone ()  // inherited from IPopulation inhertied from ICloneable
		{
		
			return (new Simple (this.cellsVariety, this.items, this.rule, this.states));

		}
		
	}
	
}
