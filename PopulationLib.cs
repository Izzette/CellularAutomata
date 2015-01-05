using System;
using CellularAutomata.Cells;
using CellularAutomata.Rules;

namespace CellularAutomata.Populations
{

	public class Population
	{

		public static ICell Evolve (ICell current, Rule rule, int[] size, ref string states)
		{
			Population population = new Population (rule, current, size, states);
			ICell cell = population.GetRoot ();
			int length = population.GetLength ();

			states = "";

			for (int i = 0; i < length; i++) {

				foreach (int n in size) {

					if (0 == i % n) {

						states += "\n";

					}

				}

				int state = Implement.Absolute (cell.GetNeighbourhood (rule.place), rule);

				current.SetState (state);
				states += Convert.ToString (state);

				current = current.GetNext ();
				cell = cell.GetNext ();
				
			}

			return current;
		}

		public static Population BuildECA (Rule rule, int length)
		{
			
			ElementaryCell root = new ElementaryCell (1);
			ElementaryCell current = root;

			string states = "\n1";

			for (int i = 0; i < length - 1; i++) {
				
				ElementaryCell cell = new ElementaryCell (0);
				current = current.AddNeighbour (ref cell);

				states += "0";
				
			}

			states += "\n";
			
			current = current.AddNeighbour (ref root);
			
			return new Population (rule, current, new int[1] {length}, states);
			
		}

		public static Population BuildVNA (Rule rule, int[] size)
		{

			VNCell root = new VNCell (1);
			VNCell current = root;

			string states = "\n\n1";

			for (int i = 0; i < size[1]; i++) {

				for (int ie = 0; ie < size[0] - 1; ie++) {

					VNCell cell = new VNCell (0);
					current = current.AddNeighbour (ref cell, false);

					states += "0";

				}

				if (size [1] - 1 != i) {

					VNCell edgeCell = new VNCell (0);
					current = current.AddNeighbour (ref edgeCell, true);

					states += "\n0";

				}

			}

			states += "\n\n";

			return new Population (rule, root, size, states);

		}

		private Rule rule;
		
		public Rule Rule {
			
			get { return this.rule; }
			set { this.rule = value; }
			
		}
		
		private ICell root;
		private string states;
		private int[] size;
		private int length;

		public Population (Rule rule, ICell root, int[] size, string states)
		{
			
			this.rule = rule;
			this.root = root;
			this.states = states;

			this.size = size;
			this.length = 1;
			
			foreach (int n in size) {
				
				this.length = this.length * n;
				
			}
			
		}

		public ICell GetRoot ()
		{
			
			return this.root;
			
		}

		public int[] GetSize ()
		{
			
			return this.size;
			
		}
		
		public int GetLength ()
		{
			
			return this.length;
			
		}

		public string GetStates ()
		{

			return this.states;

		}

		public string Evolve ()
		{
			
			return this.Evolve (this.rule);
			
		}

		public string Evolve (int generations)
		{
			
			return this.Evolve (this.rule, generations);
			
		}

		public string Evolve (Rule rule)
		{
			
			this.root = Population.Evolve (this.root, rule, this.size, ref this.states);

			return this.states;
			
		}

		public string Evolve (Rule rule, int generations)
		{

			string states = "\n";

			for (int i = 0; i < generations; i++) {

				states += "\\g" + this.Evolve (rule);

			}

			states += "\n";

			return states;
			
		}
		
	}
	
}
