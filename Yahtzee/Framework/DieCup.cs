using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework
{
	public class DieCup : IDieCup
	{
		private readonly IDie _die;

		public DieCup(IDie die)
		{
			_die = die;
		}

		public IEnumerable<int> Roll()
		{
			return Roll(6);
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