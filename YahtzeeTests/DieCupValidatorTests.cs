using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Yahtzee.Framework.DiceCombinationValidators;

namespace YahtzeeTests
{
	[TestClass]
	public class DieCupValidatorTests
	{
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
	}
}