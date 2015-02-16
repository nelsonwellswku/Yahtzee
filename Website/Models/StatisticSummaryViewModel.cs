using System.Collections.Generic;

namespace Website.Models
{
	public class StatisticSummaryViewModel
	{
		public int TotalGamesPlayed { get; set; }
		public ScoreViewModel HighestScore { get; set; }
		public ScoreViewModel LowestScore { get; set; }

		public IEnumerable<ScoreViewModel> Scores { get; set; }
	}
}