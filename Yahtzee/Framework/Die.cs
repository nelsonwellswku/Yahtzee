using System;

namespace Yahtzee.Framework
{
	public class Die : IDie
	{
		private const int _lowestPossibleRoll = 1;
		private const int _highestPossibleRoll = 6;
		private readonly Random _random;

		public Die()
		{
			_random = new Random();
		}

		public int Roll()
		{
			return _random.Next(_lowestPossibleRoll, _highestPossibleRoll + 1);
		}
	}
}