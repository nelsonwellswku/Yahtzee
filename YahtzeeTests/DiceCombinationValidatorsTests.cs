
using FluentAssertions;
using YahtzeeTests.Support;
using Yahtzee.Framework.DiceCombinationValidators;
using NUnit.Framework;

namespace YahtzeeTests
{

	public class DiceCombinationValidatorsTests
	{
		private TestDieFactory _testDieFactory = new TestDieFactory();

		#region Dice of a kind tests
		[Test]
		public void DiceOfAKindValidator_ValidThreeOfAKindSet_ReturnsTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 3, 5, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[Test]
		public void DiceOfAKindValidator_ValidThreeOfAKindSetMoreDups_ReturnsTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 3, 3, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeTrue();
		}

		[Test]
		public void DiceOfAKindValidator_InvalidThreeOfAKindSet_ReturnsFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 1, 5, 3, 4 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(3, dice);
			result.Should().BeFalse();
		}

		[Test]
		public void DiceOfAKindValidator_ValidFourOfAKindSet_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 2, 2, 6, 2, 2 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeTrue();
		}

		[Test]
		public void DiceOfAKindValidator_ValidFourOfAKindSetMoreThanFourDups_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 2, 2, 2, 2, 2 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeTrue();
		}

		[Test]
		public void DiceOfAKindValidator_InvalidFourOfAKindSet_ReturnFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 5, 2, 5 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(4, dice);
			result.Should().BeFalse();
		}

		[Test]
		public void DiceOfAKindValidator_ValidFiveOfAKindSet_ReturnTrue()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 3, 3, 3 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeTrue();
		}

		[Test]
		public void DiceOfAKindValidator_InvalidFiveOfAKindSet_ReturnsFalse()
		{
			var dice = _testDieFactory.CreateDieEnumerable(new[] { 3, 3, 3, 3, 6 });

			DiceOfAKindValidator diceOfAKindValidator = new DiceOfAKindValidator();
			bool result = diceOfAKindValidator.IsValid(5, dice);
			result.Should().BeFalse();
		}
		#endregion

		#region Full house tests
		[Test]
		public void FullHouseValidator_ValidFullHouseSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 3, 2, 3 });

			FullHouseValidator fullHouseValidator = new FullHouseValidator();
			bool result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeTrue();
		}

		[Test]
		public void FullHouseValidator_InvalidFullHouseSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 2, 3, 4, 3 });

			FullHouseValidator fullHouseValidator = new FullHouseValidator();
			bool result = fullHouseValidator.IsValid(diceValues);
			result.Should().BeFalse();
		}
		#endregion

		#region Straight tests
		[Test]
		public void StraightValidator_ValidSmallStraightSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 2, 4, 6 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[Test]
		public void StraightValidator_ValidSmallStraightSetVariation_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 1, 6, 4, 5, 3 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[Test]
		public void StraightValidator_ValidSmallStraightSetWithDuplicate_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 1, 3, 4, 2 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeTrue();
		}

		[Test]
		public void StraightValidator_InvalidSmallStraightSet_ReturnsFalse()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 1, 3, 5, 2, 6 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(4, diceValues);
			result.Should().BeFalse();
		}

		[Test]
		public void StraightValidator_ValidLargeStraightSet_ReturnsTrue()
		{
			var diceValues = _testDieFactory.CreateDieEnumerable(new[] { 3, 4, 2, 6, 5 });

			StraightValidator straightValidator = new StraightValidator();
			bool result = straightValidator.IsValid(5, diceValues);
			result.Should().BeTrue();
		}

		[Test]
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