
using FluentAssertions;
using Yahtzee.Framework;
using NUnit.Framework;

namespace YahtzeeTests
{

	public class DieTests
	{
		[Test]
		public void Die_RollDie_ValueShouldBeInSixSidedDieRange()
		{
			// Arrange
			var die = new Die();

			// This is not deterministic, but for 100 rolls, the result should be in the correct range all the time
			for (int i = 0; i < 100; i++)
			{
				// Act
				var rollResult = die.Roll();

				// Assert
				die.Value.Should().BeInRange(1, 6);
				die.Value.Should().Be(rollResult);
			}
		}

		[Test]
		public void Die_RollDie_ValueFrequencyShouldBeRelativelyEvenlyDistributed()
		{
			// Arrange
			var die = new Die();
			var rolls = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

			// This is not completely deterministic, but given 100 rolls, each valid value (1, 2, 3, 4, 5, 6) should happen at least 5 times each
			for (int i = 0; i < 100; i++)
			{
				// Act
				rolls[die.Roll()]++;
			}

			// Assert
			rolls[0].Should().Be(0);
			for (int i = 1; i <= 6; i++)
			{
				rolls[i].Should().BeGreaterOrEqualTo(5);
			}
		}

		[Test]
		public void Die_StatesChangeFromThrowableToHeldAndBackToThrowable_StateShouldMatchThrowableHeldThrowableInSequence()
		{
			// I'm not convinced about the value of testing automatic properties,
			// but I'm not positive it isn't valuable either.
			// In any case, I want as much code coverage as practical and this is an easy win.

			var die = new Die();
			die.State.Should().Be(DieState.Throwable);

			die.State = DieState.Held;
			die.State.Should().Be(DieState.Held);

			die.State = DieState.Throwable;
			die.State.Should().Be(DieState.Throwable);
		}
	}
}