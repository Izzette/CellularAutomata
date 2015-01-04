using System;
using CellularAutomata.Cells;
using CellularAutomata.Rules;

namespace CellularAutomata.Populations
{

	public class Population
	{

		public static ICell Evolve (ICell current, Rule rule, int[] size)
		{
			Population population = new Population (rule, current, size);
			ICell cell = population.GetRoot ();
			int length = population.GetLength ();

			for (int i = 0; i < length; i++) {
				
				current.SetState (Implement.Absolute (cell.GetNeighbourhood (rule.place), rule));
				current = current.GetNext ();
				cell = cell.GetNext ();
				
			}

			return current;
		}

		public static Population BuildECA (Rule rule, int length)
		{
			
			ElementaryCell root = new ElementaryCell (1);
			ElementaryCell current = root;

			for (int i = 0; i < length - 1; i++) {
				
				ElementaryCell cell = new ElementaryCell (0);
				current = current.AddNeighbour (ref cell);
				
			}
			
			current = current.AddNeighbour (ref root);
			
			return new Population (rule, current, length);
			
		}

		public static Population BuildVNA (Rule rule, int[] size)
		{

			VNCell root = new VNCell (1);
			VNCell current = root;

			for (int i = 0; i < size[1]; i++) {

				for (int ie = 0; ie < size[0] - 1; ie++) {

					VNCell cell = new VNCell (0);
					current = current.AddNeighbour (ref cell, false);

				}

				if (size [1] - 1 != i) {

					VNCell edgeCell = new VNCell (0);
					current = current.AddNeighbour (ref edgeCell, true);

				}

			}

			return new Population (rule, root, size);

		}

		private Rule rule;
		
		public Rule Rule {
			
			get { return this.rule; }
			set { this.rule = value; }
			
		}
		
		private ICell root;
		private int[] size;
		private int length;

		public Population (Rule rule, ICell root, params int[] size)
		{
			
			this.rule = rule;
			this.root = root;
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

		public void Evolve ()
		{
			
			this.root = Population.Evolve (this.root, this.rule, this.size);
			
		}

		public void Evolve (int generations)
		{
			
			this.Evolve (this.rule, generations);
			
		}

		public void Evolve (Rule rule)
		{
			
			this.root = Population.Evolve (this.root, rule, this.size);
			
		}

		public void Evolve (Rule rule, int generations)
		{
			
			this.Evolve (rule);
			
			generations--;
			
			if (0 < generations) {
				
				this.Evolve (rule, generations);
				
			}
			
		}
		
	}
	
}
