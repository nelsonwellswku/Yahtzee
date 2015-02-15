using System;

namespace Yahtzee.Framework
{
	public class Die : IDie
	{
		private const int LowestPossibleRoll = 1;
		private const int HighestPossibleRoll = 6;
		private static readonly Random Random = new Random();

		public DieState State { get; set; }
		public int Value { get; private set; }

		public Die() : this(DieState.Throwable) { }

		private Die(DieState state)
		{
			State = state;
		}

		public int Roll()
		{
			Value = State == DieState.Held ? Value : Random.Next(LowestPossibleRoll, HighestPossibleRoll + 1);
			return Value;
		}
	}
}