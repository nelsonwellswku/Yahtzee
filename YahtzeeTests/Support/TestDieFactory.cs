using System.Collections.Generic;
using System.Linq;
using Moq;
using Yahtzee.Framework;

namespace YahtzeeTests.Support
{
	public class TestDieFactory
	{
		public IList<IDie> CreateDieEnumerable(params int[] diceValues)
		{
			var dice = new IDie[diceValues.Length];

			var counter = 0;
			foreach(var value in diceValues)
			{
				var mock = new Mock<IDie>();
				mock.Setup(x => x.Value).Returns(value);
				dice[counter] = mock.Object;
				counter++;
			}

			return dice.ToList();
		}
	}
}