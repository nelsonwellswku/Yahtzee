﻿using FluentAssertions;
using Octogami.Yahtzee.Application.Framework.DiceCombinationValidators;
using Xunit;
using YahtzeeTests.Support;

namespace YahtzeeTests
{
	public class DiceCombinationValidatorsTests
	{
		private readonly TestDieFactory _testDieFactory = new TestDieFactory();

		#region Dice of a kind tests

		[Fact]
		public void DiceOfAKindValidator_ValidThreeOfAKindSet_ReturnsTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(3, 4, 3, 5, 3);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[Fact]
		public void DiceOfAKindValidator_ValidThreeOfAKindSetMoreDups_ReturnsTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(3, 4, 3, 3, 3);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[Fact]
		public void DiceOfAKindValidator_InvalidThreeOfAKindSet_ReturnsFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(3, 1, 5, 3, 4);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeFalse();
		}

		[Fact]
		public void DiceOfAKindValidator_ValidFourOfAKindSet_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(2, 2, 6, 2, 2);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeTrue();
		}

		[Fact]
		public void DiceOfAKindValidator_ValidFourOfAKindSetMoreThanFourDups_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(2, 2, 2, 2, 2);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeTrue();
		}

		[Fact]
		public void DiceOfAKindValidator_InvalidFourOfAKindSet_ReturnFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(1, 3, 5, 2, 5);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeFalse();
		}

		[Fact]
		public void DiceOfAKindValidator_ValidFiveOfAKindSet_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(3, 3, 3, 3, 3);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeTrue();
		}

		[Fact]
		public void DiceOfAKindValidator_InvalidFiveOfAKindSet_ReturnsFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(3, 3, 3, 3, 6);

			var diceOfAKindValidator = new DiceOfAKindValidator();
			var result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeFalse();
		}

		#endregion

		#region Full house tests

		[Fact]
		public void FullHouseValidator_ValidFullHouseSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(3, 2, 3, 2, 3);

			var fullHouseValidator = new FullHouseValidator();
			var result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeTrue();
		}

		[Fact]
		public void FullHouseValidator_InvalidFullHouseSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(3, 2, 3, 4, 3);

			var fullHouseValidator = new FullHouseValidator();
			var result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeFalse();
		}

		#endregion

		#region Straight tests

		[Fact]
		public void StraightValidator_ValidSmallStraightSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(1, 3, 2, 4, 6);

			var straightValidator = new StraightValidator();
			var result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[Fact]
		public void StraightValidator_ValidSmallStraightSetVariation_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(1, 6, 4, 5, 3);

			var straightValidator = new StraightValidator();
			var result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[Fact]
		public void StraightValidator_ValidSmallStraightSetWithDuplicate_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(3, 1, 3, 4, 2);

			var straightValidator = new StraightValidator();
			var result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[Fact]
		public void StraightValidator_InvalidSmallStraightSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(1, 3, 5, 2, 6);

			var straightValidator = new StraightValidator();
			var result = straightValidator.IsValid(4, diceValues);
			result.Should().BeFalse();
		}

		[Fact]
		public void StraightValidator_ValidLargeStraightSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(3, 4, 2, 6, 5);

			var straightValidator = new StraightValidator();
			var result = straightValidator.IsValid(5, diceValues);
			result.Should().BeTrue();
		}

		[Fact]
		public void StraightValidator_InvalidLargeStraightSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(3, 2, 4, 1, 6);

			var straightValidator = new StraightValidator();
			var result = straightValidator.IsValid(5, diceValues);
			result.Should().BeFalse();
		}

		#endregion
	}
}