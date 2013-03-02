using System.Collections.Generic;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public interface IFullHouseValidator
	{
		bool IsValid(IEnumerable<int> diceValues);
	}
}