using System;
using CellularAutomata.Populations;  // reference Variety, IPopulation, Simple
using CellularAutomata.Populations.Rules;  // reference IRule, Absolute
using CellularAutomata.Commands;  // reference Option

namespace CellularAutomata.Commands  // console UI interface
{

	// Command: Population, pop 
	public class PopulationControl  // interfaces console access to Population
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

			// reference holder for either population or clone
			IPopulation tempPopulation;

			// format: (target) [(intial conditions)]
			switch (arguments [0]) {

			case "population":
			case "p":

				tempPopulation = population;

				break;

			case "clone":
			case "c":

				tempPopulation = clone;

				break;

			}

			// implement same behavior on selelcted population through tempPopulation

			if (1 < arguments.Length) {  // if specified initial conditions

				int[] states = new int [arguments [1].Length];

				// get each character
				string[] stateStrings  = arguments [1].Split (new string [1] { string.Empty }, StringSplitOptions.RemoveEmptyEntries);

				for (int i = 0; i < stateStrings.Length; i++) {  // add manual states

					states [i] = Convert.ToInt32 (stateStrings [i]);

				}

				tempPopulation = new Simple (variety, size, states);  // constructor will correct for states

			} else {

				tempPopulation = new Simple (variety, size);

			}

			tempPopulation.SetRule (rule);  // setter dependancy

		}

	}

}
