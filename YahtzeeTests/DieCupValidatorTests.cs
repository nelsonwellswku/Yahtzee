using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Yahtzee.Framework.DiceCombinationValidators;

namespace YahtzeeTests
{
	[TestClass]
	public class DieCupValidatorTests
	{
		#region Dice of a kind tests
		[TestMethod]
		public void ValidateThreeOfAKind()
		{
			int[] diceValues = { 3, 4, 3, 5, 3 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidThreeOfAKind()
		{
			int[] diceValues = { 3, 1, 5, 3, 4 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, diceValues);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateFourOfAKind()
		{
			int[] diceValues = { 2, 2, 6, 2, 2 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidFourOfAKind()
		{
			int[] diceValues = { 1, 3, 5, 2, 5 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, diceValues);
			result.Should().BeFalse();
		}

		[TestMethod]
		public void ValidateFiveOfAKind()
		{
			int[] diceValues = { 3, 3, 3, 3, 3 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, diceValues);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidFiveOfAKind()
		{
			int[] diceValues = { 3, 3, 3, 3, 6 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, diceValues);
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