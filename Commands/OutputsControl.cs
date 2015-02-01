using System;
using System.IO;
using System.Security.AccessControl;
using CellularAutomata.Commands;
using CellularAutomata.Populations;
using CellularAutomata.Outputs;

namespace CellularAutomata.Commands
{
	public static class OutputsControl
	{

		public static void Excecute (string method, Option[] options, string[] arguments)
		{
			switch (method) {
			case "rescale":
				if (2 > arguments.Length) {
					CommandsWarning.ArgumentNotValid (Command, method);
					return;
				}
				switch (arguments [0]) {
				case "one":
					try {
						OneDCubicImageManager.ReScale (Convert.ToInt32 (arguments [1]));
					} catch (FormatException) {
						CommandsWarning.ArgumentNotValid (Command, method, "<NEW SCALE>");
						return;
					}
					break;
				case "two":
					try {
						TwoDCubicImageManager.ReScale (Convert.ToInt32 (arguments [1]));
					} catch (FormatException) {
						CommandsWarning.ArgumentNotValid (Command, method, "<NEW SCALE>");
						return;
					}
					break;
				case "hex":
					try {
						TwoDHexagonalImageManager.ReScale (Convert.ToInt32 (arguments [1]));
					} catch (FormatException) {
						CommandsWarning.ArgumentNotValid (Command, method, "<NEW SCALE>");
						return;
					}
					break;
				case "tunnel":
					try {
						TunnelImageManager.ReScale (Convert.ToInt32 (arguments [1]));
					} catch (FormatException) {
						CommandsWarning.ArgumentNotValid (Command, method, "<NEW SCALE>");
					}
					break;
				default:
					CommandsWarning.ArgumentNotValid (Command, method, arguments [0]);
					return;
				}
				break;
			default:
				CommandsWarning.MethodNotFound (Command, method);
				return;
			}
		}

		public static void Init (States states, int maxGeneration, string path, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			string[] splitPath = path.Split (new char [1] { '/' }, StringSplitOptions.RemoveEmptyEntries);

			if (2 == splitPath.Length) {
				bin.CreateSubdirectory (splitPath [0]);
			}

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
			case OutputsFormat.Png:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Init (states.Values, maxGeneration);
					break;
				case CellsArangement.TwoDCubic:
					TwoDCubicImageManager.Init (states.Sizes, states.Values);
					SaveImage (path, cellsArangement, format);
					break;
				case CellsArangement.TwoDHexagonal:
					TwoDHexagonalImageManager.Init (states.Sizes, states.Values);
					SaveImage (path, cellsArangement, format);
					break;
				case CellsArangement.Tunnel:
					TunnelImageManager.Init (states.Sizes, states.Values);
					SaveImage (path, cellsArangement, format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statement
				break;
			case OutputsFormat.BitmapSection:
				switch (cellsArangement) {
				case CellsArangement.TwoDCubic:
				case CellsArangement.OneDCubic:
				case CellsArangement.TwoDHexagonal:
				case CellsArangement.Tunnel:
					int[] sectionValues = new int [states.Sizes [0]];
					for (int i = 0; i < sectionValues.Length; i++) {
						sectionValues [i] = states.Values [i];
					}
					OneDCubicImageManager.Init (sectionValues, maxGeneration);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			default:
				throw new ArgumentException ();
			}  // end switch (format) statment

		}  // end Init, public static void method

		// collection name contains CellsVariety, IPopulation type, IRule as string
		public static void Update (States states, int currentGeneration, string path, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			CellsArangement cellsArangement = states.Arangement;

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
			case OutputsFormat.Png:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					OneDCubicImageManager.Update (states.Values, currentGeneration);
					break;
				case CellsArangement.TwoDCubic:
					TwoDCubicImageManager.Update (states.Values);
					SaveImage (path, cellsArangement, format);
					break;
				case CellsArangement.TwoDHexagonal:
					TwoDHexagonalImageManager.Update (states.Values);
					SaveImage (path, cellsArangement, format);
					break;
				case CellsArangement.Tunnel:
					TunnelImageManager.Update (states.Values);
					SaveImage (path, cellsArangement, format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statment
				break;
			case OutputsFormat.BitmapSection:
				switch (cellsArangement) {
				case CellsArangement.TwoDCubic:
				case CellsArangement.OneDCubic:
				case CellsArangement.TwoDHexagonal:
				case CellsArangement.Tunnel:
					int[] sectionValues = new int [states.Sizes [0]];
					for (int i = 0; i < sectionValues.Length; i++) {
						sectionValues [i] = states.Values [i];
					}
					OneDCubicImageManager.Update (sectionValues, currentGeneration);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			default:
				throw new ArgumentException ();
			}  // end switch (format) statment

		}  // end Update, public static void method

		public static void Final (string path, CellsArangement cellsArangement, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			switch (format) {
			case OutputsFormat.Gif:
			case OutputsFormat.Bitmap:
			case OutputsFormat.Png:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					SaveImage (path, cellsArangement, format);
					break;
				case CellsArangement.TwoDCubic:
				case CellsArangement.TwoDHexagonal:
				case CellsArangement.Tunnel:
					return;
				default:
					throw new ArgumentException ();
				}
				break;
			case OutputsFormat.BitmapSection:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
				case CellsArangement.TwoDCubic:
				case CellsArangement.TwoDHexagonal:
				case CellsArangement.Tunnel:
					SaveImage (path + "Sect", cellsArangement, format);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			default:
				throw new ArgumentException ();
			}

		}

		private static void SaveImage (string path, CellsArangement cellsArangement, OutputsFormat format)
		{

			if (OutputsFormat.Quiet == format) {
				return;
			}

			path = "bin/" + path;
			bool success;

			switch (format) {
			case OutputsFormat.Bitmap:
			case OutputsFormat.Gif:
			case OutputsFormat.Png:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
					success = OneDCubicImageManager.Save (path, format);
					break;
				case CellsArangement.TwoDCubic:
					success = TwoDCubicImageManager.Save (path, format);
					break;
				case CellsArangement.TwoDHexagonal:
					success = TwoDHexagonalImageManager.Save (path, format);
					break;
				case CellsArangement.Tunnel:
					success = TunnelImageManager.Save (path, format);
					break;
				default:
					throw new ArgumentException ();
				}  // end switch (cellsArangement) statment
				break;
			case OutputsFormat.BitmapSection:
				switch (cellsArangement) {
				case CellsArangement.OneDCubic:
				case CellsArangement.TwoDCubic:
				case CellsArangement.TwoDHexagonal:
				case CellsArangement.Tunnel:
					success = OneDCubicImageManager.Save (path, format);
					break;
				default:
					throw new ArgumentException ();
				}
				break;
			default:
				throw new ArgumentException ();
			}

			if (!success) {
				throw new IOException ();
			}

		}  // end SaveBitmap, public static void method

		private static string Command {
			get { return "OutputsControl"; }
			set { ; }
		}

		private static DirectoryInfo bin = Directory.CreateDirectory ("bin");
	
	}  // end OutputsControl, public static class

}  // end CellularAutomata.Commands namespace

