using System;
using CellularAutomata.Populations;  // reference IPopulation, Simple
using CellularAutomata.Populations.Rules;  // reference IRule, Absolute
using CellularAutomata.Populations.Cells; // reference Variety
using CellularAutomata.Commands;  // reference Option, OutputFormats, OutputsControl

namespace CellularAutomata.Commands  // console UI interface
{

	// Command: Population, pop 
	public static class PopulationsControl  // interfaces console access to Population
	{

		// IPopulations
		static private IPopulation population;
		static private IPopulation clone;  // Clone

		static private Variety DefaultVariety {  // default general
			get { return Variety.General; }
			set { ; }
		}

		static private int[] DefaultSize {  // default depends on variety
			get {
				switch (variety) {
				case Variety.General:
					return (new int [1] { 125 });
				case Variety.VonNeumann:
					return (new int [2] { 100, 30 });
				default:  // will throw errors if a new Variety member is added
					throw new NotImplementedException ("Population.Variety match not found!");
				}
			}
			set { ; }
		}

		static private IRule DefaultRule {  // returns 2k110
			get {
				IRule rule = new Absolute ();
				rule.Parse ("[2,110]");
				return rule;
			}
			set { ; }
		}
		 
		//  Starts a defaults
		static private Variety variety = DefaultVariety;
		static private IRule rule = DefaultRule;
		static private int[] size = DefaultSize;

		// method: new
		// format: [-options[:arguments]] (IPopulation) [(inital condition)]
		public static void New (Option[] options, string[] arguments)  // Creates reinitializes population
		{

			foreach (Option o in options) {

				switch (o.name) {

				// format: -s:(x)[:(y) (...) ]
				case "size":
				case "s":

					size = new int [o.arguments.Length];

					for (int i = 0; i < o.arguments.Length; i++) {

						size [i] = Convert.ToInt32 (o.arguments [i]);

					}

					// correct any variety incompatability
					if ((1 == size.Length) && (Variety.General != variety)) {

						variety = Variety.General;

					} else if ((2 == size.Length) && (Variety.VonNeumann != variety)) {

						variety = Variety.VonNeumann;

					}

					break;

				// format: -r:[(k),(n)]
				// literal brackets above
				case "rule":
				case "r":

					rule = new Absolute ();
					rule.Parse (o.arguments [0]);  // the rule handles the option argument interpretation

					break;

				// foramt: -v:(variety)
				case "variety":
				case "v":

					switch (o.arguments [0]) {

					case "general":
					case "g":

						variety = Variety.General;

						// correct any size incompatabilities
						if (1 != size.Length) {

							size = DefaultSize;

						}

						break;

					case "vonNeumann":
					case "v":

						variety = Variety.VonNeumann;

						// correct any size incompatabilities
						if (2 != size.Length) {

							size = DefaultSize;

						}

						break;

					}

					break;

				}

			}

			IPopulation tempPopulation;

			if (1 < arguments.Length) {  // if specified initial conditions

				int[] values = new int [arguments [1].Length];

				// get each character
				string[] valuestrings  = arguments [1].Split (new string [1] { string.Empty }, StringSplitOptions.RemoveEmptyEntries);

				for (int i = 0; i < valuestrings.Length; i++) {  // add manual values

					values [i] = Convert.ToInt32 (valuestrings [i]);

				}

				tempPopulation = new Simple (variety, size, values);

			} else {

				// set with simple intital contdition
				tempPopulation = new Simple (variety, size);

			}  // end if (1 < arguments.Length)

			// setter injection
			tempPopulation.SetRule (rule);

			// format: (target) [(intial conditions)]
			switch (arguments [0]) {
			case "population":
			case "p":
				population = tempPopulation;
				break;
			case "clone":
			case "c":
				clone = tempPopulation;
				break;
			}

		}  // end New, public static void method

		private static OutputFormats DefaultFormat {
			get { return OutputFormats.Bitmap; }
			set { ; }
		}

		// method: evolve 
		// format: [-options[:arguments]] (IPopulation) (output)
		public static void Evolve (Option[] options, string[] arguments)  // Creates reinitializes population
		{

			// default number of iterations
			int generation = 1;

			//  switch for each option
			foreach (Option option in options) {

				switch (option.name) {
				// multiple generations
				case "generations":
				case "g":
					generation = Convert.ToInt32 (option.arguments [0]);
					break;
				}  // end switch (options.name) statement

			}  // end foreach (Option option in options) loop

			// set default format
			OutputFormats format = DefaultFormat;

			try {
				// second argument is output format
				switch (arguments [1]) {
				case "quiet":
				case "q":
					format = OutputFormats.Quiet;
					break;
				case "bitmap":
				case "b":
					format = OutputFormats.Bitmap;
					break;
				}  // end switch (arguments [1]) statement
			} catch (IndexOutOfRangeException) {
				;
			}

			// format: (target) [(intial conditions)]
			switch (arguments [0]) {
				case "population":
				case "p":
				OutputsControl.Out (population.ToString (), 0, population.GetStates (), format);
				for (int i = 1; i <= 100; i++) {
					population.Evolve ();
					OutputsControl.Out (population.ToString (), i, population.GetStates (), format);
				}
				break;
				case "clone":
				case "c":
				OutputsControl.Out (clone.ToString (), 0, clone.GetStates (), format);
				for (int i = 1; i <= 100; i++) {
					clone.Evolve ();
					OutputsControl.Out (clone.ToString (), i, clone.GetStates (), format);
				}
				break;
			}

		}  // end Evolve, public static void method

	}  // end PopulationsControl, public static class

}  // end CellularAutomata.Commands, namespace
