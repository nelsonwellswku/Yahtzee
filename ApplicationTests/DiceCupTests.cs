﻿using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Octogami.Yahtzee.Application.Framework;
using Xunit;

namespace YahtzeeTests
{
	public class DiceCupTests
	{
		private int _rollIteration;
		private int _valueIteration;
		private IEnumerable<int> _dieRolls;
		private Mock<IDie> _dieMock;

		public DiceCupTests()
		{
			_dieRolls = new List<int> {4, 3, 2, 1, 2, 4, 2, 3, 6, 4, 4, 5, 5, 1, 2, 3, 5, 3, 2, 5, 1, 6, 6, 2, 3, 4, 1, 2, 5, 3};
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

		[Fact]
		public void DiceCup_ConstructorTakesFiveDice_DiceCupConstructedSuccessfully()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};

			// Act
			var diceCup = new DiceCup(dice);

			// Assert
			diceCup.Should().NotBeNull();
		}

		[Fact]
		public void DiceCup_ConstructorTakesMoreThanFiveDice_ArgumentOutOfRangeExceptionThrown()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die, die, die};

			// Act
			// ReSharper disable once ObjectCreationAsStatement
			Action act = () => new DiceCup(dice);

			// Assert
			act.Should().Throw<ArgumentOutOfRangeException>();
		}

		[Fact]
		public void DiceCup_InitialRoll_DieValuesMatchMockedDieValues()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};
			var expectedResult = new[] {4, 3, 2, 1, 2};

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			IEnumerable<IDie> rollResult = diceCup.Dice;

			// Assert
			rollResult.Select(x => x.Value).ToList().Should().BeEquivalentTo(expectedResult);
		}

		[Fact]
		public void DiceCup_InitialRollSaveFirstThreeValues_ThreeDiceInTheCupWerePlacedInTheHeldState()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Hold(0, 1, 2);

			//Assert
			_dieMock.VerifySet(x => x.State = DieState.Held, Times.Exactly(3));
		}

		[Fact]
		public void DiceCup_InitialRollHoldThreeValuesUnholdTwoValues_ThreeDiceInTheCupWerePlacedInTheHeldStateAndTwoDiceInTheCupWerePlacedInTheThrowableState()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Hold(0, 2, 4);
			diceCup.Unhold(2, 4);

			// Assert
			_dieMock.VerifySet(x => x.State = DieState.Held, Times.Exactly(3));
			_dieMock.VerifySet(x => x.State = DieState.Throwable, Times.Exactly(2));
		}

		[Fact]
		public void DiceCup_RollTwoTimesAndCheckFinalizedState_FinalizedStateIsFalse()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Roll();

			// Assert
			diceCup.IsFinal().Should().BeFalse();
		}

		[Fact]
		public void DiceCup_RollThreeTimesAndCheckFinalizedState_FinalizedStateIsTrue()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Roll();
			diceCup.Roll();

			// Assert
			diceCup.IsFinal().Should().BeTrue();
		}

		[Fact]
		public void DiceCup_RollThreeTimesAndPreventFourthRoll_ReturnsNull()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, die, die};

			// Act
			var diceCup = new DiceCup(dice);
			diceCup.Roll();
			diceCup.Roll();
			diceCup.Roll();

			// Assert
			IEnumerable<IDie> finalRoll = diceCup.Roll();
			finalRoll.Should().BeNull();
		}

		[Fact]
		public void DiceCupKeepsTrackOfNumberOfRolls()
		{
			var die = _dieMock.Object;
			var dice = new List<IDie> {die, die, die, /* my darling */ die, die};

			var diceCup = new DiceCup(dice);
			diceCup.RollCount.Should().Be(0);

			diceCup.Roll();
			diceCup.RollCount.Should().Be(1);

			diceCup.Roll();
			diceCup.RollCount.Should().Be(2);

			diceCup.Roll();
			diceCup.RollCount.Should().Be(3);

			diceCup.Roll();
			diceCup.RollCount.Should().Be(3);
		}
	}
}