using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework.DiceCombinationValidators
{
	public class StraightValidator : IStraightValidator
	{
		public bool IsValid(int lengthOfStraight, IEnumerable<IDie> dice)
		{
			List<int> orderedValues = dice.OrderBy(x => x.Value).Select(x => x.Value).Distinct().ToList();
			return CheckValidity(lengthOfStraight, orderedValues);
		}

		private bool CheckValidity(int lengthOfStraight, List<int> orderedValues)
		{
			if(orderedValues.Count < lengthOfStraight)
			{
				return false;
			}

			var isValid = true;

			for (int i = 0; i < lengthOfStraight - 1; i++)
			{
				var expectedNextValue = orderedValues[i] + 1;
				var nextValue = orderedValues[i + 1];
				if (expectedNextValue != nextValue)
				{
					isValid = CheckValidity(lengthOfStraight, orderedValues.Skip(1).ToList());
				}
			}

			return isValid;
		}
	}
}
