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
			int[] dice = { 3, 4, 3, 5, 3 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[TestMethod]
		public void ValidateInvalidThreeOfAKind()
		{
			int[] dice = { 3, 1, 5, 3, 4 };

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeFalse();
		}
	}
}