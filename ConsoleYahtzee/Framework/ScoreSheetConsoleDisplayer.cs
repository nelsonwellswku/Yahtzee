using ConsoleYahtzee.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Yahtzee.Framework;

namespace ConsoleYahtzee.Framework
{
	public class ScoreSheetConsoleDisplayer : IScoreSheetDisplayer
	{
		private readonly string _template;

		public ScoreSheetConsoleDisplayer()
		{
			// TODO: Does this break dependency inversion?  Should the template be passed into the constructor?
			_template = Resources.ScoreSheetTemplate;
		}

		public void Display(IScoreSheet scoreSheet)
		{
			Dictionary<string, string> templateParameters = BuildTemplateParametersDictionary(scoreSheet);
			string output = Regex.Replace(_template, @"\{(.+?)\}", m => templateParameters[m.Groups[1].Value]);
			Console.WriteLine(output);
		}

		private Dictionary<string, string> BuildTemplateParametersDictionary(IScoreSheet scoreSheet)
		{
			var templateParameters = new Dictionary<string, string>();

			templateParameters["ones"] = scoreSheet.Ones.ToString();
			templateParameters["twos"] = scoreSheet.Twos.ToString();
			templateParameters["threes"] = scoreSheet.Threes.ToString();
			templateParameters["fours"] = scoreSheet.Fours.ToString();
			templateParameters["fives"] = scoreSheet.Fives.ToString();
			templateParameters["sixes"] = scoreSheet.Sixes.ToString();
			templateParameters["upperSectionTotal"] = IsUpperSectionComplete(scoreSheet) ? scoreSheet.UpperSectionTotal.ToString() : string.Empty;
			templateParameters["bonus"] = IsUpperSectionComplete(scoreSheet) ? scoreSheet.UpperSectionBonus.ToString() : string.Empty;
			templateParameters["upperSectionTotalWithBonus"] = IsUpperSectionComplete(scoreSheet) ? scoreSheet.UpperSectionTotalWithBonus.ToString() : string.Empty;

			templateParameters["threeOfAKind"] = scoreSheet.ThreeOfAKind.ToString();
			templateParameters["fourOfAKind"] = scoreSheet.FourOfAKind.ToString();
			templateParameters["fullHouse"] = scoreSheet.FullHouse.ToString();
			templateParameters["smallStraight"] = scoreSheet.SmallStraight.ToString();
			templateParameters["largeStraight"] = scoreSheet.LargeStraight.ToString();
			templateParameters["chance"] = scoreSheet.Chance.ToString();
			templateParameters["yahtzee"] = scoreSheet.Yahtzee.ToString();

			var yahtzeeBonus = new int[3];
			yahtzeeBonus[0] = scoreSheet.YahtzeeBonus.ElementAtOrDefault(0);
			yahtzeeBonus[1] = scoreSheet.YahtzeeBonus.ElementAtOrDefault(1);
			yahtzeeBonus[2] = scoreSheet.YahtzeeBonus.ElementAtOrDefault(2);

			templateParameters["yahtzeeBonus1"] = yahtzeeBonus[0] == 0 ? " " : "X";
			templateParameters["yahtzeeBonus2"] = yahtzeeBonus[1] == 0 ? " " : "X";
			templateParameters["yahtzeeBonus3"] = yahtzeeBonus[2] == 0 ? " " : "X";

			templateParameters["lowerSectionTotal"] = IsLowerSectionComplete(scoreSheet) ? scoreSheet.LowerSectionTotal.ToString() : string.Empty;
			templateParameters["grandTotal"] = IsSheetComplete(scoreSheet) ? scoreSheet.GrandTotal.ToString() : string.Empty;

			return templateParameters;
		}

		private bool IsUpperSectionComplete(IScoreSheet scoreSheet)
		{
			if (scoreSheet.Ones == null || scoreSheet.Twos == null || scoreSheet.Threes == null ||
				scoreSheet.Fours == null || scoreSheet.Fives == null || scoreSheet.Sixes == null)
			{
				return false;
			}

			return true;
		}

		private bool IsLowerSectionComplete(IScoreSheet scoreSheet)
		{
			if (scoreSheet.ThreeOfAKind == null || scoreSheet.FourOfAKind == null || scoreSheet.FullHouse == null ||
				scoreSheet.SmallStraight == null || scoreSheet.LargeStraight == null || scoreSheet.Chance == null || scoreSheet.Yahtzee == null)
			{
				return false;
			}

			return true;
		}

		private bool IsSheetComplete(IScoreSheet scoreSheet)
		{
			return IsUpperSectionComplete(scoreSheet) && IsLowerSectionComplete(scoreSheet);
		}
	}
}
