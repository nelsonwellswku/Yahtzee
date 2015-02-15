using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Framework
{
	public class DiceCup : IDiceCup
	{
		private const string InvalidRollExceptionMessage = "Dice can not be thrown after three rolls.";
		private const int MaxRolls = 3;

		public virtual IList<IDie> Dice { get; private set; }
		public DiceCup(IList<IDie> dice)
		{
			RollCount = 0;
			if (dice.Count() != 5) throw new ArgumentOutOfRangeException("dice", "Dice cup must contain 5 die.");
			Dice = dice;
		}

		public IList<IDie> Roll()
		{
			if (IsFinal())
			{
				return null;
			}

			foreach (var die in Dice)
			{
				die.Roll();
			}

			RollCount++;
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
			return RollCount == MaxRolls;
		}


		public int RollCount { get; private set; }
	}
}