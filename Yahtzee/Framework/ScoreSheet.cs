﻿using System;
using System.Linq;

using Yahtzee.Framework.DiceCombinationValidators;

namespace Yahtzee.Framework
{
	public class ScoreSheet
	{
		private readonly IDiceOfAKindValidator _diceOfAKindValidator;
		private readonly IFullHouseValidator _fullHouseValidator;
		private readonly IStraightValidator _straightValidator;

		public int? ThreeOfAKind { get; private set; }
		public int? FourOfAKind { get; private set; }
		public int? FullHouse { get; private set; }

		public ScoreSheet(IDiceOfAKindValidator diceOfAKindValidator, IFullHouseValidator fullHouseValidator, IStraightValidator straightValidator)
		{
			_diceOfAKindValidator = diceOfAKindValidator;
			_fullHouseValidator = fullHouseValidator;
			_straightValidator = straightValidator;
		}

		public int? RecordThreeOfAKind(IDiceCup diceCup)
		{
			if (ThreeOfAKind != null) return null;

			if (_diceOfAKindValidator.IsValid(3, diceCup.Dice))
			{
				ThreeOfAKind = diceCup.Dice.Select(x => x.Value).Sum();
			}
			else
			{
				ThreeOfAKind = 0;
			}

			return ThreeOfAKind;
		}

		public int? RecordFourOfAKind(IDiceCup diceCup)
		{
			if (FourOfAKind != null) return null;

			if (_diceOfAKindValidator.IsValid(4, diceCup.Dice))
			{
				FourOfAKind = diceCup.Dice.Select(x => x.Value).Sum();
			}
			else
			{
				FourOfAKind = 0;
			}

			return FourOfAKind;
		}

		public int? RecordFullHouse(IDiceCup diceCup)
		{
			if (FullHouse != null) return null;

			if (_fullHouseValidator.IsValid(diceCup.Dice))
			{
				FullHouse = 25;
			}
			else
			{
				FullHouse = 0;
			}

			return FullHouse;
		}
	}
}
