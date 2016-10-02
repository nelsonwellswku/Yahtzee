using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Octogami.Yahtzee.Application.Framework;
using Website.DAL;
using Website.DAL.Entities;
using Website.HubHelpers;
using Website.Models;

namespace Website
{
	public class YahtzeeHub : Hub
	{
		private static readonly ConcurrentDictionary<string, GameStateModel> StateDict = new ConcurrentDictionary<string, GameStateModel>();

		private readonly Func<IDiceCup> _diceCupFactory;
		private readonly ILifetimeScope _hubScope;
		private readonly Func<IScoreSheet> _scoreSheetFactory;
		private readonly ApplicationDbContext _dbContext;
		private readonly DbSet<ApplicationUser> _userRepository;
		private readonly DbSet<GameStatistic> _gameStatRepository;

		// This isn't awesome but to control the lifetime scope of the hub's dependencies,
		// the root container needs to be passed in, similar to a service locator.
		// See http://autofac.readthedocs.org/en/latest/integration/signalr.html
		public YahtzeeHub(ILifetimeScope rootScope)
		{
			_hubScope = rootScope.BeginLifetimeScope();

			_scoreSheetFactory = _hubScope.Resolve<Func<IScoreSheet>>();
			_diceCupFactory = _hubScope.Resolve<Func<IDiceCup>>();
			_dbContext = _hubScope.Resolve<ApplicationDbContext>();
			_userRepository = _dbContext.Set<ApplicationUser>();
			_gameStatRepository = _dbContext.Set<GameStatistic>();
		}

		public override Task OnConnected()
		{
			CreateState();
			return base.OnConnected();
		}

		public override Task OnDisconnected(bool stopCalled)
		{
			GameStateModel removedModel;
			StateDict.TryRemove(Context.ConnectionId, out removedModel);
			return base.OnDisconnected(stopCalled);
		}

		public void RollDice()
		{
			var state = GetState();

			if(state.CurrentDiceCup.IsFinal())
			{
				return;
			}

			var rollResult = state.CurrentDiceCup.Roll();
			if(rollResult != null)
			{
				var rollData = new
				{
					dice = rollResult.Select(x => x.Value).ToList(),
					rollCount = state.CurrentDiceCup.RollCount,
					isFinal = state.CurrentDiceCup.IsFinal()
				};

				Clients.Caller.processRoll(rollData);
			}
			else
			{
				state.CurrentDiceCup = _diceCupFactory();
			}
		}

		public void TakeUpper(int number)
		{
			var state = GetState();

			if(state.CurrentDiceCup.RollCount == 0)
			{
				return;
			}

			var section = (UpperSectionItem) number;
			state.ScoreSheet.RecordUpperSection(section, state.CurrentDiceCup);

			var score = GetScoreForUpperSection(section, state.ScoreSheet);

			state.CurrentDiceCup = _diceCupFactory();

			var isUpperSectionComplete = state.ScoreSheet.IsUpperSectionComplete;
			int? upperSectionScore = null;
			int? upperSectionBonus = null;
			int? upperSectionTotal = null;
			if(isUpperSectionComplete)
			{
				upperSectionScore = state.ScoreSheet.UpperSectionTotal;
				upperSectionBonus = state.ScoreSheet.UpperSectionBonus;
				upperSectionTotal = state.ScoreSheet.UpperSectionTotalWithBonus;
			}

			var isScoreSheetComplete = state.ScoreSheet.IsScoreSheetComplete;
			int? grandTotal = state.ScoreSheet.IsScoreSheetComplete ? (int?)state.ScoreSheet.GrandTotal : null;

			Clients.Caller.setUpper(new
			{
				upperNum = number,
				score,
				isUpperSectionComplete,
				upperSectionScore,
				upperSectionBonus,
				upperSectionTotal,
				isScoreSheetComplete,
				grandTotal
			});

			if (state.ScoreSheet.IsScoreSheetComplete)
			{
				SaveStatistics(isGameComplete: true);
			}
		}

