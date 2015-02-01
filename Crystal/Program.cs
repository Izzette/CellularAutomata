using System;
using CellularAutomata.Commands;

namespace Crystal
{

	public static class Program
	{

		public static void Main ()
		{

			string rescaleCommand = "Out rescale two 1";
			Console.WriteLine (rescaleCommand);
			Interpreter.Excecute (rescaleCommand);

			Random random = new Random (932023);
			string seed = string.Empty;

			for (int i = 0; i < 32; i++) {
				seed += random.Next (0, 2).ToString ();
			}

			int rowPadLength = 1080 / 2;
			int vertPadLength = (int)(Math.Pow (1080D, 2D) / 2D);
			int totalPadLength = rowPadLength + vertPadLength;
			string paddedSeed = seed.PadLeft (totalPadLength, '0');

			string newCommand = "Pop new -v:m:(1080,1080) -r:bt:(2,174826) p str " + paddedSeed;
			Console.WriteLine ("Pop new -v:m:(1080,1080) -r:bt:(2,174826) p pad 0x{0} {1}", totalPadLength.ToString (), seed);
			Interpreter.Excecute (newCommand);

			string quietCommand = "Pop evolve -g:5 p quiet";

			for (int i = 0; i < 1000; i++) {
				string itterationString = i.ToString ().PadLeft (3, '0');
				string bitmapCommand = String.Format ("Pop evolve -g:0 -d:Crystal/img{0} p bitmap", itterationString);
				Console.WriteLine (" >> " + bitmapCommand);
				Interpreter.Excecute (bitmapCommand);
				Console.WriteLine (" >> " + quietCommand);
				Interpreter.Excecute (quietCommand);
			}

		}

	}

}

