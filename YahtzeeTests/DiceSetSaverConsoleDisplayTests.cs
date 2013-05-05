using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using FluentAssertions;

using Yahtzee.Framework;
using ConsoleYahtzee.Framework;
using YahtzeeTests.Properties;

namespace YahtzeeTests
{
	[TestClass]
	public class DiceSetSaverConsoleDisplayTests
	{
		private const string _prompt = "Roll again (R) or save score (S)? ";
		private const string _forcedSavePrompt = "You've used three rolls this turn. Press enter to record your score.";

		private string _actual;
		private string _expected;

		private IDiceSetSaverDisplay _diceSetSaverDisplay;
		private Mock<IDiceCup> _diceCupMock;

		public DiceSetSaverConsoleDisplayTests()
		{
			_diceCupMock = new Mock<IDiceCup>();
		}

		[TestMethod]
		public void DiceSetSaverConsoleDisplay_DiceCupCanBeRolledAgain_PromptTheUserToRecordOrSave()
		{
			// Arrange
			_expected = _prompt;
			SetupDiceCupWithUnfinishedTurn();
			
			_diceSetSaverDisplay = new DiceSetSaverConsoleDisplay(_diceCupMock.Object);

			// Act
			var currentConsoleOut = Console.Out;
			_actual = GetConsoleOutput(_diceSetSaverDisplay.RollOrSave);

			// Assert
			_actual.Should().Be(_expected);
		}

		[TestMethod]
		public void DiceSetSaverConsoleDisplay_DiceCupCannotBeRolledAgain_TellTheUserTheyAreGoingToSaveTheirScore()
		{
			// Arrange
			_expected = _forcedSavePrompt;
			SetupDiceCupWithFinishedTurn();

			_diceSetSaverDisplay = new DiceSetSaverConsoleDisplay(_diceCupMock.Object);

			// Act
			_actual = GetConsoleOutput(_diceSetSaverDisplay.RollOrSave);

			// Assert
			_actual.Should().Be(_expected);
		}

		[TestMethod]
		public void DiceSetSaverConsoleDisplay_UserWantsToSave_DisplayPromptWithOptions()
		{
			// Arrange
			_expected = Resources.SaveScoreOptions;
			_diceSetSaverDisplay = new DiceSetSaverConsoleDisplay(_diceCupMock.Object);

			// Act
			_actual = GetConsoleOutput(_diceSetSaverDisplay.SaveScore);

			// Assert
			_actual.Should().Be(_expected);
		}

		private void SetupDiceCupWithUnfinishedTurn()
		{
			_diceCupMock.Setup(x => x.IsFinal()).Returns(false);
		}

		private void SetupDiceCupWithFinishedTurn()
		{
			_diceCupMock.Setup(x => x.IsFinal()).Returns(true);
		}

		private string GetConsoleOutput(Action action)
		{
			string result;
			var currentConsoleOut = Console.Out;
			using (var stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);
				action();
				result = stringWriter.ToString();
				Console.SetOut(currentConsoleOut);
			}

			return result;
		}
	}
}
