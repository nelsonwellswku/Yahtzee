using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YahtzeeTests.Support
{
	class ConsoleCapturer : IDisposable
	{
		private TextWriter _previousOut;
		private StringWriter _capturer;

		public ConsoleCapturer()
		{
			_capturer = new StringWriter();
			_previousOut = Console.Out;
			Console.SetOut(_capturer);
		}

		public string GetStandardOut()
		{
			return _capturer.ToString();
		}

		public void Dispose()
		{
			Console.SetOut(_previousOut);
		}
	}
}
