using System;
using System.Collections;
using System.Numerics;
using CellularAutomata.Cells;

namespace CellularAutomata.Populations
{
	
	public struct Rule
	{
		
		public BigInteger number;
		public BigInteger place;
		
		public Rule (BigInteger number, BigInteger place)
		{
			
			this.number = number;
			this.place = place;
			
		}
		
	}

	public class Population : IEnumerator,IEnumerable
	{
		
		public static int Implement (int neighbourhood, Rule rule)
		{
			
			BigInteger upper = rule.number % BigInteger.Pow (rule.place, neighbourhood + 1);

			BigInteger lower = rule.number % BigInteger.Pow (rule.place, neighbourhood);
			BigInteger final = (upper - lower) / BigInteger.Pow (rule.place, neighbourhood);
				
			return (int)final;
			
		}

		public static ICell Evolve (ICell current, Rule rule, params int[] size)
		{
			Population population = new Population (rule, current, size);

			foreach (ICell cell in population) {
				
				current.SetState (Population.Implement (cell.GetNeighbourhood (rule.place), rule));
				current = current.GetNext ();
				
			}

			return current;
		}

		public static Population BuildECA (int length)
		{
			
			return BuildECA (new Rule (0, 2), length);
			
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
		private ICell root;
		private int[] size;

		// For use with IEnumerator
		private int length;
		private ICell current;
		private int position;

		public Population (Rule rule, ICell root, params int[] size)
		{
			
			this.rule = rule;
			this.root = root;
			this.current = this.root;
			this.size = size;
			this.length = 1;
			this.position = 0;
			
			foreach (int n in size) {
				
				this.length = this.length * n;
				
			}
			
		}

		public ICell GetRoot ()
		{
			
			return this.root;
			
		}

		public Rule GetRule ()
		{
			
			return this.rule;
			
		}

		public int[] GetSize ()
		{
			
			return this.size;
			
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

		// Required by IEnumerator
		public object Current
		{
			
			get { return this.current; }
			
		}

		// Required by IEnumerator
		public bool MoveNext()
		{
			
			this.current = this.current.GetNext ();
			
			this.position++;
			
			return (this.position < this.length);
			
		}

		// Required by IEnumerator
		public void Reset()
		
		{
			this.current = this.root;
			this.position = 0;
			
		}

		// Required by IEnumerable
		public IEnumerator GetEnumerator()
		{
			
			return (IEnumerator)this;
			
		}
		
	}
	
}
