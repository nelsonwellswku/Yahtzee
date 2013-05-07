using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using FluentAssertions;
using Yahtzee.Framework;
using YahtzeeTests.Support;
using ConsoleYahtzee.Framework;
using System.Text;

namespace YahtzeeTests
{
	[TestClass]
	public class DiceCupConsoleDisplayTests
	{
		private const string _prompt = "Your current dice cup contents:";

		string _actual;
		string _expected;

		Mock<IDiceCup> _diceCupMock;
		TestDieFactory _testDieFactory;

		IDiceCupDisplay _diceCupConsoleDisplay;

		public DiceCupConsoleDisplayTests()
		{
			_testDieFactory = new TestDieFactory();
			_diceCupMock = new Mock<IDiceCup>();
		}

		[TestMethod]
		public void DiceCupConsoleDisplay_DiceCupHasSetOfOnes_DisplayOnConsole()
		{
			// Arrange
			_diceCupMock.Setup(x => x.Dice).Returns(_testDieFactory.CreateDieEnumerable(1, 1, 1, 1, 1));
			_expected = _prompt + "\r\n" + "| 1 | | 1 | | 1 | | 1 | | 1 |";

			//Act
			using (var capturer = new ConsoleCapturer())
			{
				_diceCupConsoleDisplay = new DiceCupConsoleDisplay();
				_diceCupConsoleDisplay.Show(_diceCupMock.Object);
				_actual = capturer.GetStandardOut();
			}

			// Assert
			_actual.Should().Be(_expected);
		}

		[TestMethod]
		public void DiceCupConsoleDisplay_DIceCupHasRandomCollection_DisplayOnConsole()
		{
			// Arrange
			_diceCupMock.Setup(x => x.Dice).Returns(_testDieFactory.CreateDieEnumerable(1, 5, 3, 4, 6));
			_expected = _prompt + "\r\n" + "| 1 | | 5 | | 3 | | 4 | | 6 |";

			// Act
			using (var capturer = new ConsoleCapturer())
			{
				_diceCupConsoleDisplay = new DiceCupConsoleDisplay();
				_diceCupConsoleDisplay.Show(_diceCupMock.Object);
				_actual = capturer.GetStandardOut();
			}

			// Assert
			_actual.Should().Be(_expected);
		}
	}
}
