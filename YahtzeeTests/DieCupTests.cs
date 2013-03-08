using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using FluentAssertions;
using Yahtzee.Framework;

namespace YahtzeeTests
{
	[TestClass]
	public class DieCupTests
	{
		[TestMethod]
		public void RollAllDice()
		{
			var die = new Die(DieState.Throwable);
			var dieCup = new DieCup(die);

			IEnumerable<int> cupRollResults = dieCup.Roll();
			cupRollResults.Count().Should().Be(5);
		}

		[TestMethod]
		public void RollSomeDice()
		{
			var die = new Die(DieState.Throwable);
			var dieCup = new DieCup(die);

			dieCup.Roll(1).Count().Should().Be(1);
			dieCup.Roll(2).Count().Should().Be(2);
			dieCup.Roll(3).Count().Should().Be(3);
			dieCup.Roll(4).Count().Should().Be(4);
			dieCup.Roll(5).Count().Should().Be(5);
		}
	}
}