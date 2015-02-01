using System;
using System.IO;
using System.Collections.Generic;
using YahtzeeTests.Properties;

using ConsoleYahtzee.Framework;
using FluentAssertions;
using Moq;
using Yahtzee.Framework;
using NUnit.Framework;


namespace YahtzeeTests
{

	public class ScoreSheetConsoleDisplayerTests
	{

		private IScoreSheetDisplayer _scoreSheetDisplayer;
		private Mock<IScoreSheet> _scoreSheetMock;

		[Test]
		public void ScoreSheetConsoleDisplayer_ScoreSheetIsEmpty_WriteEmptyScoreSheetToStandardOut()
		{
			// Arrange
			_scoreSheetMock = new Mock<IScoreSheet>();
			var expectedScoreSheetDisplayOutput = Resources.EmptyScoreSheetConsoleOutput;
			string actualScoreSheetDisplayOutput;

			// Act
			using (StringWriter stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);

				_scoreSheetDisplayer = new ScoreSheetConsoleDisplayer();
				_scoreSheetDisplayer.Display(_scoreSheetMock.Object);

				actualScoreSheetDisplayOutput = stringWriter.ToString();
			}

			// Assert
			expectedScoreSheetDisplayOutput.Should().Be(actualScoreSheetDisplayOutput);
		}

		[Test]
		public void ScoreSheetConsoleDisplayer_ScoreSheetIsComplete_WriteCompletedScoreSheetToStandardOut()
		{
			// Arrange
			_scoreSheetMock = new Mock<IScoreSheet>();
			SetUpCompleteScoreSheet();
			var expectedScoreSheetDisplayOutput = Resources.CompletedScoreSheetConsoleOutput;
			string actualScoreSheetDisplayOutput;

			// Act
			using (StringWriter stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);

				_scoreSheetDisplayer = new ScoreSheetConsoleDisplayer();
				_scoreSheetDisplayer.Display(_scoreSheetMock.Object);

				actualScoreSheetDisplayOutput = stringWriter.ToString();
			}

			// Assert
			expectedScoreSheetDisplayOutput.Should().Be(actualScoreSheetDisplayOutput);
		}

		[Test]
		public void ScoreSheetConsoleDisplayer_ScoreSheetIsPartiallyComplete_WritePartiallyCompletedScoreSheetToStandardOut()
		{
			// Arrange
			_scoreSheetMock = new Mock<IScoreSheet>();
			SetUpPartiallyCompleteScoreSheet();
			var expectedScoreSheetDisplayOutput = Resources.PartiallyCompletedScoreSheetConsoleOutput;
			string actualScoreSheetDisplayOutput;

			// Act
			using (StringWriter stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);

				_scoreSheetDisplayer = new ScoreSheetConsoleDisplayer();
				_scoreSheetDisplayer.Display(_scoreSheetMock.Object);

				actualScoreSheetDisplayOutput = stringWriter.ToString();
			}

			// Assert
			actualScoreSheetDisplayOutput.Should().Be(expectedScoreSheetDisplayOutput);
		}

		private void SetUpPartiallyCompleteScoreSheet()
		{
			_scoreSheetMock.Setup(x => x.Ones).Returns(3);
			_scoreSheetMock.Setup(x => x.Twos).Returns(6);
			_scoreSheetMock.Setup(x => x.Threes).Returns((int?)null);
			_scoreSheetMock.Setup(x => x.Fours).Returns(0);
			_scoreSheetMock.Setup(x => x.Fives).Returns(15);
			_scoreSheetMock.Setup(x => x.Sixes).Returns((int?)null);
			_scoreSheetMock.Setup(x => x.UpperSectionTotal).Returns(24);
			_scoreSheetMock.Setup(x => x.UpperSectionBonus).Returns(0);
			_scoreSheetMock.Setup(x => x.UpperSectionTotalWithBonus).Returns(24);

			_scoreSheetMock.Setup(x => x.ThreeOfAKind).Returns(17);
			_scoreSheetMock.Setup(x => x.FourOfAKind).Returns((int?)null);
			_scoreSheetMock.Setup(x => x.FullHouse).Returns(25);
			_scoreSheetMock.Setup(x => x.SmallStraight).Returns((int?)null);
			_scoreSheetMock.Setup(x => x.LargeStraight).Returns(40);
			_scoreSheetMock.Setup(x => x.Yahtzee).Returns(50);
			_scoreSheetMock.Setup(x => x.YahtzeeBonus).Returns(new List<int>());
			_scoreSheetMock.Setup(x => x.Chance).Returns((int?)null);
			_scoreSheetMock.Setup(x => x.LowerSectionTotal).Returns(132);
			_scoreSheetMock.Setup(x => x.GrandTotal).Returns(156);
		}

		private void SetUpCompleteScoreSheet()
		{
			_scoreSheetMock.Setup(x => x.Ones).Returns(3);
			_scoreSheetMock.Setup(x => x.Twos).Returns(6);
			_scoreSheetMock.Setup(x => x.Threes).Returns(9);
			_scoreSheetMock.Setup(x => x.Fours).Returns(12);
			_scoreSheetMock.Setup(x => x.Fives).Returns(15);
			_scoreSheetMock.Setup(x => x.Sixes).Returns(18);
			_scoreSheetMock.Setup(x => x.UpperSectionTotal).Returns(63);
			_scoreSheetMock.Setup(x => x.UpperSectionBonus).Returns(35);
			_scoreSheetMock.Setup(x => x.UpperSectionTotalWithBonus).Returns(98);

			_scoreSheetMock.Setup(x => x.ThreeOfAKind).Returns(17);
			_scoreSheetMock.Setup(x => x.FourOfAKind).Returns(26);
			_scoreSheetMock.Setup(x => x.FullHouse).Returns(25);
			_scoreSheetMock.Setup(x => x.SmallStraight).Returns(30);
			_scoreSheetMock.Setup(x => x.LargeStraight).Returns(40);
			_scoreSheetMock.Setup(x => x.Yahtzee).Returns(50);
			_scoreSheetMock.Setup(x => x.YahtzeeBonus).Returns(new[] { 100 });
			_scoreSheetMock.Setup(x => x.Chance).Returns(22);
			_scoreSheetMock.Setup(x => x.LowerSectionTotal).Returns(310);
			_scoreSheetMock.Setup(x => x.GrandTotal).Returns(408);
		}
	}
}
