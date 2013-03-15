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
		private readonly TestDieFactory _testDieFactory;
		private readonly Mock<IDiceCup> _diceCup;
		private readonly Mock<IDiceCup> _diceCup2;
		private readonly Mock<IDiceOfAKindValidator> _diceOfAKindValidator;
		private readonly Mock<IFullHouseValidator> _fullHouseValidator;
		private readonly Mock<IStraightValidator> _straightValidator;

		public ScoreSheetTests ()
		{
			_testDieFactory = new TestDieFactory();
			_diceCup = new Mock<IDiceCup>();
			_diceCup2 = new Mock<IDiceCup>();
			_diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			_fullHouseValidator = new Mock<IFullHouseValidator>();
			_straightValidator = new Mock<IStraightValidator>();
		}

		#region Three and four of a kind tests

		[TestMethod]
		public void RecordThreeOfAKindWithValidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? threeOfAKindScore = scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			//Assert
			scoreSheet.ThreeOfAKind.Should().Be(20);

		}

		[TestMethod]
		public void RecordThreeOfAKindWithInvalidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(false);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? threeOfAKindScore = scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			//Assert
			threeOfAKindScore.Should().Be(0);
			scoreSheet.ThreeOfAKind.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingThreeOfAKindOnceItsBeenSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 4, 2, 4, 4, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 3, 4 });
			_diceCup2.Setup(x => x.Dice).Returns(dice2);

			_diceOfAKindValidator.Setup(x => x.IsValid(3, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoreSheet.RecordThreeOfAKind(_diceCup.Object);
			var secondRecording = scoreSheet.RecordThreeOfAKind(_diceCup2.Object);

			// Assert
			secondRecording.Should().NotHaveValue();
			scoreSheet.ThreeOfAKind.Should().Be(17);
		}

		[TestMethod]
		public void RecordFourOfAKindWithValidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 3, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? fourOfAKindScore = scoreSheet.RecordFourOfAKind(_diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(17);
			scoreSheet.FourOfAKind.Should().Be(17);

		}

		[TestMethod]
		public void RecordFourOfAKindWithInvalidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(false);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? fourOfAKindScore = scoreSheet.RecordFourOfAKind(_diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(0);
			scoreSheet.FourOfAKind.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingFourOfAKindOnceItsBeenSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 4, 2, 4, 4, 4 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(new[] { 5, 5, 5, 3, 5 });
			_diceCup2.Setup(x => x.Dice).Returns(dice2);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoreSheet.RecordFourOfAKind(_diceCup.Object);
			var secondRecording = scoreSheet.RecordFourOfAKind(_diceCup2.Object);

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
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? fullHouseScore = scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			scoreSheet.FullHouse.Should().Be(25);

		}

		[TestMethod]
		public void RecordFullHouseWithInvalidSet()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? fullHouseScore = scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			fullHouseScore.Should().Be(0);
			scoreSheet.FullHouse.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingFullHouseOnceItsBeenSet()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoreSheet.RecordFullHouse(_diceCup.Object);
			var secondRecord = scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();
			scoreSheet.FullHouse.Should().Be(25);
		}

		#endregion

		#region Straight tests

		[TestMethod]
		public void RecordSmallStraightWithValidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? smallStraightScore = scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			scoreSheet.SmallStraight.Should().Be(30);

		}

		[TestMethod]
		public void RecordSmallStraightWithInvalidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? smallStraightScore = scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			scoreSheet.SmallStraight.Should().Be(0);

		}

		[TestMethod]
		public void DisallowSettingSmallStraightOnceItsBeenSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoreSheet.RecordSmallStraight(_diceCup.Object);
			int? secondRecord = scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();

		}

		[TestMethod]
		public void RecordLargeStraightWithValidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? largeStraightScore = scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			scoreSheet.LargeStraight.Should().Be(40);

		}

		[TestMethod]
		public void RecordLargeStraightWithInvalidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? largeStraightScore = scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			scoreSheet.LargeStraight.Should().Be(0);

		}

		[TestMethod]
		public void DisallowSettingLargeStraightOnceItsBeenSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoreSheet.RecordLargeStraight(_diceCup.Object);
			int? secondRecord = scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();

		}

		#endregion

		#region Chance tests

		[TestMethod]
		public void RecordChance()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 6, 3, 2, 4, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? chanceScore = scoreSheet.RecordChance(_diceCup.Object);

			//Assert
			scoreSheet.Chance.Should().Be(18);

		}

		[TestMethod]
		public void DisallowChanceFromBeingSetOnceItsBeenSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(new[] { 1, 1, 1, 2, 1 });
			_diceCup2.Setup(x => x.Dice).Returns(dice2);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoreSheet.RecordChance(_diceCup.Object);
			int? chanceScore = scoreSheet.RecordChance(_diceCup2.Object);

			//Assert
			chanceScore.Should().NotHaveValue();
			scoreSheet.Chance.Should().Be(19);
		}

		#endregion

		#region Yahtzee tests

		[TestMethod]
		public void RecordYahtzeeWithValidSet()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? yahtzeeScore = scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(50);
			scoreSheet.Yahtzee.Should().Be(50);
		}

		[TestMethod]
		public void RecordYahtzeeWithInvalidSet()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			int? yahtzeeScore = scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(0);
			scoreSheet.Yahtzee.Should().Be(0);
		}

		#endregion
	}
}