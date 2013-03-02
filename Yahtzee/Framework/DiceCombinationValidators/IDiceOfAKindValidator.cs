using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public interface IDiceOfAKindValidator
	{
		bool IsValid(int numberOfDiceToMatch, IEnumerable<int> dice);
	}
}