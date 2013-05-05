using ConsoleYahtzee.Properties;
using System;
using Yahtzee.Framework;

namespace ConsoleYahtzee.Framework
{
	public class DiceSetSaverConsoleDisplay : IDiceSetSaverDisplay
	{
		private const string _prompt = "Roll again (R) or save score (S)? ";
		private const string _forcedPrompt = "You've used three rolls this turn. Press enter to record your score.";
		private readonly IDiceCup _diceCup;

		public DiceSetSaverConsoleDisplay(IDiceCup diceCup)
		{
			_diceCup = diceCup;
		}

		public void RollOrSave()
		{
			if (_diceCup.IsFinal())
			{
				Console.Write(_forcedPrompt);
			}
			else
			{
				Console.Write(_prompt);
			}
		}

		public void SaveScore()
		{
			Console.Write(Resources.SaveScoreOptions);
		}
	}
}
