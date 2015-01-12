using System;
using CellularAutomata.Commands;  // reference ApplicationCommands, Option, PopulationControl

namespace CellularAutomata.Commands  // console UI interface
{
	
	public class Interpreter
	{	

		public static ApplicationCommands Parse (string line) // returns master application command enum
		{

			// line format$ Command method [-options[:optionArguments]] [arguments]

			string[] phrases = line.Split (new char[1] {' '}, StringSplitOptions.RemoveEmptyEntries);  // split by spaces

			string command = phrases [0];  // first in array is Command
			string method = phrases [1];  // second in array is Method

			// options and arguments to be parsed
			Option[] options;
			string[] arguments;

			int index = 2;  // hard(er) index of parsing phrases, skip command

			int numberOption = 0;  // number of options, for options array size

			for (int i = index; i < phrases.Length; i++) {  // options, incrementing local int i, just peeking

				if (phrases [i].StartsWith ("-")) {  // is an option

					numberOption++;  // increment the number of options

				} else {

					break;  // break at arguments

				}

			}

			options = new Option[numberOption];  // create array of options

			for ( ; index < numberOption; index++) {  // add each option to array

				string[] optionStrings = phrases [index].Split (new char[] { '-', ':' }, StringSplitOptions.RemoveEmptyEntries);  //split the options and it's arguments

				string optionName = optionStrings [0];  // option name

				string[] optionArgs = new string[optionStrings.Length - 1];  // create array for option arguments, does not include option name

				for (int i = 0; i < optionArgs.Length; i++) {  // loop through option arguments if not empty

					optionArgs [i] = optionStrings [i + 1];  // add to array

				}

				options [index] = new Option (optionName, optionArgs);  // create option

			}

			arguments = new string[phrases.Length - index];  // create array for arguments, may be empty

			for (int i = 0; i < arguments.Length; i++) {  // loop through if not empty

				arguments [i] = phrases [i + index];  // add arguments

			}

			switch (command) {  // distribute!

			// population manager class
			case "pop":
			case "Population":

				switch (method) {

				case "new":  // create new population

					PopulationsControl.New (options, arguments);

					break;

				// evolve
				case "evolve":

					PopulationsControl.Evolve (options, arguments);

					break;

				default:  // could not find method

					string message = "Not a valid command!  Could not find command method Population ";
					message += method;

					throw new FormatException (message);

				}

				break;
			
			// master application manager
			case "app":
			case "Application":

				switch (method) {

				case "reset":  // call entry again

					return ApplicationCommands.Reset;

				case "quit":  // quit app

					return ApplicationCommands.Quit;

				default:  // could not find method

					string message = "Not a valid command!  Could not find command method Application ";
					message += method;

					throw new FormatException (message);

				}

			default:  // could not find command

				string lowMessage = "Not a valid command!  Could not find command ";
				lowMessage += command;

				throw new FormatException (lowMessage);

			}

			return ApplicationCommands.Continue;  // continue parse line loop in Entry Point

		}
		
	}
	
}
