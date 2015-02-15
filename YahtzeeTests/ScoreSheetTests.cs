using System.Collections.Generic;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Yahtzee.Framework;
using Yahtzee.Framework.DiceCombinationValidators;
using YahtzeeTests.Support;

namespace YahtzeeTests
{
	public class ScoreSheetTests
	{
		private TestDieFactory _testDieFactory;
		private Mock<IDiceCup> _diceCup;
		private Mock<IDiceCup> _diceCup2;
		private Mock<IDiceOfAKindValidator> _diceOfAKindValidator;
		private Mock<IFullHouseValidator> _fullHouseValidator;
		private Mock<IStraightValidator> _straightValidator;
		private ScoreSheet _scoreSheet;

		[SetUp]
		public void SetUp()
		{
			_testDieFactory = new TestDieFactory();
			_diceCup = new Mock<IDiceCup>();
			_diceCup2 = new Mock<IDiceCup>();
			_diceOfAKindValidator = new Mock<IDiceOfAKindValidator>();
			_fullHouseValidator = new Mock<IFullHouseValidator>();
			_straightValidator = new Mock<IStraightValidator>();

			_scoreSheet = new ScoreSheet(_diceOfAKindValidator.Object, _fullHouseValidator.Object, _straightValidator.Object);
		}

		[Test]
		public void IsUpperSectionComplete_Yes()
		{
			SetupNoBonusDiceCupMock();
			SetupLowerSectionMock();

			_scoreSheet.RecordUpperSection(UpperSectionItem.Ones, _diceCup.Object);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Twos, _diceCup.Object);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Threes, _diceCup.Object);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Fours, _diceCup.Object);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Fives, _diceCup.Object);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Sixes, _diceCup.Object);

			_scoreSheet.IsUpperSectionComplete.Should().Be(true);
		}

		[Test]
		public void IsUpperSectionComplete_No()
		{
			SetupNoBonusDiceCupMock();
			SetupLowerSectionMock();

			_scoreSheet.RecordUpperSection(UpperSectionItem.Ones, _diceCup.Object);

			_scoreSheet.IsUpperSectionComplete.Should().Be(false);
		}

		[Test]
		public void IsLowerSectionComplete_Yes()
		{
			SetupNoBonusDiceCupMock();
			SetupLowerSectionMock();

			_scoreSheet.RecordThreeOfAKind(_diceCup.Object);
			_scoreSheet.RecordFourOfAKind(_diceCup.Object);
			_scoreSheet.RecordSmallStraight(_diceCup.Object);
			_scoreSheet.RecordLargeStraight(_diceCup.Object);
			_scoreSheet.RecordFullHouse(_diceCup.Object);
			_scoreSheet.RecordChance(_diceCup.Object);
			_scoreSheet.RecordYahtzee(_diceCup.Object);

			_scoreSheet.IsLowerSectionComplete.Should().Be(true);
		}

		[Test]
		public void IsLowerSectionComplete_No()
		{
			SetupNoBonusDiceCupMock();
			SetupLowerSectionMock();

			_scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			_scoreSheet.IsLowerSectionComplete.Should().Be(false);
		}

		[Test]
		public void IsScoreSheetComplete_Yes()
		{
			IsUpperSectionComplete_Yes();
			IsLowerSectionComplete_Yes();

			_scoreSheet.IsScoreSheetComplete.Should().BeTrue();
		}

		[Test]
		public void IsScoreSheetComplete_No()
		{
			IsUpperSectionComplete_Yes();
			IsLowerSectionComplete_No();

			_scoreSheet.IsScoreSheetComplete.Should().BeFalse();
		}

		#region Upper section

		// TODO: Comprehensive tests for 2, 4, 5, 6
		// Maybe elaborate on tests for 1 and 3

		[Test]
		public void ScoreSheet_RecordUpperSectionOnes_RecordTwo()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(2, 4, 1, 3, 1);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			var onesScore = _scoreSheet.RecordUpperSection(UpperSectionItem.Ones, _diceCup.Object);

			// Assert
			onesScore.Should().Be(2);
			_scoreSheet.Ones.Should().Be(2);
		}

		[Test]
		public void ScoreSheet_RecordUpperSectionThrees_RecordNine()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(3, 4, 3, 1, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			var threesScore = _scoreSheet.RecordUpperSection(UpperSectionItem.Threes, _diceCup.Object);

			// Assert
			threesScore.Should().Be(9);
			_scoreSheet.Threes.Should().Be(9);
		}

		[Test]
		public void ScoreSheet_RecordUpperSectionTwosAfterItsAlreadyBeenRecorded_DoNotOverwritePreviousValue()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(1, 3, 2, 5, 2);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			_scoreSheet.RecordUpperSection(UpperSectionItem.Twos, _diceCup.Object);
			var twosScore = _scoreSheet.RecordUpperSection(UpperSectionItem.Twos, _diceCup.Object);

			// Assert
			twosScore.Should().NotHaveValue();
			_scoreSheet.Twos.Should().Be(4);
		}

		[Test]
		public void ScoreSheet_UpperSectionNoBonus_ReturnsSumOfUpperSectionValuesAsThirtyFour()
		{
			// Arrange
			SetupNoBonusDiceCupMock();
			RecordAllUpperSection(_diceCup.Object);

			// Act
			var total = _scoreSheet.UpperSectionTotal;

			// Assert
			total.Should().Be(34);
		}

		[Test]
		public void ScoreSheet_UpperSectionTotalsEnoughForBonus_ReturnsBonusOf35Points()
		{
			// Arrange
			SetupBonusDiceCupMock();
			RecordAllUpperSection(_diceCup.Object);

			// Act
			var bonus = _scoreSheet.UpperSectionBonus;

			// Assert
			bonus.Should().Be(35);
		}

		[Test]
		public void ScoreSheet_UpperSectionTotalsExactlyEnoughForBonus_ReturnsBonusOf35Points()
		{
			// Arrange
			SetupBonusDiceCupMock_63Points();
			RecordAllUpperSection(_diceCup.Object);

			// Act
			var bonus = _scoreSheet.UpperSectionBonus;

			// Assert
			bonus.Should().Be(35);
		}

		[Test]
		public void ScoreSheet_UpperSectionDoesNotTotalEnoughForBonus_ReturnsBonusOf0Points()
		{
			// Arrange
			SetupNoBonusDiceCupMock();
			RecordAllUpperSection(_diceCup.Object);

			// Act
			var bonus = _scoreSheet.UpperSectionBonus;

			// Assert
			bonus.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_UpperSectionTotalWithBonusAndBonusExists_ReturnsUpperSectionScoresPlus35BonusPoints()
		{
			// Arrange
			SetupBonusDiceCupMock();
			RecordAllUpperSection(_diceCup.Object);

			// Act
			var totalWithBonus = _scoreSheet.UpperSectionTotalWithBonus;

			// Assert
			totalWithBonus.Should().Be(101);
		}

		[Test]
		public void ScoreSheet_UpperSectionTotalWithBonusAndBonusDoesntExist_ReturnsUpperSectionScoresOnly()
		{
			// Arrange
			SetupNoBonusDiceCupMock();
			RecordAllUpperSection(_diceCup.Object);

			// Act
			var totalWithBonus = _scoreSheet.UpperSectionTotalWithBonus;

			// Assert
			totalWithBonus.Should().Be(34);
		}

		#endregion

		//Lower Section

		#region Three and four of a kind tests

		[Test]
		public void ScoreSheet_ThreeOfAKindUnset_RecordThreeOfAKindWithValidSetAsTwenty()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(3, 3, 5, 6, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(true);

			// Act
			var threeOfAKindScore = _scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			//Assert
			_scoreSheet.ThreeOfAKind.Should().Be(20);
			threeOfAKindScore.Should().Be(20);
		}

		[Test]
		public void ScoreSheet_ThreeOfAKindUnset_RecordThreeOfAKindWithInvalidSetAsZero()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(3, 2, 5, 6, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(3, dice)).Returns(false);

			// Act

			var threeOfAKindScore = _scoreSheet.RecordThreeOfAKind(_diceCup.Object);

			//Assert
			threeOfAKindScore.Should().Be(0);
			_scoreSheet.ThreeOfAKind.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_ThreeOfAKindAlreadySet_DoNotOverwritePreviousValue()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(4, 2, 4, 4, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(3, 3, 5, 3, 4);
			_diceCup2.Setup(x => x.Dice).Returns(dice2);

			_diceOfAKindValidator.Setup(x => x.IsValid(3, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordThreeOfAKind(_diceCup.Object);
			var secondRecording = _scoreSheet.RecordThreeOfAKind(_diceCup2.Object);

			// Assert
			secondRecording.Should().NotHaveValue();
			_scoreSheet.ThreeOfAKind.Should().Be(17);
		}

		[Test]
		public void ScoreSheet_FourOfAKindUnset_RecordFourOfAKindWithValidSetAsSeventeen()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(3, 3, 5, 3, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(true);

			// Act
			var fourOfAKindScore = _scoreSheet.RecordFourOfAKind(_diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(17);
			_scoreSheet.FourOfAKind.Should().Be(17);
		}

		[Test]
		public void ScoreSheet_FourOfAKindUnset_RecordFourOfAKindWithInvalidSetAsZero()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(3, 2, 5, 6, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);
			_diceOfAKindValidator.Setup(x => x.IsValid(4, dice)).Returns(false);

			// Act
			var fourOfAKindScore = _scoreSheet.RecordFourOfAKind(_diceCup.Object);

			//Assert
			fourOfAKindScore.Should().Be(0);
			_scoreSheet.FourOfAKind.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_FourOfAKindAlreadySet_DoNotOverwritePreviousValue()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(4, 2, 4, 4, 4);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(5, 5, 5, 3, 5);
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

		[Test]
		public void ScoreSheet_FullHouseUnset_RecordFullHouseWithValidSetAsTwentyFive()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var fullHouseScore = _scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			_scoreSheet.FullHouse.Should().Be(25);
			fullHouseScore.Should().Be(25);
		}

		[Test]
		public void ScoreSheet_FullHouseUnset_RecordFullHouseWithInvalidSetAsZero()
		{
			// Arrange
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var fullHouseScore = _scoreSheet.RecordFullHouse(_diceCup.Object);

			//Assert
			fullHouseScore.Should().Be(0);
			_scoreSheet.FullHouse.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_FullHouseAlreadySet_DoNotOverwritePreviousValue()
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

		[Test]
		public void ScoreSheet_SmallStraightUnset_RecordSmallStraightWithValidSetAsThirty()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var smallStraightScore = _scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			_scoreSheet.SmallStraight.Should().Be(30);
			smallStraightScore.Should().Be(30);
		}

		[Test]
		public void ScoreSheet_SmallStraightUnset_RecordSmallStraightWithInvalidSetAsZero()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var smallStraightScore = _scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			_scoreSheet.SmallStraight.Should().Be(0);
			smallStraightScore.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_SmallStraightAlreadySet_DoNotOverwritePreviousValue()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(4, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordSmallStraight(_diceCup.Object);
			var secondRecord = _scoreSheet.RecordSmallStraight(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();
		}

		[Test]
		public void ScoreSheet_LargeStraightUnset_RecordLargeStraightWithValidSetAsFourty()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var largeStraightScore = _scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			_scoreSheet.LargeStraight.Should().Be(40);
			largeStraightScore.Should().Be(40);
		}

		[Test]
		public void ScoreSheet_LargeStraightUnset_RecordLargeStraightWithInvalidSetAsZero()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var largeStraightScore = _scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			_scoreSheet.LargeStraight.Should().Be(0);
			largeStraightScore.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_LargeStraightUnset_DoNotOverwritePreviousValue()
		{
			// Arrange
			_straightValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordLargeStraight(_diceCup.Object);
			var secondRecord = _scoreSheet.RecordLargeStraight(_diceCup.Object);

			//Assert
			secondRecord.Should().NotHaveValue();
		}

		#endregion

		#region Chance tests

		[Test]
		public void ScoreSheet_ChanceUnset_RecordChanceAsValidValueOfEighteen()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(6, 3, 2, 4, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			// Act
			var chanceScore = _scoreSheet.RecordChance(_diceCup.Object);

			//Assert
			_scoreSheet.Chance.Should().Be(18);
			chanceScore.Should().Be(18);
		}

		[Test]
		public void ScoreSheet_ChanceHasAlreadyBeenSet_DoNotOverwritePreviousValue()
		{
			// Arrange
			var dice = _testDieFactory.CreateDieEnumerable(3, 2, 5, 6, 3);
			_diceCup.Setup(x => x.Dice).Returns(dice);

			var dice2 = _testDieFactory.CreateDieEnumerable(1, 1, 1, 2, 1);
			_diceCup2.Setup(x => x.Dice).Returns(dice2);

			// Act
			_scoreSheet.RecordChance(_diceCup.Object);
			var chanceScore = _scoreSheet.RecordChance(_diceCup2.Object);

			//Assert
			chanceScore.Should().NotHaveValue();
			_scoreSheet.Chance.Should().Be(19);
		}

		#endregion

		#region Yahtzee tests

		[Test]
		public void ScoreSheet_YahtzeeUnset_RecordYahtzeeWithValidSet()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			var yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(50);
			_scoreSheet.Yahtzee.Should().Be(50);
		}

		[Test]
		public void ScoreSheet_YahtzeeUnset_RecordYahtzeeWithInvalidSetAsZero()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(false);

			// Act
			var yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			// Assert
			yahtzeeScore.Should().Be(0);
			_scoreSheet.Yahtzee.Should().Be(0);
		}

		[Test]
		public void ScoreSheet_YahtzeeSet_RecordYahtzeeBonusAfterYahtzeeWithValidSet()
		{
			// Arrange
			var expectedArray = new int?[] {100};
			_diceOfAKindValidator.Setup(x => x.IsValid(5, It.IsAny<IEnumerable<IDie>>())).Returns(true);

			// Act
			_scoreSheet.RecordYahtzee(_diceCup.Object);
			var yahtzeeScore = _scoreSheet.RecordYahtzee(_diceCup.Object);

			yahtzeeScore.Should().Be(100);
			_scoreSheet.YahtzeeBonus.Should().BeEquivalentTo(expectedArray);
		}

		[Test]
		public void ScoreSheet_YahtzeeIsZero_DoNotSetYahtzeeBonus()
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

		[Test]
		public void ScoreSheet_YahtzeeSet_RecordYahtzeeBonusOnlyThreeTimes()
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

		[Test]
		public void ScoreSheet_LowerSectionTotal_ReturnsSumOfAllLowerSectionItems()
		{
			// Arrange
			_diceOfAKindValidator.Setup(x => x.IsValid(It.IsAny<int>(), It.IsAny<IEnumerable<IDie>>())).Returns(true);
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);
			_straightValidator.Setup(x => x.IsValid(It.IsAny<int>(), It.IsAny<IEnumerable<IDie>>())).Returns(true);
			SetupLowerSectionMock();
			RecordAllLowerSection(_diceCup.Object);

			// Act
			var lowerTotal = _scoreSheet.LowerSectionTotal;

			// Assert
			lowerTotal.Should().Be(304);
		}

		[Test]
		public void ScoreSheet_GrandTotal_ReturnsSumOfUpperSectionTotalWithBonusAndLowerSectionTotal()
		{
			// Arrange
			SetupBonusDiceCupMock();
			RecordAllUpperSection(_diceCup.Object);

			_diceOfAKindValidator.Setup(x => x.IsValid(It.IsAny<int>(), It.IsAny<IEnumerable<IDie>>())).Returns(true);
			_fullHouseValidator.Setup(x => x.IsValid(It.IsAny<IEnumerable<IDie>>())).Returns(true);
			_straightValidator.Setup(x => x.IsValid(It.IsAny<int>(), It.IsAny<IEnumerable<IDie>>())).Returns(true);
			SetupLowerSectionMock();
			RecordAllLowerSection(_diceCup.Object);

			// Act
			var grandTotal = _scoreSheet.GrandTotal;

			// Assert
			grandTotal.Should().Be(405);
		}

		#endregion

		#region Private helper methods

		private void SetupNoBonusDiceCupMock()
		{
			var onesDice = _testDieFactory.CreateDieEnumerable(1, 3, 1, 5, 4); // 2
			var twosDice = _testDieFactory.CreateDieEnumerable(1, 5, 2, 2, 2); // 6
			var threesDice = _testDieFactory.CreateDieEnumerable(3, 4, 3, 3, 3); // 12
			var foursDice = _testDieFactory.CreateDieEnumerable(4, 1, 4, 2, 2); // 8
			var fivesDice = _testDieFactory.CreateDieEnumerable(1, 4, 3, 6, 6); // 0
			var sixesDice = _testDieFactory.CreateDieEnumerable(1, 3, 6, 4, 3); // 6
			// total = 34

			_diceCup.Setup(x => x.Dice).ReturnsInOrder(onesDice, twosDice, threesDice, foursDice, fivesDice, sixesDice);
		}

		private void SetupBonusDiceCupMock()
		{
			var onesDice = _testDieFactory.CreateDieEnumerable(1, 3, 1, 5, 1); // 3
			var twosDice = _testDieFactory.CreateDieEnumerable(1, 5, 2, 2, 2); // 6
			var threesDice = _testDieFactory.CreateDieEnumerable(3, 4, 3, 3, 3); // 12
			var foursDice = _testDieFactory.CreateDieEnumerable(4, 1, 4, 2, 4); // 12
			var fivesDice = _testDieFactory.CreateDieEnumerable(5, 5, 3, 5, 6); // 15
			var sixesDice = _testDieFactory.CreateDieEnumerable(6, 3, 6, 4, 6); // 18
			// total = 66

			_diceCup.Setup(x => x.Dice).ReturnsInOrder(onesDice, twosDice, threesDice, foursDice, fivesDice, sixesDice);
		}

		private void SetupBonusDiceCupMock_63Points()
		{
			var onesDice = _testDieFactory.CreateDieEnumerable(1, 3, 1, 5, 1); // 3
			var twosDice = _testDieFactory.CreateDieEnumerable(1, 5, 2, 2, 2); // 6
			var threesDice = _testDieFactory.CreateDieEnumerable(3, 4, 3, 4, 3); // 9
			var foursDice = _testDieFactory.CreateDieEnumerable(4, 1, 4, 2, 4); // 12
			var fivesDice = _testDieFactory.CreateDieEnumerable(5, 5, 3, 5, 6); // 15
			var sixesDice = _testDieFactory.CreateDieEnumerable(6, 3, 6, 4, 6); // 18
			// total = 63

			_diceCup.Setup(x => x.Dice).ReturnsInOrder(onesDice, twosDice, threesDice, foursDice, fivesDice, sixesDice);
		}

		private void SetupLowerSectionMock()
		{
			var threeOfAKind = _testDieFactory.CreateDieEnumerable(4, 4, 4, 3, 1); // 16
			var fourOfAKind = _testDieFactory.CreateDieEnumerable(6, 6, 6, 6, 1); // 25
			var fullHouse = _testDieFactory.CreateDieEnumerable(1, 1, 1, 4, 4); // 25
			var smallStraight = _testDieFactory.CreateDieEnumerable(2, 3, 4, 6, 5); // 30
			var largeStraight = _testDieFactory.CreateDieEnumerable(1, 2, 3, 4, 5); // 40
			var yahtzee = _testDieFactory.CreateDieEnumerable(4, 4, 4, 4, 4); // 50
			var yahtzeeBonus = _testDieFactory.CreateDieEnumerable(2, 2, 2, 2, 2); // 100
			var chance = _testDieFactory.CreateDieEnumerable(3, 6, 3, 2, 4); // 18
			// total = 304


			// Three of a kind and four of a kind are duplicated because setting them requires the dice in the cup
			// to be examined twice.  It's a leaky abstraction but it will do for a test.
			_diceCup.Setup(x => x.Dice).ReturnsInOrder(threeOfAKind, threeOfAKind, fourOfAKind, fourOfAKind, fullHouse, smallStraight, largeStraight,
				yahtzee, yahtzeeBonus, chance);
		}

		private void RecordAllUpperSection(IDiceCup diceCup)
		{
			_scoreSheet.RecordUpperSection(UpperSectionItem.Ones, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Twos, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Threes, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Fours, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Fives, diceCup);
			_scoreSheet.RecordUpperSection(UpperSectionItem.Sixes, diceCup);
		}

		private void RecordAllLowerSection(IDiceCup diceCup)
		{
			_scoreSheet.RecordThreeOfAKind(diceCup);
			_scoreSheet.RecordFourOfAKind(diceCup);
			_scoreSheet.RecordFullHouse(diceCup);
			_scoreSheet.RecordSmallStraight(diceCup);
			_scoreSheet.RecordLargeStraight(diceCup);
			_scoreSheet.RecordYahtzee(diceCup);
			_scoreSheet.RecordYahtzee(diceCup);
			_scoreSheet.RecordChance(diceCup);
		}

		#endregion
	}
}