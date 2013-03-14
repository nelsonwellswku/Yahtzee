using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Moq;
using Yahtzee.Framework;
using Yahtzee.Framework.DiceCombinationValidators;
using YahtzeeTests.Support;

namespace YahtzeeTests
{
	[TestClass]
	public class ScoreSheetTests
	{
		private TestDieFactory _testDieFactory = new TestDieFactory();

		#region Three and four of a kind tests

		[TestMethod]
		public void RecordThreeOfAKindWithValidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 6, 3 });
			var diceCup = new Mock<IDiceCup>();
			diceCup.Setup(x => x.Dice).Returns(dice);

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(true);
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			int? threeOfAKindScore = scoreSheet.RecordThreeOfAKind(diceCup.Object);

			//Assert
			scoreSheet.ThreeOfAKind.Should().Be(20);

		}

		[TestMethod]
		public void RecordThreeOfAKindWithInvalidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			var diceCup = new Mock<IDiceCup>();
			diceCup.Setup(x => x.Dice).Returns(dice);

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(false);
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			int? threeOfAKindScore = scoreSheet.RecordThreeOfAKind(diceCup.Object);

			//Assert
			threeOfAKindScore.Should().Be(0);
			scoreSheet.ThreeOfAKind.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingThreeOfAKindOnceItsBeenSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 4, 2, 4, 4, 3 });
			var diceCup = new Mock<IDiceCup>();
			diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 3, 4 });
			var diceCup2 = new Mock<IDiceCup>();
			diceCup2.Setup(x => x.Dice).Returns(dice2);

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			diceOfAKindValidator.Setup(x => x.IsValid(3, It.IsAny<IEnumerable<IDie>>())).Returns(true);
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			scoreSheet.RecordThreeOfAKind(diceCup.Object);
			var secondRecording = scoreSheet.RecordThreeOfAKind(diceCup2.Object);

			// Assert
			secondRecording.Should().NotHaveValue();
			scoreSheet.ThreeOfAKind.Should().Be(17);
		}

		[TestMethod]
		public void RecordFourOfAKindWithValidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 3, 3 });
			var diceCup = new Mock<IDiceCup>();
			diceCup.Setup(x => x.Dice).Returns(dice);

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(true);
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			int? fourOfAKindScore = scoreSheet.RecordFourOfAKind(diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(17);
			scoreSheet.FourOfAKind.Should().Be(17);

		}

		[TestMethod]
		public void RecordFourOfAKindWithInvalidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			var diceCup = new Mock<IDiceCup>();
			diceCup.Setup(x => x.Dice).Returns(dice);

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(false);
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			int? fourOfAKindScore = scoreSheet.RecordFourOfAKind(diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(0);
			scoreSheet.FourOfAKind.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingFourOfAKindOnceItsBeenSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 4, 2, 4, 4, 4 });
			var diceCup = new Mock<IDiceCup>();
			diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(new[] { 5, 5, 5, 3, 5 });
			var diceCup2 = new Mock<IDiceCup>();
			diceCup2.Setup(x => x.Dice).Returns(dice2);

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			diceOfAKindValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			scoreSheet.RecordFourOfAKind(diceCup.Object);
			var secondRecording = scoreSheet.RecordFourOfAKind(diceCup2.Object);

			// Assert
			secondRecording.Should().NotHaveValue();
			scoreSheet.FourOfAKind.Should().Be(18);
		}

		#endregion

		#region Full house tests

		[TestMethod]
		public void RecordFullHouseWithValidSet()
		{
			// Arrange
			var diceCup = new Mock<IDiceCup>();

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			int? fullHouseScore = scoreSheet.RecordFullHouse(diceCup.Object);

			//Assert
			scoreSheet.FullHouse.Should().Be(25);

		}

		[TestMethod]
		public void RecordFullHouseWithInvalidSet()
		{
			// Arrange
			var diceCup = new Mock<IDiceCup>();

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(false);
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			int? fullHouseScore = scoreSheet.RecordFullHouse(diceCup.Object);

			//Assert
			fullHouseScore.Should().Be(0);
			scoreSheet.FullHouse.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingFullHouseOnceItsBeenSet()
		{
			// Arrange
			var diceCup = new Mock<IDiceCup>();

			var diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			var fullHouseValidator = new Mock<IFullHouseValidator>();
			fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);
			var straightValidator = new Mock<IStraightValidator>();

			// Act
			var scoreSheet = new ScoreSheet(diceOfAKindValidator.Object, fullHouseValidator.Object, straightValidator.Object);
			scoreSheet.RecordFullHouse(diceCup.Object);
			var secondRecord = scoreSheet.RecordFullHouse(diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();
			scoreSheet.FullHouse.Should().Be(25);
		}

		#endregion
	}
}