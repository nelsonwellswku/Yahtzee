using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class StraightValidator : IStraightValidator
	{
		public bool IsValid(int lengthOfStraight, IEnumerable<int> diceValues)
		{
			IEnumerable<int> orderedDice = diceValues.OrderBy(x => x);
			for (int i = 0; i < lengthOfStraight - 1; i++)
			{
				var currentIncrementedValue = orderedDice.ElementAt(i) + 1;
				if (!(currentIncrementedValue == orderedDice.ElementAt(i + 1))) return false;
			}

			return true;
		}
	}
}
