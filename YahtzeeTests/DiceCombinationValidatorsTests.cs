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
		public void ValidateThreeOfAKind()
		{
			var dice =  _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 3, 5, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidThreeOfAKind()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 1, 5, 3, 4 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateFourOfAKind()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 2, 2, 6, 2, 2 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidFourOfAKind()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 5, 2, 5 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateFiveOfAKind()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 3, 3, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidFiveOfAKind()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 3, 3, 6 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeFalse();
		}
		#endregion

		#region Full house tests
		[TestMethod]
		public void ValidateFullHouse()
		{
			int[] diceValues = { 3, 2, 3, 2, 3 };

			FullHouseValidator fullHouseValidator = new FullHouseValidator();
			bool result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidFullHouse()
		{
			int[] diceValues = { 3, 2, 3, 4, 3 };

			FullHouseValidator fullHouseValidator = new FullHouseValidator();
			bool result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeFalse();
		}
		#endregion

		#region Straight tests
		[TestMethod]
		public void ValidateSmallStraight()
		{
			int[] diceValues = { 1, 3, 2, 4, 6 };

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateSmallInvalidStraight()
		{
			int[] diceValues = { 1, 3, 5, 2, 6 };

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateLargeStraight()
		{
			int[] diceValues = { 3, 4, 2, 6, 5 };

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(5, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidLargeStraight()
		{
			int[] diceValues = { 3, 2, 4, 1, 6 };

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(5, diceValues);
			result.Should().BeFalse();
		}
		#endregion
	}
}