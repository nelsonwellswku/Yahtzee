using System.Collections.Generic;
using System.Linq;
using Yahtzee.Framework.DiceCombinationValidators;

namespace Yahtzee.Framework
{
	public class ScoreSheet : IScoreSheet
	{
		private readonly IDiceOfAKindValidator _diceOfAKindValidator;
		private readonly IFullHouseValidator _fullHouseValidator;
		private readonly IStraightValidator _straightValidator;

		public int? Ones { get; private set; }
		public int? Twos { get; private set; }
		public int? Threes { get; private set; }
		public int? Fours { get; private set; }
		public int? Fives { get; private set; }
		public int? Sixes { get; private set; }

		public int UpperSectionBonus
		{
			get { return UpperSectionTotal >= 63 ? 35 : 0; }
		}

		public int UpperSectionTotal
		{
			get { return (int) (Ones + Twos + Threes + Fours + Fives + Sixes); }
		}

		public int UpperSectionTotalWithBonus
		{
			get { return UpperSectionTotal + UpperSectionBonus; }
		}

		public int? ThreeOfAKind { get; private set; }
		public int? FourOfAKind { get; private set; }
		public int? FullHouse { get; private set; }
		public int? SmallStraight { get; private set; }
		public int? LargeStraight { get; private set; }
		public int? Chance { get; private set; }
		public int? Yahtzee { get; private set; }
		public IEnumerable<int> YahtzeeBonus { get; private set; }

		public int LowerSectionTotal
		{
			get { return (int) (ThreeOfAKind + FourOfAKind + FullHouse + SmallStraight + LargeStraight + Chance + Yahtzee + YahtzeeBonus.Sum()); }
		}

		public int GrandTotal
		{
			get { return UpperSectionTotalWithBonus + LowerSectionTotal; }
		}


		public ScoreSheet(IDiceOfAKindValidator diceOfAKindValidator, IFullHouseValidator fullHouseValidator, IStraightValidator straightValidator)
		{
			_diceOfAKindValidator = diceOfAKindValidator;
			_fullHouseValidator = fullHouseValidator;
			_straightValidator = straightValidator;

			YahtzeeBonus = new List<int>();
		}

		public int? RecordUpperSection(UpperSectionItem upperSection, IDiceCup diceCup)
		{
			int? sum = null;

			switch(upperSection)
			{
				case UpperSectionItem.Ones:
					if(Ones != null) return null;
					sum = SumDiceOfValue(diceCup, 1);
					Ones = sum;
					break;
				case UpperSectionItem.Twos:
					if(Twos != null) return null;
					sum = SumDiceOfValue(diceCup, 2);
					Twos = sum;
					break;
				case UpperSectionItem.Threes:
					if(Threes != null) return null;
					sum = SumDiceOfValue(diceCup, 3);
					Threes = sum;
					break;
				case UpperSectionItem.Fours:
					if(Fours != null) return null;
					sum = SumDiceOfValue(diceCup, 4);
					Fours = sum;
					break;
				case UpperSectionItem.Fives:
					if(Fives != null) return null;
					sum = SumDiceOfValue(diceCup, 5);
					Fives = sum;
					break;
				case UpperSectionItem.Sixes:
					if(Sixes != null) return null;
					sum = SumDiceOfValue(diceCup, 6);
					Sixes = sum;
					break;
			}

			return sum;
		}

		public int? RecordThreeOfAKind(IDiceCup diceCup)
		{
			if(ThreeOfAKind != null) return null;

			ThreeOfAKind = _diceOfAKindValidator.IsValid(3, diceCup.Dice) ? diceCup.Dice.Select(x => x.Value).Sum() : 0;
			return ThreeOfAKind;
		}

		public int? RecordFourOfAKind(IDiceCup diceCup)
		{
			if(FourOfAKind != null) return null;

			FourOfAKind = _diceOfAKindValidator.IsValid(4, diceCup.Dice) ? diceCup.Dice.Select(x => x.Value).Sum() : 0;
			return FourOfAKind;
		}

		public int? RecordFullHouse(IDiceCup diceCup)
		{
			if(FullHouse != null) return null;

			FullHouse = _fullHouseValidator.IsValid(diceCup.Dice) ? 25 : 0;
			return FullHouse;
		}

		public int? RecordSmallStraight(IDiceCup diceCup)
		{
			if(SmallStraight != null) return null;

			SmallStraight = _straightValidator.IsValid(4, diceCup.Dice) ? 30 : 0;
			return SmallStraight;
		}

		public int? RecordLargeStraight(IDiceCup diceCup)
		{
			if(LargeStraight != null) return null;

			LargeStraight = _straightValidator.IsValid(5, diceCup.Dice) ? 40 : 0;
			return LargeStraight;
		}

		public int? RecordChance(IDiceCup diceCup)
		{
			if(Chance != null) return null;

			Chance = diceCup.Dice.Select(x => x.Value).Sum();
			return Chance;
		}

		public int? RecordYahtzee(IDiceCup diceCup)
		{
			var isValidYahtzeeCombination = _diceOfAKindValidator.IsValid(5, diceCup.Dice);

			if(Yahtzee != null && Yahtzee > 0 && isValidYahtzeeCombination)
			{
				if(YahtzeeBonus.Count() == 3) return null;
				RecordYahtzeeBonus();
				return 100;
			}
			if(Yahtzee != null)
			{
				return null;
			}

			Yahtzee = isValidYahtzeeCombination ? 50 : 0;
			return Yahtzee;
		}

		private void RecordYahtzeeBonus()
		{
			var tempList = YahtzeeBonus.ToList();
			tempList.Add(100);
			YahtzeeBonus = tempList;
		}

		private static int SumDiceOfValue(IDiceCup diceCup, int value)
		{
			return diceCup.Dice.Where(x => x.Value == value).Sum(x => x.Value);
		}

		public bool IsUpperSectionComplete
		{
			get
			{
				return Ones.HasValue &&
				       Twos.HasValue &&
				       Threes.HasValue &&
				       Fours.HasValue &&
				       Fives.HasValue &&
				       Sixes.HasValue;
			}
		}

		public bool IsLowerSectionComplete
		{
			get
			{
				return ThreeOfAKind.HasValue &&
				       FourOfAKind.HasValue &&
				       FullHouse.HasValue &&
				       SmallStraight.HasValue &&
				       LargeStraight.HasValue &&
				       Yahtzee.HasValue && Chance.HasValue;
			}
		}

		public bool IsScoreSheetComplete
		{
			get { return IsUpperSectionComplete && IsLowerSectionComplete; }
		}
	}
}