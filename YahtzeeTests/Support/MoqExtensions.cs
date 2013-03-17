using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;
using Moq.Language.Flow;

namespace YahtzeeTests.Support
{
	public static class MoqExtensions
	{
		// Found here: http://haacked.com/archive/2009/09/28/moq-sequences.aspx
		public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
		  params TResult[] results) where T : class
		{
			setup.Returns(new Queue<TResult>(results).Dequeue);
		}
	}
}
