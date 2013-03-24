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
		private readonly ScoreSheet _scoreSheet;

		public ScoreSheetTests ()
		{
			_testDieFactory = new TestDieFactory();
			_diceCup = new Mock<IDiceCup>();
			_diceCup2 = new Mock<IDiceCup>();
			_diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			_fullHouseValidator = new Mock<IFullHouseValidator>();
			_straightValidator = new Mock<IStraightValidator>();

			_scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
		}

		#region Upper section

		// TODO: Comprehensive tests for 2, 4, 5, 6
		//			Maybe elaborate on tests for 1 and 3

		[TestMethod]
		public void RecordUpperSectionOnesWithValidValues()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 2, 4, 1, 3, 1 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			var onesScore = _scoreSheet.RecordUpperSection(UpperSectionItem.Ones, _diceCup.Object);

			// Assert
			onesScore.Should().Be(2);
			_scoreSheet.Ones.Should().Be(2);
		}

		[TestMethod]
		public void RecordUpperSectionThreesWithValidValues()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 3, 1, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			var threesScore = _scoreSheet.RecordUpperSection(UpperSectionItem.Threes, _diceCup.Object);

			// Assert
			threesScore.Should().Be(9);
			_scoreSheet.Threes.Should().Be(9);
		}

		[TestMethod]
		public void RecordUpperSectionTwosAfterItsAlreadyBeenRecorded()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 2, 5, 2 });
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			_scoreSheet.RecordUpperSection(UpperSectionItem.Twos, _diceCup.Object);
			var twosScore = _scoreSheet.RecordUpperSection(UpperSectionItem.Twos, _diceCup.Object);

			// Assert
			twosScore.Should().NotHaveValue();
			_scoreSheet.Twos.Should().Be(4);
		}

		[TestMethod]
		public void ScoreSheet_UpperSectionTotal_ReturnsSumOfUpperSectionValues()
		{
			// Arrange
			var onesDice	=	_testDieFactory.CreateDieEnumerable(new[] { 1, 3, 1, 5, 4 });		// 2
			var twosDice	=	_testDieFactory.CreateDieEnumerable(new[] { 1, 5, 2, 2, 2 });		// 6
			var threesDice =	_testDieFactory.CreateDieEnumerable(new[] { 3, 4, 3 , 3, 3 });		// 12
			var foursDice	=	_testDieFactory.CreateDieEnumerable(new[] { 4, 1, 4, 2, 2 });		// 8
			var fivesDice	=	_testDieFactory.CreateDieEnumerable(new[] { 1, 4, 3, 6, 6 });		// 0
			var sixesDice	=	_testDieFactory.CreateDieEnumerable(new[] { 1, 3, 6, 4, 3 });		// 6

			_diceCup.Setup(x => x.Dice).ReturnsInOrder(onesDice, twosDice, threesDice, foursDice, fivesDice, sixesDice);
			var diceCup = _diceCup.Object;

			_scoreSheet.RecordUpperSection(UpperSectionItem.Ones, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Twos, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Threes, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Fours, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Fives, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Sixes, diceCup);

			// Act
			// No act since UpperSectionTotal is depend on the state of the ScoreSheet

			// Assert
			_scoreSheet.UpperSectionTotal.Should().Be(34);
		}

		[TestMethod]
		public void ScoreSheet_UpperSectionTotalsEnoughForBonus_ReturnsBonusOf35Points()
		{
			// Arrange


			// Act
			throw new NotImplementedException();

			// Assert
		}

		[TestMethod]
		public void ScoreSheet_UpperSectionDoesNotTotalEnoughForBonus_ReturnsBonusOf0Points()
		{
			// Arrange


			// Act
			throw new NotImplementedException();

			// Assert
		}

		[TestMethod]
		public void ScoreSheet_UpperSectionTotalWithBonusAndBonusExists_ReturnsUpperSectionScoresPlus35BonusPoints()
		{
			// Arrange


			// Act
			throw new NotImplementedException();

			// Assert
		}

		[TestMethod]
		public void ScoreSheet_UpperSectionTotalWithBonusAndBonusDoesntExist_ReturnsUpperSectionScoresOnly()
		{
			// Arrange


			// Act
			throw new NotImplementedException();

			// Assert
		}

		#endregion

		//Lower Section

		#region Three and four of a kind tests

		[TestMethod]
		public void RecordThreeOfAKindWithValidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(true);

			// Act
			int? threeOfAKindScore = _scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			//Assert
			_scoreSheet.ThreeOfAKind.Should().Be(20);

		}

		[TestMethod]
		public void RecordThreeOfAKindWithInvalidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(false);

			// Act
			
			int? threeOfAKindScore = _scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			//Assert
			threeOfAKindScore.Should().Be(0);
			_scoreSheet.ThreeOfAKind.Should().Be(0);
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
			_scoreSheet.RecordThreeOfAKind(_diceCup.Object);
			var secondRecording = _scoreSheet.RecordThreeOfAKind(_diceCup2.Object);

			// Assert
			secondRecording.Should().NotHaveValue();
			_scoreSheet.ThreeOfAKind.Should().Be(17);
		}

		[TestMethod]
		public void RecordFourOfAKindWithValidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 5, 3, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(true);

			// Act
			int? fourOfAKindScore = _scoreSheet.RecordFourOfAKind(_diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(17);
			_scoreSheet.FourOfAKind.Should().Be(17);

		}

		[TestMethod]
		public void RecordFourOfAKindWithInvalidSet()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 5, 6, 3 });
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(false);

			// Act
			int? fourOfAKindScore = _scoreSheet.RecordFourOfAKind(_diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(0);
			_scoreSheet.FourOfAKind.Should().Be(0);
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
			_scoreSheet.RecordFourOfAKind(_diceCup.Object);
			var secondRecording = _scoreSheet.RecordFourOfAKind(_diceCup2.Object);

			// Assert
			secondRecording.Should().NotHaveValue();
			_scoreSheet.FourOfAKind.Should().Be(18);
		}

		#endregion

		#region Full house tests

		[TestMethod]
		public void RecordFullHouseWithValidSet()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			int? fullHouseScore = _scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			_scoreSheet.FullHouse.Should().Be(25);

		}

		[TestMethod]
		public void RecordFullHouseWithInvalidSet()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			int? fullHouseScore = _scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			fullHouseScore.Should().Be(0);
			_scoreSheet.FullHouse.Should().Be(0);
		}

		[TestMethod]
		public void DisallowSettingFullHouseOnceItsBeenSet()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordFullHouse(_diceCup.Object);
			var secondRecord = _scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();
			_scoreSheet.FullHouse.Should().Be(25);
		}

		#endregion

		#region Straight tests

		[TestMethod]
		public void RecordSmallStraightWithValidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			int? smallStraightScore = _scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			_scoreSheet.SmallStraight.Should().Be(30);

		}

		[TestMethod]
		public void RecordSmallStraightWithInvalidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			int? smallStraightScore = _scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			_scoreSheet.SmallStraight.Should().Be(0);

		}

		[TestMethod]
		public void DisallowSettingSmallStraightOnceItsBeenSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordSmallStraight(_diceCup.Object);
			int? secondRecord = _scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();

		}

		[TestMethod]
		public void RecordLargeStraightWithValidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			int? largeStraightScore = _scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			_scoreSheet.LargeStraight.Should().Be(40);

		}

		[TestMethod]
		public void RecordLargeStraightWithInvalidSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			int? largeStraightScore = _scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			_scoreSheet.LargeStraight.Should().Be(0);

		}

		[TestMethod]
		public void DisallowSettingLargeStraightOnceItsBeenSet()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordLargeStraight(_diceCup.Object);
			int? secondRecord = _scoreSheet.RecordLargeStraight(_diceCup.Object);

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
			int? chanceScore = _scoreSheet.RecordChance(_diceCup.Object);

			//Assert
			_scoreSheet.Chance.Should().Be(18);

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
			_scoreSheet.RecordChance(_diceCup.Object);
			int? chanceScore = _scoreSheet.RecordChance(_diceCup2.Object);

			//Assert
			chanceScore.Should().NotHaveValue();
			_scoreSheet.Chance.Should().Be(19);
		}

		#endregion

		#region Yahtzee tests

		[TestMethod]
		public void RecordYahtzeeWithValidSet()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			int? yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(50);
			_scoreSheet.Yahtzee.Should().Be(50);
		}

		[TestMethod]
		public void RecordYahtzeeWithInvalidSet()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			int? yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(0);
			_scoreSheet.Yahtzee.Should().Be(0);
		}

		[TestMethod]
		public void RecordYahtzeeBonusAfterYahtzeeWithValidSet()
		{
			// Arrange
			var expectedArray = new int?[] { 100 };
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordYahtzee(_diceCup.Object);
			var yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			yahtzeeScore.Should().Be(100);
			_scoreSheet.YahtzeeBonus.Should().BeEquivalentTo(expectedArray);
		}

		[TestMethod]
		public void RecordYahtzeeBonusAfterFailedYahtzeeRecord()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).ReturnsInOrder(false, true);

			// Act
			var scoresheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
			scoresheet.RecordYahtzee(_diceCup.Object);
			var yahtzeeScore = scoresheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(null);
			scoresheet.Yahtzee.Should().Be(0);
			scoresheet.YahtzeeBonus.ShouldAllBeEquivalentTo(new int[0]);
		}

		[TestMethod]
		public void RecordYahtzeeBonusOnlyThreeTimes()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).ReturnsInOrder(true, true, true, true, true);

			// Act
			_scoreSheet.RecordYahtzee(_diceCup.Object);
			_scoreSheet.RecordYahtzee(_diceCup.Object);
			_scoreSheet.RecordYahtzee(_diceCup.Object);
			_scoreSheet.RecordYahtzee(_diceCup.Object);
			var yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().NotHaveValue();
		}

		/* TODO: There is a "joker" rule that I didn't know about such that:
		 * * If a player rolls a Yahtzee AND the upper-section of the corresponding die value 
		 * * has been filled, then one may elect to use the Yahtzee as a wild card for a lower
		 * * section value such as full-house or a straight.
		 * 
		 * I'm not certain how the API should work for this behavior so I'll leave it for later */

		#endregion

		#region Total tests
		[TestMethod]
		public void ScoreSheet_LowerSectionTotal_ReturnsSumOfAllLowerSectionItems()
		{
			// Arrange


			// Act
			throw new NotImplementedException();

			// Assert
		}

		[TestMethod]
		public void ScoreSheet_GrandTotal_ReturnsSumOfUpperSectionTotalWithBonusAndLowerSectionTotal()
		{
			// Arrange


			// Act
			throw new NotImplementedException();

			// Assert
      }
		#endregion
	}
}