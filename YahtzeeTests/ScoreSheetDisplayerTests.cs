using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YahtzeeTests.Properties;

using ConsoleYahtzee.Framework;
using FluentAssertions;
using Moq;
using Yahtzee.Framework;


namespace YahtzeeTests
{
	[TestClass]
	public class ScoreSheetDisplayerTests
	{

		private IScoreSheetDisplayer _scoreSheetDisplayer;
		private Mock<IScoreSheet> _scoreSheetMock;

		[TestMethod]
		public void DisplayAnEmptyScoreSheetToConsole()
		{
			// Arrange
			var _scoreSheetMock = new Mock<IScoreSheet>();
			var expectedScoreSheetDisplayOutput = Resources.EmptyScoreSheetConsoleOutput;
			string actualScoreSheetDisplayOutput;

			// Act
			using(StringWriter stringWriter = new StringWriter())
			{
				Console.SetOut(stringWriter);

				_scoreSheetDisplayer = new ScoreSheetConsoleDisplayer();
				_scoreSheetDisplayer.Display(_scoreSheetMock.Object);

				actualScoreSheetDisplayOutput = stringWriter.ToString();
			}

			// Assert
			expectedScoreSheetDisplayOutput.Should().Be(actualScoreSheetDisplayOutput);
		}
	}
}
