using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class DiceOfAKindValidator : IDiceOfAKindValidator
	{
		public bool IsValid(int numberOfDiceToMatch, IEnumerable<IDie> dice)
		{
			var values = dice.ToList();
			foreach (var uniqueValue in values.Select(x => x.Value).Distinct())
			{
				if (values.Count(x => x.Value == uniqueValue) >= numberOfDiceToMatch)
				{
					return true;
				}
			}

			return false;
		}
	}
}