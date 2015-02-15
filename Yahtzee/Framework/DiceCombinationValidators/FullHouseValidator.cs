using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class FullHouseValidator : IFullHouseValidator
	{
		public bool IsValid(IEnumerable<IDie> diceValues)
		{
			var values = diceValues.ToList();
			var uniqueValues = values.Select(x => x.Value).Distinct().ToList();

			if(uniqueValues.Count() != 2) return false;
			if(values.Count(x => x.Value == uniqueValues.ElementAt(0)) == 2 && values.Count(x => x.Value == uniqueValues.ElementAt(1)) == 3) return true;
			if(values.Count(x => x.Value == uniqueValues.ElementAt(0)) == 3 && values.Count(x => x.Value == uniqueValues.ElementAt(1)) == 2) return true;

			return false;
		}
	}
}