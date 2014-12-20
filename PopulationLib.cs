using System;
using System.Collections;
using CellularAutomata.Cells;

namespace CellularAutomata.Populations
{
	public struct Rule
	{
		public Rule (int number, int place)
		{
			this.number = number;
			this.place = place;
		}

		public int number, place;
	}

	public class Population : IEnumerator,IEnumerable
	{
		public static int Implement (int neighbourhood, Rule rule)
		{
			int upper = Convert.ToInt32 (rule.number % Math.Pow (rule.place, neighbourhood + 1));

			if (0 == neighbourhood) {
				return upper;
			} else {
				int lower = Convert.ToInt32 (rule.number % Math.Pow (rule.place, neighbourhood));
				int final = Convert.ToInt32 ((upper - lower) / Math.Pow (rule.place, neighbourhood));
				return final;
			}
		}

		public static ICell Evolve (ICell current, Rule rule, params int[] size)
		{
			Population population = new Population (rule, current, size);

			foreach (ICell cell in population) {
				current.SetState (Population.Implement (cell.GetNeighbourhood (), rule));
				current = current.GetNext ();
			}

			return current;
		}

		public static string GetChars (Population population)
		{
			char[][] states = new char[population.size.Length][];
			for (int i = 0; i < population.size.Length; i++) {
				states[i] = new char[population.size[i].Length];
			}
			
			for (int i = 0; i < population.size.Length; i++) {
				for (int ie = 0; i < population.size[i]; i++) {
					states[i][ie] = Convert.ToChar (.GetState ());
				}
			}
			
			return states;
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

		public void Evolve (Rule rule, int generations)
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