		public void TakeLower(string name)
		{
			var state = GetState();

			if(state.CurrentDiceCup.RollCount == 0)
			{
				return;
			}

			var score = LowerSectionScorer.Score[name](state.ScoreSheet, state.CurrentDiceCup);

			state.CurrentDiceCup = _diceCupFactory();
			var isLowerSectionComplete = state.ScoreSheet.IsLowerSectionComplete;
			int? lowerSectionTotal = null;
			if(isLowerSectionComplete)
			{
				lowerSectionTotal = state.ScoreSheet.LowerSectionTotal;
			}
			var isScoreSheetComplete = state.ScoreSheet.IsScoreSheetComplete;
			int? grandTotal = state.ScoreSheet.IsScoreSheetComplete ? (int?)state.ScoreSheet.GrandTotal : null;

			Clients.Caller.setLower(new
			{
				name,
				score = score ?? -1,
				isLowerSectionComplete,
				lowerSectionTotal,
				isScoreSheetComplete,
				grandTotal
			});

			if (state.ScoreSheet.IsScoreSheetComplete)
			{
				SaveStatistics(isGameComplete: true);
			}
		}

		public void ToggleHoldDie(int index)
		{
			var state = GetState();

			if(state.CurrentDiceCup.IsFinal() || state.CurrentDiceCup.RollCount == 0)
			{
				return;
			}

			if(state.CurrentDiceCup.Dice[index].State == DieState.Held)
			{
				state.CurrentDiceCup.Unhold(index);
			}
			else
			{
				state.CurrentDiceCup.Hold(index);
			}

			Clients.Caller.toggleHoldDie(new
			{
				index,
				dieState = state.CurrentDiceCup.Dice[index].State.ToString()
			});
		}

		private void SaveStatistics(bool isGameComplete)
		{
			var state = GetState();
			var user = _userRepository.Find(state.UserId);
			var statistic = new GameStatistic
			{
				User = user,
				FinalScore = state.ScoreSheet.GrandTotal,
				GameCompleted = isGameComplete,
				GameEndTime = DateTime.UtcNow,
				GameStartTime = state.GameStartTime
			};
			_gameStatRepository.Add(statistic);
			_dbContext.SaveChanges();
		}

		private static int GetScoreForUpperSection(UpperSectionItem section, IScoreSheet scoreSheet)
		{
			switch(section)
			{
				case UpperSectionItem.Ones:
					return scoreSheet.Ones ?? -1;
				case UpperSectionItem.Twos:
					return scoreSheet.Twos ?? -1;
				case UpperSectionItem.Threes:
					return scoreSheet.Threes ?? -1;
				case UpperSectionItem.Fours:
					return scoreSheet.Fours ?? -1;
				case UpperSectionItem.Fives:
					return scoreSheet.Fives ?? -1;
				case UpperSectionItem.Sixes:
					return scoreSheet.Sixes ?? -1;
			}

			return 0;
		}

		private void CreateState()
		{
			var connectionId = Context.ConnectionId;
			var user = Context.User;
			string userId = null;
			if(user != null)
			{
				userId = user.Identity.GetUserId<string>();
			}

			var gameState = new GameStateModel(connectionId, userId)
			{
				ScoreSheet = _scoreSheetFactory(),
				CurrentDiceCup = _diceCupFactory()
			};


			StateDict.AddOrUpdate(connectionId, gameState, (key, existingVal) => gameState);
		}

		private GameStateModel GetState()
		{
			var connectionId = Context.ConnectionId;
			GameStateModel state;
			var stateExists = StateDict.TryGetValue(connectionId, out state);

			if(!stateExists)
			{
				throw new InvalidOperationException("Game not available.");
			}

			return state;
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing && _hubScope != null)
			{
				_hubScope.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}