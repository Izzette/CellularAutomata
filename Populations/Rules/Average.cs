using System;

namespace CellularAutomata.Populations
{
	public class Average : IRule
	{
		public Average ()
		{
			Console.WriteLine (" INFORMATION: Fuild rules are ment only to be applied to VN neighbourhood");
		}

		public void Parse (string code) { ; }  // only has one state

		// states 0 to 7, on moore neighbourhood only
		public ushort Implement (ICell cell)
		{
			int neighbourhood = cell.GetNeighbourhood (16);
			int newState = 0;
			int numberLocal = 0;
			for (int i = 1; i < 5; i++) {
				int index = (4 * i) + ((i + 1) % 4);
				int placeValue = (int)Math.Pow (2, index);
				int oldState = newState;
				newState += ((neighbourhood % (2 * placeValue)) / placeValue) * (int)Math.Pow (2, (i + 1) % 4);
				if (oldState != newState) {
					numberLocal++;
				}
			}
			if (1 < numberLocal) {
				int oldState = newState;
				newState = 0;
				for (int i = 0; i < 4; i++) {
					newState += ((oldState % (int)Math.Pow (2, i + 1)) / (int)Math.Pow (2, i)) * (int)Math.Pow (2, (i + 2) % 4);
				}
			}
			return (ushort)newState;
		}

		public new string ToString ()
		{
			return code;
		}

		public IRule Clone ()
		{
			return new Average ();
		}

		private const string code = "avg";
	
	}

}

