using System;
using System.Threading;
using System.Threading.Tasks;
using CellularAutomata.Populations; // reference IPopulation, States
using CellularAutomata.Populations.Cells;  // reference ICell, General, VonNeumann, Arangements, Variety
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

		// states structure
		private States states;

		// private constructor for Clone () method, passes all instance variables
		private Simple (Variety variety, ICell[] items, IRule rule, States states)
		{

			this.variety = variety;
			this.items = items;
			this.rule = rule;
			this.states = states;

		}

		// public constructor for simple IC, initailization dependancy SetRule (IRule rule)
		public Simple (Variety variety, int[] sizes)
		{

			this.variety = variety;
			int[] tempValues = new int [0] { };

			switch (this.variety) {

			case Variety.General:  // General Cells

				this.items = General.Build (sizes, out tempValues);  // out init this.states

				break;

			case Variety.VonNeumann:  // Von Neumann Cells

				this.items = VonNeumann.Build (sizes, out tempValues);    // out init this.states

				break;

			}

			this.states = new States (this.items [0].GetArangement (), tempValues, sizes);
			
		}

		// public constructor for custom states
		// states can be shorter than, but not longer than length
		// initailization dependancy SetRule (IRule rule)
		public Simple (Variety variety, int[] sizes, int[] values)
		{

			this.variety = variety;

			switch (this.variety) {

				case Variety.General:  // General Cells

				this.items = General.Build (sizes, values);  // out init this.states

				break;

				case Variety.VonNeumann:  // Von Neumann Cells

				this.items = VonNeumann.Build (sizes, values);    // out init this.states

				break;

			}

			this.states = new States (this.items [0].GetArangement (), values, sizes);

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

		public Variety GetVariety ()  // inherit IPopulation
		{

			return this.variety;

		}

		public new string ToString ()  // return string with type, Variety, and IRule.  For collection naming
		{

			string collection = String.Empty;

			collection += "Simple_";
			collection += this.variety.ToString ();
			collection += "_";
			collection += this.rule.ToString ();

			return collection;

		}

		public object Clone ()  // inherited from IPopulation inhertied from ICloneable
		{
		
			return (new Simple (this.variety, this.items, this.rule, this.states));

		}
		
	}
	
}
