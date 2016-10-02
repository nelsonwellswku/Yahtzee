using System.Collections.Generic;

namespace Octogami.Yahtzee.Application.Framework.DiceCombinationValidators
{
	public interface IStraightValidator
	{
		bool IsValid(int lengthOfStraight, IEnumerable<IDie> diceValues);
	}
}