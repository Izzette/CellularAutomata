using System;
using CellularAutomata.Commands;
using System.IO;

namespace HexBt3K
{

	public static class Program
	{

		public static void Main ()
		{
			DirectoryInfo bin = Directory.CreateDirectory ("bin");
			FileInfo[] fileInfo = bin.GetFiles ();
			foreach (FileInfo info in fileInfo) {
				string name = info.Name;
				string number = name.Split (new char [1] { '_' }, StringSplitOptions.RemoveEmptyEntries) [0];
				Interpreter.Excecute (String.Format ("Pop new -v:h:(100,100) -r:bt:(3,{0}) p str 1", number));
				Interpreter.Excecute ("Pop evolve -g:100 p bitmap");
			}
		}

	}

}

