using System;

namespace CellularAutomata
{
	class MainClass
	{
		public static void Main ()
		{
			Console.WriteLine ("Welcome to CellularAutomata!");
			Console.WriteLine ("This program allows you to use any of the 256 rules.\n");

			Console.WriteLine ("Please type the width and height for this session and press enter.");

			Console.Write ("Width: ");
			int width = Convert.ToInt32 (Console.ReadLine ());

			Console.Write ("Height: ");
			int height = Convert.ToInt32 (Console.ReadLine ());

			Population pop = new Population (width, 0);

			do {
				Console.Write ("Rule: ");
				string cmd = Console.ReadLine ();
				string[] cmds = cmd.Split (new char[1] {' '});

				byte rule;

				if ("new" == cmds[0].ToLower ()) {
					try {
						rule = Convert.ToByte (cmds[1]);
						pop = new Population (width, rule);
					} catch (System.NullReferenceException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					} catch (System.OverflowException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					}
				} else {
					try {
						rule = Convert.ToByte (cmds[0]);
						pop = new Population (width, rule, pop.GetRoot ());
					} catch (System.OverflowException) {
						Console.WriteLine ("Sorry, not valid input.  Please try again.");
						continue;
					} catch (System.FormatException) {
						rule = pop.Rule;
						Console.Write ("{0}", rule);
					}
				}  

				Console.Write ("\n");
				
				for (int i = 0; i < height; i++) {
					pop.PrintCells ();
					pop.Evolve (1);
				}
				if (0 == rule) {
					break;
				}
			} while (true);
		}
	}

	class CState
	{
		private byte state;
		public byte State {
			get { return this.state; }
			set { this.state = value; }
		}
	}
	class CRule
	{
		private byte rule;
		public byte Rule {
			get { return this.rule; }
			set { this.rule = value; }
		}
		public byte Implement (byte neighbourhood)
		{
			byte upper = Convert.ToByte (this.Rule % Math.Pow(2, neighbourhood + 1));
			byte lower;
			if (0 == neighbourhood) {
				lower = Convert.ToByte (0);
			} else {
				lower = Convert.ToByte (this.Rule % Math.Pow (2, neighbourhood));
			}
			byte final = Convert.ToByte (upper - lower);
			if (0 == final) {
				return 0;
			} else {
				return 1;
			}
		}
	}
	class Cell : CState
	{
		private Cell left;
		public Cell Left {
			get { return this.left; }
			set { this.SetLeft (ref value); }
		}
		private Cell right;
		public Cell Right {
			get { return this.right; }
			set { this.SetRight (ref value); }
		}
		public Cell () { }
		public Cell (Cell right)
		{
			this.Right = right;
		}
		public Cell (Cell right, Cell left)
		{
			this.Right = right;
			this.Left = left;
		}
		private void SetLeft (ref Cell cell)
		{
			this.left = cell;
			cell.right = this;
		}
		private void SetRight (ref Cell cell)
		{
			this.right = cell;
			cell.left = this;
		}
		public byte GetNeighbourhood ()
		{
			byte left = this.Left.State;
			byte right = this.Right.State;
			byte center = this.State;

			byte neighbourhood = Convert.ToByte (left * 4);
			neighbourhood += Convert.ToByte (center * 2);
			neighbourhood += Convert.ToByte (right);

			return neighbourhood;
		}
	}
	class Population : CRule
	{
		private Cell root;
		private int length;

		public Population (int length, byte rule)
		{
			this.root = new Cell ();
			this.root.State = 1;
			this.Rule = rule;
			this.length = length;

			Cell current = this.root;
			current.Right = current;

			for (int i = 1; i <= length; i++) {
				current.Right = new Cell (current.Right, current);
				current.Right.Right.Left = current.Right;
				current = current.Right;
			}
		}
		public Population (int length, byte rule, Cell root)
		{
			this.length = length;
			this.Rule = rule;
			this.root = root;
		}
		public Cell GetRoot ()
		{
			return this.root;
		}
		public void Evolve (int generations)
		{
			generations--;

			Cell current = this.root;
			Cell newCurrent = new Cell ();
			newCurrent.Right = newCurrent;
			newCurrent.State = this.Implement (current.GetNeighbourhood ());

			for (int i = 1; i <= length; i++) {
				newCurrent.Right = new Cell (newCurrent.Right, newCurrent);
				newCurrent.Right.Right.Left = newCurrent.Right;
				newCurrent.Right.State = this.Implement (current.Right.GetNeighbourhood ());
				current = current.Right;
				newCurrent = newCurrent.Right;
			}

			this.root = newCurrent.Right;

			if (0 != generations) {
				this.Evolve (generations);
			}
		}

		public void PrintCells ()
		{
			Cell current = root;
			for (int i = 0; i <= this.length; i++) {
				if (0 == current.State) {
					Console.Write (" ");
				} else {
					Console.BackgroundColor = ConsoleColor.Black;
					Console.Write (" ");
					Console.ResetColor ();
				}
				current = current.Right;
			}
			Console.Write ("\n");
		}
	}
}