using System.Collections.Generic;

namespace Octogami.Yahtzee.Application.Framework.DiceCombinationValidators
{
	public interface IDiceOfAKindValidator
	{
		bool IsValid(int numberOfDiceToMatch, IEnumerable<IDie> dice);
	}
}