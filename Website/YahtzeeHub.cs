using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using Yahtzee.Framework;
using System;
using System.Linq;
using Autofac;

namespace Website
{
	public class YahtzeeHub : Hub
	{
		private static Dictionary<string, StateModel> _stateDict = new Dictionary<string, StateModel>();

		private readonly ILifetimeScope _hubScope;

		private readonly Func<IScoreSheet> _scoreSheetFactory;
		private readonly Func<IDiceCup> _diceCupFactory;


		// This isn't awesome but to control the lifetime scope of the hub's dependencies,
		// the root container needs to be passed in, similar to a service locator.
		// See http://autofac.readthedocs.org/en/latest/integration/signalr.html
		public YahtzeeHub(ILifetimeScope rootScope)
		{
			_hubScope = rootScope.BeginLifetimeScope();

			_scoreSheetFactory = _hubScope.Resolve<Func<IScoreSheet>>();
			_diceCupFactory = _hubScope.Resolve<Func<IDiceCup>>();
		}

		public void RollDice()
		{
			var state = GetOrCreateState();

			var rollResult = state.CurrentDiceCup.Roll();
			if (rollResult != null)
			{
				Clients.Caller.displayDice(rollResult.Select(x => x.Value).ToList());
			}
			else
			{
				state.CurrentDiceCup = _diceCupFactory();
			}
		}

		public void TakeUpper(int number)
		{
			var state = GetOrCreateState();
			UpperSectionItem section = (UpperSectionItem) number;
			state.ScoreSheet.RecordUpperSection(section, state.CurrentDiceCup);

			var score = GetScoreForUpperSection(section, state.ScoreSheet);

			Clients.Caller.setUpper(new { upperNum = number, score = score });
		}

		private int GetScoreForUpperSection(UpperSectionItem section, IScoreSheet scoreSheet)
		{
			switch(section)
			{
				case UpperSectionItem.Ones:
					return scoreSheet.Ones.Value;
				case UpperSectionItem.Twos:
					return scoreSheet.Twos.Value;
				case UpperSectionItem.Threes:
					return scoreSheet.Threes.Value;
				case UpperSectionItem.Fours:
					return scoreSheet.Fours.Value;
				case UpperSectionItem.Fives:
					return scoreSheet.Fives.Value;
				case UpperSectionItem.Sixes:
					return scoreSheet.Sixes.Value;
			}

			return 0;
		}

		private StateModel GetOrCreateState()
		{
			var currentConnectionId = Context.ConnectionId;
			StateModel state;
			var stateExists = _stateDict.TryGetValue(currentConnectionId, out state);

			if (!stateExists)
			{
				state = new StateModel
				{
					ConnectionId = currentConnectionId,
					ScoreSheet = _scoreSheetFactory(),
					CurrentDiceCup = _diceCupFactory()
				};
				_stateDict.Add(currentConnectionId, state);
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

	public class StateModel
	{
		public string ConnectionId { get; set; }
		public IScoreSheet ScoreSheet { get; set; }
		public IDiceCup CurrentDiceCup { get; set; }
	}
}