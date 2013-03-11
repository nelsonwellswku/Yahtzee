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
			_rollIteration = -1;
			_dieRolls = new List<int> { 4, 3, 2, 1, 2, 4, 2, 3, 6, 4, 4, 5, 5, 1, 2, 3, 5, 3, 2, 5, 1, 6, 6, 2, 3, 4, 1, 2, 5, 3 };

			_dieMock = new Mock<IDie>();
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
		public void InitialRoll()
		{ 
			// Arrange
			var die = _dieMock.Object;
			var dice = new List<IDie> { die, die, die, die, die };
			var expectedResult = new[] { 4, 3, 2, 1, 2 };

			// Act
			var DiceCup = new DiceCup(dice);
			DiceCup.Roll();
			IEnumerable<IDie> rollResult = DiceCup.Dice;
			
			// Assert
			rollResult.Select(x => x.Value).ToList().Should().BeEquivalentTo(expectedResult);
		}
	}
}