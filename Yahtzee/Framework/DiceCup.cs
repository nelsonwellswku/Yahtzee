using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework
{
	public class DiceCup : IDiceCup
	{
		public IEnumerable<IDie> Dice { get; private set; }
		public DiceCup(IEnumerable<IDie> dice)
		{
			Dice = dice;
		}

		public IEnumerable<IDie> Roll()
		{
			foreach (var die in Dice)
			{
				die.Roll();
			}

			return Dice;
		}

		public void Hold(params int[] indicesToHold)
		{
			foreach (var index in indicesToHold)
			{
				Dice.ElementAt(index).State = DieState.Held;
			}
		}
	}
}