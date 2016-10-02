using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PagedList;
using Website.DAL.Entities;
using Website.Models;
using Website.Security;

namespace Website.Controllers
{
	[EnforceHttps]
	public class StatisticController : Controller
	{
		private readonly DbContext _dbContext;

		public StatisticController(DbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// GET: Statistics
		public async Task<ActionResult> Index(int? page)
		{
			var statSet = _dbContext.Set<GameStatistic>();

			var totalGamesPlayed = await _dbContext.Set<GameStatistic>().CountAsync();

			var highestScore = await statSet.OrderByDescending(x => x.FinalScore)
				.Select(x => new ScoreViewModel { User = x.User.DisplayName ?? "Anonymous", Score = x.FinalScore, Date = x.GameEndTime })
				.FirstAsync();

			var lowestScore = await statSet.OrderBy(x => x.FinalScore)
				.Select(x => new ScoreViewModel { User = x.User.DisplayName ?? "Anonymous", Score = x.FinalScore, Date = x.GameEndTime })
				.FirstAsync();

			var latestScores = await new TaskFactory().StartNew(() => statSet
				.OrderByDescending(x => x.GameEndTime)
				.Select(x => new ScoreViewModel {User = x.User.DisplayName ?? "Anonymous", Score = x.FinalScore, Date = x.GameEndTime})
				.ToPagedList(page ?? 1, 10));

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