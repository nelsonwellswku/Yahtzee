using System;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Yahtzee.Framework;

namespace YahtzeeTests
{
	[TestClass]
	public class DieTests
	{
		[TestMethod]
		public void RollDie()
		{
			// Arrange
			var die = new Die(DieState.Throwable);

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

		[TestMethod]
		public void TestRollFrequency()
		{
			// Arrange
			var die = new Die(DieState.Throwable);
			var rolls = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

			// This is not completely deterministic, but given 100 rolls, each valid value (1, 2, 3, 4, 5, 6) should happen at least 10 times each
			for (int i = 0; i < 100; i++)
			{
				// Act
				rolls[die.Roll()]++;
			}

			// Assert
			rolls[0].Should().Be(0);
			for (int i = 1; i <= 6; i++)
			{
				rolls[i].Should().BeGreaterThan(10);
			}
		}

		[TestMethod]
		public void VerifyDieStates()
		{
			// I'm not convinced about the value of testing automatic properties,
			// but I'm not positive it isn't valuable either.
			// In any case, I want as much code coverage as practical and this is an easy win.

			var die = new Die(DieState.Throwable);
			die.State.Should().Be(DieState.Throwable);

			die.State = DieState.Held;
			die.State.Should().Be(DieState.Held);

			die.State = DieState.Throwable;
			die.State.Should().Be(DieState.Throwable);
		}
	}
}