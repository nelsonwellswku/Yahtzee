using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Moq;
using Yahtzee.Framework;
using NUnit.Framework;
using System;

namespace YahtzeeTests
{

	public class DiceCupTests
	{
		private int _rollIteration;
		private int _valueIteration;
		private IEnumerable<int> _dieRolls;
		private Mock<IDie> _dieMock;

		[SetUp]
		public void Setup()
		{
			_dieRolls = new List<int> { 4, 3, 2, 1, 2, 4, 2, 3, 6, 4, 4, 5, 5, 1, 2, 3, 5, 3, 2, 5, 1, 6, 6, 2, 3, 4, 1, 2, 5, 3 };
			_dieMock = new Mock<IDie>();

			_rollIteration = -1;
			_dieMock.Setup(die => die.Roll()).Returns(() =>
			{
				_rollIteration++;
				return _dieRolls.ElementAt(_rollIteration);
			});

			_valueIteration = -1;
			_dieMock.Setup(die => die.Value).Returns(() =>
			{
				_valueIteration++;
				return _dieRolls.ElementAt(_valueIteration);
			});
		}

		[Test]
		public void DiceCup_ConstructorTakesFiveDice_DiceCupConstructedSuccessfully()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);

			// Assert
			diceCup.Should().NotBeNull();
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void DiceCup_ConstructorTakesMoreThanFiveDice_ArgumentOutOfRangeExceptionThrown()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);

			// Assert handled by ExpectedException attribute
		}

		[Test]
		public void DiceCup_InitialRoll_DieValuesMatchMockedDieValues()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };
			var expectedResult = new[] { 4, 3, 2, 1, 2 };

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			IEnumerable<IDie> rollResult = diceCup.Dice;

			// Assert
			rollResult.Select(x => x.Value).ToList().Should().BeEquivalentTo(expectedResult);
		}

		[Test]
		public void DiceCup_InitialRollSaveFirstThreeValues_ThreeDiceInTheCupWerePlacedInTheHeldState()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Hold(0, 1, 2);

			//Assert
			_dieMock.VerifySet(x => x.State = DieState.Held, Times.Exactly(3));
		}

		[Test]
		public void DiceCup_InitialRollHoldThreeValuesUnholdTwoValues_ThreeDiceInTheCupWerePlacedInTheHeldStateAndTwoDiceInTheCupWerePlacedInTheThrowableState()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Hold(0, 2, 4);
			diceCup.Unhold(2, 4);

			// Assert
			_dieMock.VerifySet(x => x.State = DieState.Held, Times.Exactly(3));
			_dieMock.VerifySet(x => x.State = DieState.Throwable, Times.Exactly(2));
		}

		[Test]
		public void DiceCup_RollTwoTimesAndCheckFinalizedState_FinalizedStateIsFalse()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Roll();

			// Assert
			diceCup.IsFinal().Should().BeFalse();
		}

		[Test]
		public void DiceCup_RollThreeTimesAndCheckFinalizedState_FinalizedStateIsTrue()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Roll();
			diceCup.Roll();

			// Assert
			diceCup.IsFinal().Should().BeTrue();
		}

		[Test]
		public void DiceCup_RollThreeTimesAndPreventFourthRoll_ReturnsNull()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Roll();
			diceCup.Roll();

			// Assert
			IEnumerable<IDie> finalRoll = diceCup.Roll();
			finalRoll.Should().BeNull();
		}
	}
}