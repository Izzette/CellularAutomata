using System;
using System.Collections;
using CellularAutomata.Cells;

namespace CellularAutomata.Populations
{
	
	public struct Rule
	{
		
		public ulong number;
		public ulong place;
		
		public Rule (ulong number, ulong place)
		{
			
			this.number = number;
			this.place = place;
			
		}
		
	}

	public class Population : IEnumerator,IEnumerable
	{
		
		public static ulong Implement (ulong neighbourhood, Rule rule)
		{
			
			double upper = rule.number % Math.Pow (rule.place, neighbourhood + 1);

			if (0 == neighbourhood) {
				
				return Convert.ToUInt64 (upper);
				
			} else {
				
				double lower = rule.number % Math.Pow (rule.place, neighbourhood);
				double final = (upper - lower) / Math.Pow (rule.place, neighbourhood);
				
				return Convert.ToUInt64 (final);
			}
		}

		public static ICell Evolve (ICell current, Rule rule, params ulong[] size)
		{
			Population population = new Population (rule, current, size);

			foreach (ICell cell in population) {
				current.SetState (Population.Implement (cell.GetNeighbourhood (rule.place), rule));
				current = current.GetNext ();
			}

			return current;
		}

		public static Population BuildECA (ulong length)
		{
			return BuildECA (new Rule (0, 2), length);
		}

		public static Population BuildECA (Rule rule, ulong length)
		{
			ElementaryCell root = new ElementaryCell (1);
			ElementaryCell current = root;

			for (ulong i = 0; i < length - 1; i++) {
				ElementaryCell cell = new ElementaryCell (0);
				current = current.AddNeighbour (ref cell);
			}
			
			current = current.AddNeighbour (ref root);
			
			return new Population (rule, current, length);
		}

		private Rule rule;
		private ICell root;
		private ulong[] size;

		// For use with IEnumerator
		private ulong length;
		private ICell current;
		private ulong position;

		public Population (Rule rule, ICell root, params ulong[] size)
		{
			this.rule = rule;
			this.root = root;
			this.current = this.root;
			this.size = size;
			this.length = 1;
			this.position = 0;
			foreach (ulong n in size) {
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

		public ulong[] GetSize ()
		{
			return this.size;
		}

		public void Evolve ()
		{
			this.root = Population.Evolve (this.root, this.rule, this.size);
		}

		public void Evolve (ulong generations)
		{
			this.root = Population.Evolve (this.root, this.rule, this.size);
			generations--;
			if (0 < generations) {
				this.Evolve (generations);
			}
		}

		public void Evolve (Rule rule)
		{
			this.root = Population.Evolve (this.root, rule, this.size);
		}

		public void Evolve (Rule rule, ulong generations)
		{
			this.root = Population.Evolve (this.root, rule, this.size);
			generations--;
			if (0 < generations) {
				this.Evolve (rule, generations);
			}
		}

		// Required by IEnumerator
		public object Current
		{
			get { return this.current;}
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
