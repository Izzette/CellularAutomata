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
			case "tunnel":
				NewTunnel (options, arguments);
				break;
			case "evolve":
				Evolve (options, arguments);
				break;
			case "rule":
				SetRule (options, arguments);
				break;
			case "clone":
				Clone (options, arguments);
				break;
			case "states":
				CopyStates (options, arguments);
				break;
			default:
				CommandsWarning.MethodNotFound (Command, method);
				return;
			}  // end switch (method) statment

		}  // end Excecute, public static void

		// IPopulations
		private static IPopulation population;
		private static IPopulation clone;

		// default populations
		private static CellsVariety DefaultCellsVariety {
			get { return CellsVariety.General; }
		}

		static private int[] DefaultSizes {
			get { return new int [1] { 125 }; }
		}

		// default initial conditions
		static private ushort[] DefaultValues {
			get { return new ushort [1] { 1 }; }
		}

		static private IRule DefaultRule {  // returns 2k110
			get {
				IRule rule = new Absolute ();
				rule.Parse ("(2,110)");
				return rule;
			}
		}

		private static OutputsFormat DefaultFormat {
			get { return OutputsFormat.Bitmap; }
		}

		private static string Command {
			get { return "Population"; }
		}

		private static int[] GetSizes (string code)
		{
			int[] sizes;
			string[] sizesStrings;
			sizesStrings = code.Split (new char [3] { '(', ')', ',' }, StringSplitOptions.RemoveEmptyEntries);
			sizes = new int [sizesStrings.Length];
			for (int i = 0; i < sizes.Length; i++) {
				sizes [i] = Convert.ToInt32 (sizesStrings [i]);
			}
			return sizes;
		}  // end GetSizes, private static int[] method

		private static IPopulation GetPopulation (string pop)
		{
			IPopulation tempPop;
			switch (pop) {
			case "p":
				tempPop = population;
				break;
			case "c":
				tempPop = clone;
				break;
			default:
				throw new FormatException ();
			}
			return tempPop;
		}

		private static IRule GetRule (string code)
		{
			IRule rule;
			switch (code) {
			case "a":
				rule = new Absolute ();
				break;
			case "t":
				rule = new Totalistic ();
				break;
			case "bt":
				rule = new BorderTotalistic ();
				break;
			case "ll":
				rule = new LifeLike ();
				break;
			case "avg":
				rule = new Average ();
				break;
			default:
				throw new FormatException ();
			} // end switch (code) statement
			return rule;
		}  // end GetRule, private static IRule method

		private static CellsVariety GetVariety (string code)
		{
			CellsVariety cellsVariety;
			switch (code) {
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
			case "h":
				cellsVariety = CellsVariety.Hexagonal;
				break;
			default:
				throw new FormatException ();
			} // end switch (option.Argumetns [0]) statment
			return cellsVariety;
		}  // end GetVariety, private static CellsVariety method

		private static ushort[] GetValues (string code)
		{
			ushort[] values;
			char[] valuesStrings = code.ToCharArray ();
			values = new ushort[valuesStrings.Length];
			for (int i = 0; i < values.Length; i++) {
				values [i] = Convert.ToUInt16 (valuesStrings [i].ToString ());
			}
			return values;
		}

		private static ushort[] GetRandomValues (string color, int[] sizes)
		{
			ushort[] values;
			int length = 1;
			foreach (int n in sizes) {
				length = length * n;
			}
			values = RandomSequence.GetSequence (Convert.ToInt32 (color), length);
			return values;
		}

		private static void NewPopulation (Option[] options, string[] arguments)
		{

			const string method = "new";

			int[] sizes = DefaultSizes;
			ushort[] values = DefaultValues;
			IRule rule = DefaultRule;
			CellsVariety cellsVariety = DefaultCellsVariety;
			bool init = false;

			foreach (Option option in options) {
				switch (option.Name) {
				case "i":
					init = true;
					break;
				case "s":
					try {
						sizes = GetSizes (option.Arguments [0]);
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					}
					break;
				case "r":
					try {
						rule = GetRule (option.Arguments [0]);
						rule.Parse (option.Arguments [1]);
					} catch (FormatException) {
						string combined = option.Arguments [0] + "or" + option.Arguments [1];
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, combined);
						return;
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					}
					break;
				case "v":
					try {
						cellsVariety = GetVariety (option.Arguments [0]);
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					} // end try statment
					try {
						sizes = GetSizes (option.Arguments [1]);
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [1]);
						return;
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, "<MISSING SIZE DECLAREATION>");
						return;
					}  // end try statment
					break;
				default:
					CommandsWarning.OptionNotFound (Command, method, option.Name);
					return;
				}  // end switch (option.Name) statement
			}  // end foreach (Option option in options) loop

			try {
				switch (arguments [1]) {
				case "str":
					values = GetValues (arguments [2]);
					break;
				case "rand":
					if (init) {
						int length = 1;
						foreach (int s in sizes) {
							length = length * s;
						}
						int color = Convert.ToInt32 (arguments [2]);
						int maxRandSize = (int)Math.Ceiling ((float)length / (int)(Math.Log (UInt32.MaxValue, color) - Math.Log (color, 2)));
						RandomSequence.Init (maxRandSize, maxRandSize, 5, 8);
					}
					values = GetRandomValues (arguments [2], sizes);
					break;
				default:
					CommandsWarning.ArgumentNotValid (Command, method, arguments [1]);
					return;
				}
			} catch (FormatException) {
				CommandsWarning.ArgumentNotValid (Command, method, arguments [2]);
				return;
			} catch (IndexOutOfRangeException) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}

			try {
				// select population
				switch (arguments [0]) {
				// main population
				case "p":
					population = new Simple (cellsVariety, sizes, values);
					population.SetRule (rule);
					break;
				case "c":
					clone = new Simple (cellsVariety, sizes, values);
					clone.SetRule (rule);
					break;
				default:
					CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
					return;
				}  // end switch (arguments [0]) statment
			} catch (ArgumentException) {
				CommandsWarning.OptionArgumentNotValid (Command, method, "<SIZE MISSING OR WRONG>");
				return;
			}  catch (IndexOutOfRangeException) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}  // end try statment
		}  // end NewPopulation, private static void method

		private static void NewTunnel (Option[] options, string[] arguments)
		{
			const string method = "tunnel";

			int[] sizes = DefaultSizes;
			int updateToDraw = 1;
			int scale = 1;

			foreach (Option option in options) {
				switch (option.Name) {
					case "s":
					try {
						sizes = GetSizes (option.Arguments [0]);
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					}
					break;
					case "u":
					try {
						updateToDraw = Convert.ToInt32 (option.Arguments [0]);
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					}
					break;
					case "r":
					try {
						scale = Convert.ToInt32 (option.Arguments [0]);
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
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

			try {
				// select population
				switch (arguments [0]) {
					// main population
					case "p":
					population = new Tunnel (sizes, Convert.ToDouble (arguments [1]), Convert.ToDouble (arguments [2]), updateToDraw, scale);
					break;
					case "c":
					clone = new Tunnel (sizes, Convert.ToDouble (arguments [1]), Convert.ToDouble (arguments [2]), updateToDraw, scale);
					break;
					default:
					CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
					return;
				}  // end switch (arguments [0]) statment
			} catch (ArgumentException) {
				CommandsWarning.OptionArgumentNotValid (Command, method, "<SIZE MISSING OR WRONG>");
				return;
			}  catch (IndexOutOfRangeException) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}  // end try statment
		}

		private static void Evolve (Option[] options, string[] arguments)
		{

			const string method = "evolve";

			string subDirName = String.Empty;
			int generation = 1;
			OutputsFormat format = DefaultFormat;

			foreach (Option option in options) {
				switch (option.Name) {
				case "d":
					try {
						subDirName = option.Arguments [0];
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
						return;
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					}
					break;
				case "g":
					try {
						generation = Convert.ToInt32 (option.Arguments [0]);
					} catch (FormatException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name, option.Arguments [0]);
						return;
					} catch (IndexOutOfRangeException) {
						CommandsWarning.OptionArgumentNotValid (Command, method, option.Name);
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
			case "bmpSect":
				format = OutputsFormat.BitmapSection;
				break;
			case "png":
				format = OutputsFormat.Png;
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [1]);
				return;
			}  // end switch (arguments [1]) statment

			IPopulation tempPop;

			try {
				tempPop = GetPopulation (arguments [0]);
			} catch (FormatException) {
				CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
				return;
			} catch (IndexOutOfRangeException) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}

			if (String.Empty == subDirName) {
				subDirName = tempPop.ToString () + "/";
			}

			string path = subDirName + "img" + "".PadLeft ((int)Math.Log10 (generation) + 1, '0');
			OutputsControl.Init (tempPop.GetStates (), generation, path, format);
			ConsoleKeyInfo cki = new ConsoleKeyInfo ();
			Console.Write (" (Press 'h' for help): ");
			for (int i = 1; i < generation; i++) {
				if (Console.KeyAvailable) {
					cki = Console.ReadKey (false);
					Console.WriteLine ();
					switch (cki.KeyChar) {
					case 'h':
						Console.WriteLine (" Pop evolve help menu:");
						Console.WriteLine (" key h: display this help text");
						Console.WriteLine (" key s: display evolve status");
						Console.WriteLine (" key p: pause");
						Console.WriteLine (" key q: break loop");
						break;
					case 's':
						Console.WriteLine (
							" Pop evolve status: {0} {1}% {2}/{3}",
							DateTime.Now,
							((int)(100D * (double)i / (double)generation)).ToString (),
							i.ToString (),
							generation.ToString ()
						);
						break;
					case 'p':
						Console.Write (" Paused, press enter to continue");
						Console.ReadLine ();
						break;
					case 'q':
						Console.Write (" Are you sure you want to break? [Y,n]: ");
						string answer = Console.ReadLine ();
						if ((String.Empty == answer) || ("y" == answer.ToLower ())) {
							return;
						}
						break;
					}
					Console.Write (" (Press 'h' for help): ");
				}
				tempPop.Evolve ();
				path = subDirName + "img" + i.ToString ().PadLeft ((int)Math.Log10 (generation) + 1, '0');
				OutputsControl.Update (tempPop.GetStates (), i, path, format);
			}
			path = subDirName + "img" + generation.ToString ().PadLeft ((int)Math.Log10 (generation) + 1, '0');
			OutputsControl.Final (path, tempPop.GetCellsArangement (), format);
			Console.Beep ();

		}  // end Evolve, private static void method

		private static void SetRule (Option[] options, string[] arguments)
		{

			const string method = "rule";
			IRule rule;

			try {
				rule = GetRule (arguments [1]);
				rule.Parse (arguments [2]);
			} catch (FormatException) {
				try {
					string combined = arguments [1] + " or " + arguments [2];
					CommandsWarning.ArgumentNotValid (Command, method, combined);
				} catch (IndexOutOfRangeException) {
					CommandsWarning.ArgumentNotValid (Command, method);
					return;
				}  // end try statment
				return;
			} catch (IndexOutOfRangeException) {
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}  // end try statment

			switch (arguments [0]) {
			case "p":
				population.SetRule (rule);
				break;
			case "c":
				clone.SetRule (rule);
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method);
				return;
			}  // end switch (arguments [0]) statment
		
		}  // end SetRule, private static void method

		private static void Clone (Option[] options, string[] arguments)
		{

			const string method = "clone";

			IPopulation tempClone;

			switch (arguments [1]) {
			case "p":
				tempClone = population.Clone ();
				break;
			case "c":
				tempClone = clone.Clone ();
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [1]);
				return;
			}  // end switch (arguments [1]) statment

			switch (arguments [0]) {
			case "p":
				population = tempClone.Clone ();
				break;
			case "c":
				clone = tempClone.Clone ();
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
				return;
			}  // end switch (arguments [1]) statment

		}  // end Clone, private static void method

		private static void CopyStates (Option[] options, string[] arguments)
		{

			const string method = "states";

			States states;

			switch (arguments [1]) {
			case "p":
				states = population.GetStates ();
				break;
			case "c":
				states = population.GetStates ();
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [1]);
				return;
			}  // end switch (arguments [1]) statment

			IRule rule;
			CellsVariety cellsVariety;
			switch (arguments [0]) {
			case "p":
				rule = population.GetRule ();
				cellsVariety = population.GetCellsVariety ();
				population = new Simple (cellsVariety, states.Sizes, states.Values);
				population.SetRule (rule);
				break;
			case "c":
				rule = clone.GetRule ();
				cellsVariety = clone.GetCellsVariety ();
				clone = new Simple (cellsVariety, states.Sizes, states.Values);
				clone.SetRule (rule);
				break;
			default:
				CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
				return;
			}  // end switch (arguments [0]) statment
		
		}  // end CopyStates, private static void method

	}  // end PopulationsControl, public static class

}  // end CellularAutomata.Commands, namespace
