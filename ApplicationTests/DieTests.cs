﻿using FluentAssertions;
using Octogami.Yahtzee.Application.Framework;
using Xunit;

namespace YahtzeeTests
{
	public class DieTests
	{
		[Fact]
		public void Die_RollDie_ValueShouldBeInSixSidedDieRange()
		{
			// Arrange
			var die = new Die();

			// This is not deterministic, but for 100 rolls, the result should be in the correct range all the time
			for(var i = 0; i < 100; i++)
			{
				// Act
				var rollResult = die.Roll();

				// Assert
				die.Value.Should().BeInRange(1, 6);
				die.Value.Should().Be(rollResult);
			}
		}

		[Fact]
		public void Die_RollDie_ValueFrequencyShouldBeRelativelyEvenlyDistributed()
		{
			// Arrange
			var die = new Die();
			var rolls = new[] {0, 0, 0, 0, 0, 0, 0};

			// This is not completely deterministic, but given 100 rolls, each valid value (1, 2, 3, 4, 5, 6) should happen at least 5 times each
			for(var i = 0; i < 100; i++)
			{
				// Act
				rolls[die.Roll()]++;
			}

			// Assert
			rolls[0].Should().Be(0);
			for(var i = 1; i <= 6; i++)
			{
				rolls[i].Should().BeGreaterOrEqualTo(5);
			}
		}

		[Fact]
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

		[Fact]
		public void Die_HeldDieShouldNotChangeValueWhenThrown()
		{
			var die = new Die();
			var oldValue = die.Roll();
			die.State = DieState.Held;

			var newValue = die.Roll();
			newValue.Should().Be(oldValue);
		}
	}
}