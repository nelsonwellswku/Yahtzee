using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class DiceOfAKindValidator : IDiceOfAKindValidator
	{
		public bool IsValid(int numberOfDiceToMatch, IEnumerable<IDie> dice)
		{
			foreach (var uniqueValue in dice.Distinct().Select(x => x.Value))
			{
				if (dice.Count(x => x.Value == uniqueValue) == numberOfDiceToMatch)
				{
					return true;
				}
			}

			return false;
		}
	}
}