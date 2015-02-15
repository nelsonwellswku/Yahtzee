using System;
using System.Collections.Generic;
using Yahtzee.Framework;

namespace Website.HubHelpers
{
	public static class LowerSectionScorer
	{
		public static Dictionary<string, Func<IScoreSheet, IDiceCup, int?>> Score = new Dictionary<string, Func<IScoreSheet, IDiceCup, int?>>
		{
			{"threeofakind", (scoreSheet, diceCup) => scoreSheet.RecordThreeOfAKind(diceCup)},
			{"fourofakind", (scoreSheet, diceCup) => scoreSheet.RecordFourOfAKind(diceCup)},
			{"fullhouse", (scoreSheet, diceCup) => scoreSheet.RecordFullHouse(diceCup)},
			{"smallstraight", (scoreSheet, diceCup) => scoreSheet.RecordSmallStraight(diceCup)},
			{"largestraight", (scoreSheet, diceCup) => scoreSheet.RecordLargeStraight(diceCup)},
			{"yahtzee", (scoreSheet, diceCup) => scoreSheet.RecordYahtzee(diceCup)},
			{"chance", (scoreSheet, diceCup) => scoreSheet.RecordChance(diceCup)}
		};
	}
}