using Microsoft.AspNet.SignalR;
using System.Collections.Generic;
using Yahtzee.Framework;
using System;
using System.Linq;

namespace Website
{
	public class YahtzeeHub : Hub
	{
		private static Dictionary<string, StateModel> _stateDict = new Dictionary<string, StateModel>();
		private readonly Func<IScoreSheet> _scoreSheetFactory;
		private readonly Func<IDiceCup> _diceCupFactory;

		// TODO: This needs to take a lifetime scope dependency for easier dependency management
		// See http://autofac.readthedocs.org/en/latest/integration/signalr.html
		public YahtzeeHub(Func<IScoreSheet> scoreSheetFactory, Func<IDiceCup> diceCupFactory)
		{
			_scoreSheetFactory = scoreSheetFactory;
			_diceCupFactory = diceCupFactory;
		}

		public ICollection<int> RollDice()
		{
			var currentConnectionId = Context.ConnectionId;
			StateModel state;
			var stateExists = _stateDict.TryGetValue(currentConnectionId, out state);

			if(!stateExists)
			{
				state = new StateModel
				{
					ConnectionId = currentConnectionId,
					ScoreSheet = _scoreSheetFactory(),
					CurrentDiceCup = _diceCupFactory()
				};
				_stateDict.Add(currentConnectionId, state);
			}

			return state.CurrentDiceCup.Roll().Select(x => x.Value).ToList();
		}
	}

	public class StateModel
	{
		public string ConnectionId { get; set; }
		public IScoreSheet ScoreSheet { get; set; }
		public IDiceCup CurrentDiceCup { get; set; }
	}
}