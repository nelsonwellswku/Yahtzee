using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yahtzee.Framework;

namespace ConsoleYahtzee.Framework
{
	public class DiceCupConsoleDisplay : IDiceCupDisplay
	{
		private const string _prompt = "Your current dice cup contents:\r\n";

		public void Show(IDiceCup diceCup)
		{
			StringBuilder sb = new StringBuilder(_prompt);
			foreach (var die in diceCup.Dice)
			{
				sb.Append("| ");
				sb.Append(die.Value);
				sb.Append(" | ");
			}
			Console.Write(sb.ToString().Trim());
		}
	}
}
