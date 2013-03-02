using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class DiceOfAKindValidator : IDiceOfAKindValidator
	{
		public bool IsValid(int numberOfDiceToMatch, IEnumerable<int> dice)
		{
			foreach (var uniqueValue in dice.Distinct())
			{
				if (dice.Count(x => x == uniqueValue) == numberOfDiceToMatch)
				{
					return true;
				}
			}

			return false;
		}
	}
}