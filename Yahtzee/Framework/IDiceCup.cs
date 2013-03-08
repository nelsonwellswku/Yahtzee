using System.Collections.Generic;

namespace Yahtzee.Framework
{
	public interface IDiceCup
	{
		IEnumerable<int> Roll();
		IEnumerable<int> Roll(int numberToRoll);
	}
}