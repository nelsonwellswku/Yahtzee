using System;

namespace Yahtzee.Framework
{
	public class Die : IDie
	{
		private const int _lowestPossibleRoll = 1;
		private const int _highestPossibleRoll = 6;
		private static Random _random = new Random();

		public DieState State { get; set; }
		public int Value { get; private set; }

		public Die() : this(DieState.Throwable) { }

		private Die(DieState state)
		{
			State = state;
		}

		public int Roll()
		{
			Value = State == DieState.Held ? Value : _random.Next(_lowestPossibleRoll, _highestPossibleRoll + 1);
			return Value;
		}
	}
}