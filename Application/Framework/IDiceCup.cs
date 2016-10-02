using System.Collections.Generic;

namespace Octogami.Yahtzee.Application.Framework
{
	public interface IDiceCup
	{
		IList<IDie> Dice { get; }

		IList<IDie> Roll();
		void Hold(params int[] indicesToHold);
		void Unhold(params int[] indicesToHold);
		bool IsFinal();
		int RollCount { get; }
	}
}