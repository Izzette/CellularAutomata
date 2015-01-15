using System;
using CellularAutomata.Populations;
using CellularAutomata.Commands;
using CellularAutomata.Outputs;

namespace CellularAutomata.Commands  // console UI interface
{

	// Command: Population, pop 
	public static class PopulationsControl  // interfaces console access to Population
	{

		// excecute
		public static void Excecute (string method, Option[] options, string[] arguments) {

			switch (method) {
			case "new":
				NewPopulation (options, arguments);
				break;
			case "evolve":
				Evolve (options, arguments);
				break;
			default:
				CommandsWarning.MethodNotFound (Command, method);
				return;
			}  // end switch (method) statment

		}  // end Excecute, public static void

		// IPopulations
		private static IPopulation population;

		// default populations
		private static CellsVariety DefaultCellsVariety {
			get { return CellsVariety.General; }
			set { ; }
		}

		// default initial conditions
		static private int[] DefaultValues {
			get { return new int [1] { 1 }; }
			set { ; }
		}

		static private IRule DefaultRule {  // returns 2k110
			get {
				IRule rule = new Absolute ();
				rule.Parse ("(2,110)");
				return rule;
			}
			set { ; }
		}

		private static OutputsFormat DefaultFormat {
			get { return OutputsFormat.Bitmap; }
			set { ; }
		}

		private static string Command {
			get { return "Population"; }
			set { ; }
		}

		private static int[] GetSizes (CellsVariety cellsVariety, int[] sizes)
		{
			switch (cellsVariety) {
			case CellsVariety.General:
				if (1 != sizes.Length) {
					return new int [1] { 125 };
				}
				break;
			case CellsVariety.VonNeumann:
				if (2 != sizes.Length) {
					return new int [2] { 30, 30 };
				}
				break;
			case CellsVariety.Moore:
				if (2 != sizes.Length) {
					return new int [2] { 30, 30 };
				}
				break;
			default:
				throw new ArgumentException ("At PopulationControl.GetSizes: CellsVariety variety not recognized");
			}  // end switch (cellsVariety) statment
			return sizes;
		}  // end GetSizes, private static int[] method


		// format: Population new [-<o>[:<arg>]] <population> <seed>
		private static void NewPopulation (Option[] options, string[] arguments)  // Creates reinitializes population
		{

			string method = "new";

			int[] sizes = new int [1] { 125 };
			int[] values = DefaultValues;
			IRule rule = DefaultRule;
			CellsVariety cellsVariety = DefaultCellsVariety;

			foreach (Option option in options) {
				switch (option.Name) {
				// set sizes
				// -s:(<x>[,<n>])
				case "s":
					// extract sizes
					string[] sizesStrings;
					try {
						sizesStrings = option.Arguments [0].Split (new char [3] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					}
					sizes = new int [sizesStrings.Length];
					try {
						for (int i = 0; i < sizes.Length; i++) {
							sizes [i] = Convert.ToInt32 (sizesStrings [i]);
						}
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					}
					break;
				// set rule
				// -r:<type>:<code>
				case "r":
					// select rule type
					if (2 > option.Arguments.Length) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					}
					switch (option.Arguments [0]) {
					// Absolute
					case "a":
						rule = new Absolute ();
						break;
					// Totalistic
					case "t":
						rule = new Totalistic ();
						break;
					case "bt":
						rule = new BorderTotalistic ();
						break;
					case "ll":
						rule = new LifeLike ();
						break;
					default:
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					} // end switch (option.Arguments [0]) statement
					try {
						rule.Parse (option.Arguments [1]);
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [1]);
						return;
					}  // end try catch (FormatException)
					// parse code
					break;
				// set cellsVariety
				// -v:<cellsVariety>
				case "v":
					if (0 == option.Arguments.Length) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					}
					// select cellsVariety
					switch (option.Arguments [0]) {
					// general
					case "g":
						cellsVariety = CellsVariety.General;
						break;
					case "vn":
						cellsVariety = CellsVariety.VonNeumann;
						break;
					case "m":
						cellsVariety = CellsVariety.Moore;
						break;
					case "ng":
						cellsVariety = CellsVariety.NextGeneral;
						break;
					default:
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					} // end switch (option.Argumetns [0]) statment
					break;
				default:
					CommandsWarning.OptionNotFound (Command, method, option.Name);
					return;
				}  // end switch (option.Name) statement
			}  // end foreach (Option option in options) loop

			if (2 > arguments.Length) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}
			// extract values
			char[] valuesStrings = arguments [1].ToCharArray ();
			values = new int [valuesStrings.Length];
			try {
				for (int i = 0; i < values.Length; i++) {
					values [i] = Convert.ToInt32 (valuesStrings [i].ToString ());
				}
			} catch (FormatException) {
				CommandsWarning.ArgumentNotValid (Command, method, arguments [1]);
				return;
			}

			// select population
			switch (arguments [0]) {
			// main population
			case "p":
				population = new Simple (cellsVariety, sizes, values);
				population.SetRule (rule);
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
				return;
			}  // end switch (arguments [0]) statment
			  // end try statment

		}  // end NewPopulation, private static void method

		// method: evolve
		// evolves population
		private static void Evolve (Option[] options, string[] arguments)
		{

			string method = "Evolve";

			int generation = 1;
			OutputsFormat format = DefaultFormat;

			foreach (Option option in options) {
				switch (option.Name) {
				case "g":
					if (1 > option.Arguments.Length) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					}
					try {
						generation = Convert.ToInt32 (option.Arguments [0]);
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					}
					break;
				default:
					CommandsWarning.OptionNotFound (Command, method, option.Name);
					return;
				}  // end switch (option.Name) statement
			}  // end foreach (Option option in options) loop

			if (2 > arguments.Length) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}
			// select format
			switch (arguments [1]) {
			case "bitmap":
				format = OutputsFormat.Bitmap;
				break;
			case "gif":
				format = OutputsFormat.Gif;
				break;
			case "quiet":
				format = OutputsFormat.Quiet;
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [1]);
				return;
			}  // end switch (arguments [1]) statment

			switch (arguments [0]) {
			case "p":
				OutputsControl.Init (population, generation, format);
				for (int i = 1; i <= generation; i++) {
					population.Evolve ();
					OutputsControl.Update (population, i, format);
				}
				OutputsControl.Final (population, generation.ToString (), format);
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
				return;
			}

		}  // end Evolve, private static void method

	}  // end PopulationsControl, public static class

}  // end CellularAutomata.Commands, namespace
