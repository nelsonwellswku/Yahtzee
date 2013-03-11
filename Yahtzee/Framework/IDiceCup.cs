using System.Collections.Generic;

namespace Yahtzee.Framework
{
	public interface IDiceCup
	{
		IEnumerable<IDie> Roll();
	}
}