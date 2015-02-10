using Yahtzee.Framework;

namespace Website.Models
{
	public class GameStateModel
	{
		public string ConnectionId { get; set; }
		public IScoreSheet ScoreSheet { get; set; }
		public IDiceCup CurrentDiceCup { get; set; }
	}
}