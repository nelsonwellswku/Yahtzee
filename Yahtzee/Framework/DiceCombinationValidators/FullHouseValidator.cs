using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class FullHouseValidator : IFullHouseValidator
	{
		public bool IsValid(IEnumerable<int> diceValues)
		{
			var uniqueValues = diceValues.Distinct();

			if (uniqueValues.Count() != 2) return false;
			if (diceValues.Count(x => x == uniqueValues.ElementAt(0)) == 2 && diceValues.Count(x => x == uniqueValues.ElementAt(1)) == 3) return true;
			if (diceValues.Count(x => x == uniqueValues.ElementAt(0)) == 3 && diceValues.Count(x => x == uniqueValues.ElementAt(1)) == 2) return true;

			return false;
		}
	}
}
