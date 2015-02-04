using System;
using System.Collections.Generic;
using System.Linq;

namespace Yahtzee.Framework
{
	public class DiceCup : IDiceCup
	{
		private const string _invalidRollExceptionMessage = "Dice can not be thrown after three rolls.";
		private const int _maxRolls = 3;
		private int _rollCount = 0;

		public virtual IList<IDie> Dice { get; private set; }
		public DiceCup(IList<IDie> dice)
		{
			if (dice.Count() != 5) throw new ArgumentOutOfRangeException("Dice cup must contain 5 die.");
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


		public int RollCount
		{
			get { return _rollCount; }
		}
	}
}