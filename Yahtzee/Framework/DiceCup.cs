using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework
{
	public class DiceCup : IDiceCup
	{
		private readonly IDie _die;

		public DiceCup(IDie die)
		{
			_die = die;
		}

		public IEnumerable<int> Roll()
		{
			return Roll(5);
		}

		public IEnumerable<int> Roll(int numberToRoll)
		{
			var rolls = new int[numberToRoll];

			for (int i = 0; i < numberToRoll; i++)
			{
				rolls[i] = _die.Roll();
			}

			return rolls;
		}
	}
}