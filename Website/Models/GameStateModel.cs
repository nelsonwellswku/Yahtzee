using System;
using Yahtzee.Framework;

namespace Website.Models
{
	public class GameStateModel
	{
		public string ConnectionId { get; private set; }
		public string UserId { get; private set; }
		public DateTime GameStartTime { get; private set; }

		public IScoreSheet ScoreSheet { get; set; }
		public IDiceCup CurrentDiceCup { get; set; }

		public GameStateModel(string connectionId, string userId)
		{
			ConnectionId = connectionId;
			UserId = userId;
			GameStartTime = DateTime.UtcNow;
		}
	}
}