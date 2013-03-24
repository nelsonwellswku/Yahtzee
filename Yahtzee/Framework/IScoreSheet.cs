using System.Collections.Generic;

namespace Yahtzee.Framework
{
	public interface IScoreSheet
	{
		int? Ones { get; }
		int? Twos { get; }
		int? Threes { get; }
		int? Fours { get; }
		int? Fives { get; }
		int? Sixes { get; }
		int UpperSectionBonus { get; }
		int UpperSectionTotal { get; }

		int? ThreeOfAKind { get; }
		int? FourOfAKind { get; }
		int? FullHouse { get; }
		int? SmallStraight { get; }
		int? LargeStraight { get; }
		int? Chance { get; }
		int? Yahtzee { get; }
		IEnumerable<int> YahtzeeBonus { get; }

		int? RecordUpperSection(UpperSectionItem upperSection, IDiceCup diceCup);
		int? RecordThreeOfAKind(IDiceCup diceCup);
		int? RecordFourOfAKind(IDiceCup diceCup);
		int? RecordFullHouse(IDiceCup diceCup);
		int? RecordSmallStraight(IDiceCup diceCup);
		int? RecordLargeStraight(IDiceCup diceCup);
		int? RecordChance(IDiceCup diceCup);
		int? RecordYahtzee(IDiceCup diceCup);
	}
}
