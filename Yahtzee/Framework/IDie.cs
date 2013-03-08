using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yahtzee.Framework
{
	public interface IDie
	{
		DieState State { get; set; }
		int Value { get; }
		int Roll();
	}
}
