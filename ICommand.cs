using System;
using CellularAutomata.Commands;

namespace CellularAutomata.Commands
{
	
	public interface ICommand
	{
		
		string Excecute (Option[] options, string[] arguments);
		
	}

}
