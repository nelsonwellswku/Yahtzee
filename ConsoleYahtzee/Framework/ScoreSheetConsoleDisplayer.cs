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
			templateParameters["upperSectionTotal"] = String.Empty; // TODO: Write a method in IScoreSheet to return this value
			templateParameters["bonus"] = String.Empty; // TODO: Same as above
			templateParameters["upperSectionTotalWithBonus"] = String.Empty; // TODO: Same as above

			templateParameters["threeOfAKind"] = scoreSheet.ThreeOfAKind.ToString();
			templateParameters["fourOfAKind"] = scoreSheet.FourOfAKind.ToString();
			templateParameters["fullHouse"] = scoreSheet.FullHouse.ToString();
			templateParameters["smallStraight"] = scoreSheet.SmallStraight.ToString();
			templateParameters["largeStraight"] = scoreSheet.LargeStraight.ToString();
			templateParameters["chance"] = scoreSheet.LargeStraight.ToString();
			templateParameters["yahtzee"] = scoreSheet.Yahtzee.ToString();

			var yahtzeeBonus = new int[3];
			yahtzeeBonus[0] = scoreSheet.YahtzeeBonus.ElementAtOrDefault(0);
			yahtzeeBonus[1] = scoreSheet.YahtzeeBonus.ElementAtOrDefault(1);
			yahtzeeBonus[2] = scoreSheet.YahtzeeBonus.ElementAtOrDefault(2);

			templateParameters["yahtzeeBonus1"] = yahtzeeBonus[0] == 0 ? "  " : yahtzeeBonus[0].ToString();
			templateParameters["yahtzeeBonus2"] = yahtzeeBonus[1] == 0 ? "  " : yahtzeeBonus[1].ToString();
			templateParameters["yahtzeeBonus3"] = yahtzeeBonus[2] == 0 ? "  " : yahtzeeBonus[2].ToString();

			templateParameters["lowerSectionTotal"] = string.Empty; // TODO: Write a method in IScoreSheet to return this value
			templateParameters["grandTotal"] = string.Empty; // TODO: Write a method in IScoreSheet to return this value

			return templateParameters;
		}
	}
}
