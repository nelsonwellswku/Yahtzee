using PagedList;

namespace Website.Models
{
	public class StatisticSummaryViewModel
	{
		public int TotalGamesPlayed { get; set; }
		public ScoreViewModel HighestScore { get; set; }
		public ScoreViewModel LowestScore { get; set; }

		public IPagedList<ScoreViewModel> Scores { get; set; }
	}
}