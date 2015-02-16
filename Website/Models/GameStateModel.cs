using System;
using Yahtzee.Framework;

namespace Website.Models
{
	public class GameStateModel
	{
		public GameStateModel()
		{
			GameStartTime = DateTime.UtcNow;
		}

		public string ConnectionId { get; set; }
		public string UserId { get; set; }
		public IScoreSheet ScoreSheet { get; set; }
		public IDiceCup CurrentDiceCup { get; set; }
		public DateTime GameStartTime { get; private set; }
	}
}