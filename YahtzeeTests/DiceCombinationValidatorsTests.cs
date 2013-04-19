using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using YahtzeeTests.Support;
using Yahtzee.Framework;
using Yahtzee.Framework.DiceCombinationValidators;
using System.Collections.Generic;

namespace YahtzeeTests
{
	[TestClass]
	public class DiceCombinationValidatorsTests
	{
		private TestDieFactory _testDieFactory = new TestDieFactory();

		#region Dice of a kind tests
		[TestMethod]
		public void DiceOfAKindValidator_ValidThreeOfAKindSet_ReturnsTrue()
		{
			var dice =  _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 3, 5, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void DiceOfAKindValidator_InvalidThreeOfAKindSet_ReturnsFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 1, 5, 3, 4 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void DiceOfAKindValidator_ValidFourOfAKindSet_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 2, 2, 6, 2, 2 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void DiceOfAKindValidator_InvalidFourOfAKindSet_ReturnFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 5, 2, 5 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void DiceOfAKindValidator_ValidFiveOfAKindSet_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 3, 3, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void DiceOfAKindValidator_InvalidFiveOfAKindSet_ReturnsFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 3, 3, 6 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeFalse();
		}
		#endregion

		#region Full house tests
		[TestMethod]
		public void FullHouseValidator_ValidFullHouseSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 3, 2, 3 });

			FullHouseValidator fullHouseValidator = new FullHouseValidator();
			bool result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void FullHouseValidator_InvalidFullHouseSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 3, 4, 3 });

			FullHouseValidator fullHouseValidator = new FullHouseValidator();
			bool result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeFalse();
		}
		#endregion

		#region Straight tests
		[TestMethod]
		public void StraightValidator_ValidSmallStraightSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[]{ 1, 3, 2, 4, 6 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void StraightValidator_InvalidSmallStraightSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 5, 2, 6 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void StraightValidator_ValidLargeStraightSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 2, 6, 5 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(5, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void StraightValidator_InvalidLargeStraightSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 4, 1, 6 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(5, diceValues);
			result.Should().BeFalse();
		}
		#endregion
	}
}