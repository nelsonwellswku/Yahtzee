using System.Collections.Generic;

namespace Octogami.Yahtzee.Application.Framework.DiceCombinationValidators
{
	public interface IFullHouseValidator
	{
		bool IsValid(IEnumerable<IDie> diceValues);
	}
}