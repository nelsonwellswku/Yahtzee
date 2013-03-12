using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yahtzee.Framework
{
	public class DiceCup : IDiceCup
	{
		private const string _invalidRollExceptionMessage = "Dice can not be thrown after three rolls.";
		private const int _maxRolls = 3;
		private int _rollCount = 0;

		public IEnumerable<IDie> Dice { get; private set; }
		public DiceCup(IEnumerable<IDie> dice)
		{
			Dice = dice;
		}

		public IEnumerable<IDie> Roll()
		{
			if (IsFinal())
			{
				throw new InvalidOperationException(_invalidRollExceptionMessage);
			}

			foreach (var die in Dice)
			{
				die.Roll();
			}

			_rollCount++;

			return Dice;
		}

		public void Hold(params int[] indicesToHold)
		{
			foreach (var index in indicesToHold)
			{
				Dice.ElementAt(index).State = DieState.Held;
			}
		}

		public void Unhold(params int[] indicesToHold)
		{
			foreach (var index in indicesToHold)
			{
				Dice.ElementAt(index).State = DieState.Throwable;
			}
		}

		public bool IsFinal()
		{
			return _rollCount == _maxRolls;
		}
	}
}