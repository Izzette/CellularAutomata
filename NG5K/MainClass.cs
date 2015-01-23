using System;
using CellularAutomata.Commands;

namespace NG5K
{

	public static class MainClass
	{

		public static void Main (string[] args)
		{

			int depth = Convert.ToInt32 (args [0]);

			Random random = new Random ();

			Interpreter.Excecute ("Out rescale one 2");
			Interpreter.Excecute ("Pop new -v:ng:(1500) -i p rand 2");

			for (int i = 0; i < depth; i++) {

				long ruleNumber = 0;

				for (int ie = 0; ie < 21; ie++) {
					ruleNumber = ruleNumber * 5;
					ruleNumber += random.Next (0, 5);
				}

				Interpreter.Excecute ("Pop new -v:ng:(750) p rand 5");
				Interpreter.Excecute (String.Format ("Pop rule p bt (5,{0})", ruleNumber.ToString ()));
				Interpreter.Excecute (String.Format ("Pop evolve -g:540 -d:NG5K/{0}_ p bitmap", ruleNumber.ToString ()));

			}
		
		}
	
	}

}

