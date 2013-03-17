using System;
using System.Collections.Generic;
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
		public int? SmallStraight { get; private set; }
		public int? LargeStraight { get; private set; }
		public int? Chance { get; private set; }
		public int? Yahtzee { get; private set; }
		public IEnumerable<int> YahtzeeBonus { get; private set; }

		public ScoreSheet(IDiceOfAKindValidator diceOfAKindValidator, IFullHouseValidator fullHouseValidator, IStraightValidator straightValidator)
		{
			_diceOfAKindValidator = diceOfAKindValidator;
			_fullHouseValidator = fullHouseValidator;
			_straightValidator = straightValidator;

			YahtzeeBonus = new List<int>();
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

		public int? RecordSmallStraight(IDiceCup diceCup)
		{
			if (SmallStraight != null) return null;

			if (_straightValidator.IsValid(4, diceCup.Dice))
			{
				SmallStraight = 30;
			}
			else
			{
				SmallStraight = 0;
			}

			return SmallStraight;
		}

		public int? RecordLargeStraight(IDiceCup diceCup)
		{
			if (LargeStraight != null) return null;

			if (_straightValidator.IsValid(5, diceCup.Dice))
			{
				LargeStraight = 40;
			}
			else
			{
				LargeStraight = 0;
			}

			return LargeStraight;
		}

		public int? RecordChance(IDiceCup diceCup)
		{
			if (Chance != null) return null;

			Chance = diceCup.Dice.Select(x => x.Value).Sum();
			return Chance;
		}

		public int? RecordYahtzee(IDiceCup diceCup)
		{
			bool isValidYahtzeeCombination = _diceOfAKindValidator.IsValid(5, diceCup.Dice);

			if (Yahtzee != null && Yahtzee > 0 && isValidYahtzeeCombination)
			{
				if (YahtzeeBonus.Count() == 3) return null;
				RecordYahtzeeBonus();
				return 100;
			}
			else if (Yahtzee != null)
			{
				return null;
			}

			if (isValidYahtzeeCombination)
			{
				Yahtzee = 50;
			}
			else
			{
				Yahtzee = 0;
			}

			return Yahtzee;
		}

		private void RecordYahtzeeBonus()
		{
			var tempList = YahtzeeBonus.ToList();
			tempList.Add(100);
			YahtzeeBonus = tempList;
		}
	}
}
