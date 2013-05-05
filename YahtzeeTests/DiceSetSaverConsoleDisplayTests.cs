using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using FluentAssertions;

using Yahtzee.Framework;
using ConsoleYahtzee.Framework;

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
			string actual;
			string expected = _prompt;
			SetupDiceCupWithUnfinishedTurn();
			
			_diceSetSaverDisplay = new DiceSetSaverConsoleDisplay(_diceCupMock.Object);

			// Act
			var currentConsoleOut = Console.Out;
			using (StringWriter stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);
				_diceSetSaverDisplay.RollOrSave();

				actual = stringWriter.ToString();
				Console.SetOut(currentConsoleOut);
			}

			// Assert
			actual.Should().Be(expected);
		}

		[TestMethod]
		public void DiceSetSaverConsoleDisplay_DiceCupCannotBeRolledAgain_TellTheUserTheyAreGoingToSaveTheirScore()
		{
			// Arrange
			_expected = _forcedSavePrompt;
			SetupDiceCupWithFinishedTurn();

			_diceSetSaverDisplay = new DiceSetSaverConsoleDisplay(_diceCupMock.Object);

			// Act
			var currentConsoleOut = Console.Out;
			using (StringWriter stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);
				_diceSetSaverDisplay.RollOrSave();

				_actual = stringWriter.ToString();
				Console.SetOut(currentConsoleOut);
			}

			// Assert
			_actual.Should().Be(_expected);
		}

		/*[TestMethod]
		public void DiceSetSaverConsoleDisplay_UserWantsToSave_DisplayPromptWithOptions()
		{
			
		}*/

		private void SetupDiceCupWithUnfinishedTurn()
		{
			_diceCupMock.Setup(x => x.IsFinal()).Returns(false);
		}

		private void SetupDiceCupWithFinishedTurn()
		{
			_diceCupMock.Setup(x => x.IsFinal()).Returns(true);
		}
	}
}
