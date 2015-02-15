using System.Collections.Generic;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public interface IDiceOfAKindValidator
	{
		bool IsValid(int numberOfDiceToMatch, IEnumerable<IDie> dice);
	}
}