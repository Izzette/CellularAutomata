using System;
using CellularAutomata.Commands;

namespace HexT1K
{
	public static class Program
	{

		public static void Main ()
		{
			Interpreter.Excecute ("Rand init");
			int number = 0;
			for (int i = 0; i < (int)Math.Pow(2, 8); i++) {
				Interpreter.Excecute (String.Format ("Pop new -v:h:(50,50) -r:t:(2,{0}) p rand 2", number.ToString ()));
				Interpreter.Excecute ("Pop evolve -g:20 p bitmap");
				number++;
			}
		}

	}
}

