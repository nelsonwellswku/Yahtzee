using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Moq;
using Yahtzee.Framework;

namespace YahtzeeTests
{
	[TestClass]
	public class DiceCupTests
	{
		private int _rollIteration;
		private int _valueIteration;
		private IEnumerable<int> _dieRolls;
		private Mock<IDie> _dieMock;

		public DiceCupTests()
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
			_dieMock.Setup(die => die.Value).Returns(() => {
				_valueIteration++;
				return _dieRolls.ElementAt(_valueIteration); 
			});
		}

		[TestMethod]
		public void ConstructAProperlySizedDiceCup()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };
			
			// Act
			var diceCup = new DiceCup(dice);

			// Assert
			diceCup.Should().NotBeNull();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void ConstructAnImproperlySizedDiceCup()
		{
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die, die, die };
			
			// Act
			var diceCup = new DiceCup(dice);


			// Assert handled by ExpectedException decorator
		}

		[TestMethod]
		public void InitialRoll()
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

		[TestMethod]
		public void InitialRollSaveFirstThreeValues()
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

		[TestMethod]
		public void InitialRollHoldThreeValuesUnholdTwoValues()
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

		[TestMethod]
		public void RollTwoTimesAndCheckFinalizedStateForUnfinalized()
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

		[TestMethod]
		public void RollThreeTimesAndCheckFinalizedStateForFinalized()
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

		[TestMethod]
		public void RollThreeTimesAndPreventFourthRoll()
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