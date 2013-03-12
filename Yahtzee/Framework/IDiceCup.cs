using System.Collections.Generic;

namespace Yahtzee.Framework
{
	public interface IDiceCup
	{
		IEnumerable<IDie> Dice { get; }
		
		IEnumerable<IDie> Roll();
		void Hold(params int[] indicesToHold);
		void Unhold(params int[] indicesToHold);
		bool IsFinal();
	}
}