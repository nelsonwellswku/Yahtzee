using System.Collections.Generic;

namespace Yahtzee.Framework
{
	public interface IDieCup
	{
		IEnumerable<int> Roll();
		IEnumerable<int> Roll(int numberToRoll);
	}
}