using System;
using CellularAutomata.Commands;  // reference ApplicationCommand, Option, PopulationControl

namespace CellularAutomata.Commands  // console UI interface
{
	
	public static class Interpreter
	{	

		private static string Parse (string line, out string method, out Option[] options, out string[] arguments)
		{

			// split by spaces
			string[] phrases = line.Split (new char [1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			// command to be returned
			string command = phrases [0];

			// out method
			method = phrases [1];

			// options and arugments initialization lengths
			int numberOption = 0;
			int numberArgument = 0;

			// set numberOption and numberArugment
			for (int i = 2; i < phrases.Length; i++) {
				if (phrases [i].StartsWith ("-")) {
					numberOption++;
				} else {
					numberArgument++;
				}
			}

			// initalize options and arguments
			options = new Option [numberOption];
			arguments = new string [numberArgument];

			for (int i = 0; i < options.Length; i++) {
				options [i] = new Option (phrases [2 + i]);
			}

			for (int i = 0; i < arguments.Length; i++) {
				arguments [i] = phrases [2 + options.Length + i];
			}

			// success!
			return command;

		}  // end Parse, private static string method

		public static ApplicationCommand Excecute (string line) // returns master application command enum
		{

			// line format$ Command method [-options[:optionArguments]] [arguments]

			string command;
			string method;
			Option[] options;
			string[] arguments;

			// attempt to parse
			try {
				command = Parse (line, out method, out options, out arguments);
			} catch (IndexOutOfRangeException) {
				CommandsWarning.CommandNotFound ("or Method)");
				return ApplicationCommand.Continue;
			}

			switch (command) {
			// population manager class
			case "Pop":
				PopulationsControl.Excecute (method, options, arguments);
				break;
			case "Out":
				OutputsControl.Excecute (method, options, arguments);
				break;
			case "Rand":
				switch (method) {
				case "init":
					RandomSequence.Init ();
					break;
				default:
					CommandsWarning.MethodNotFound (command, method);
					return ApplicationCommand.Continue;
				}
				break;
			// master application manager
			case "App":
				switch (method) {
				case "reset":  // call entry again
					return ApplicationCommand.Reset;
				case "quit":  // quit app
					return ApplicationCommand.Quit;
				default:  // could not find method
					CommandsWarning.MethodNotFound (command, method);
					return ApplicationCommand.Continue;
				} // end switch (method) statement
			// could not find command
			default:
				CommandsWarning.CommandNotFound (command);
				return ApplicationCommand.Continue;
			}  // end switch (command) statement

			// continue parse line loop in Entry Point
			return ApplicationCommand.Continue;

		}  // end Parse, public static ApplicationCommand method
		
	}  // end Interpreter, public static class
	
}  // end CellularAutomata.Commands, namespace
