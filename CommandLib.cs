using System;
using CellularAutomata.Commands;

namespace CellularAutomata.Commands
{
	
	public class Command
	{	
		
		// static var current command
		
		public static bool Interpret () // returns false if quit
		{
			
			string line = Console.ReadLine ();
			string[] phrases = line.Split (new char[1] {' '}, StringSplitOptions.RemoveEmptyEntries);
			
			ICommand command;
			int number = 0;
			
			foreach (string p in phrases) {
				
				if (p.StartsWith ("-") {
					
					number++;
					
					
				} else {
					
					//switch command: initialize command
				
				}
				
				int number = 0;
				
				foreach (string o in phrases) {
					
					
					
				}
				
				Option[] options = new Option[number];
				
				
				
			}
			
			
		}
		
	}
	
}
