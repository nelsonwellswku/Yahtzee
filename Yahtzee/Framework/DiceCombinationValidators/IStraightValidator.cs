using System.Collections.Generic;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public interface IStraightValidator
	{
		bool IsValid(int lengthOfStraight, IEnumerable<IDie> diceValues);
	}
}