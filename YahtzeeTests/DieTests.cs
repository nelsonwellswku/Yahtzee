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
		public void TestRollRange()
		{
			var die = new Die();

			// This is not completely deterministic, but given 100 rolls, only the valid range (1 - 6) should be generated
			for (int i = 0; i < 100; i++)
			{
				var roll = die.Roll();
				roll.Should().BeInRange(1, 6);
			}
		}

		[TestMethod]
		public void TestRollFrequency()
		{
			var die = new Die();
			var rolls = new int[7] { 0, 0, 0, 0, 0, 0, 0 };

			// This is not completely deterministic, but given 100 rolls, each valid value (1, 2, 3, 4, 5, 6) should happen at least 5 times each
			for (int i = 0; i < 100; i++)
			{
				rolls[die.Roll()]++;
			}

			rolls[0].Should().Be(0);
			for (int i = 1; i <= 6; i++)
			{
				rolls[i].Should().BeGreaterThan(5);
			}
		}
	}
}