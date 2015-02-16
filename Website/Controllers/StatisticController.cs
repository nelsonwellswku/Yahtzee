using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Website.DAL.Entities;
using Website.Models;

namespace Website.Controllers
{
	public class StatisticController : Controller
	{
		private readonly DbContext _dbContext;

		public StatisticController(DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// GET: Statistics
		public async Task<ActionResult> Index(int skip = 0, int take = 15)
		{
			var statSet = _dbContext.Set<GameStatistic>();

			var totalGamesPlayed = await _dbContext.Set<GameStatistic>().CountAsync();
			var highestScore = await statSet.OrderByDescending(x => x.FinalScore)
				.Select(x => new ScoreViewModel {User = x.User.Email, Score = x.FinalScore, Date = x.GameEndTime})
				.FirstAsync();
			var lowestScore = await statSet.OrderBy(x => x.FinalScore)
				.Select(x => new ScoreViewModel {User = x.User.Email, Score = x.FinalScore, Date = x.GameEndTime})
				.FirstAsync();

			var latestScores = await statSet
				.OrderByDescending(x => x.GameEndTime)
				.Skip(skip)
				.Take(take)
				.Select(x => new ScoreViewModel {User = x.User.Email, Score = x.FinalScore, Date = x.GameEndTime})
				.ToListAsync();

			var summaryViewModel = new StatisticSummaryViewModel
			{
				TotalGamesPlayed = totalGamesPlayed,
				HighestScore = highestScore,
				LowestScore = lowestScore,
				Scores = latestScores
			};

			return View(summaryViewModel);
		}
	}
}