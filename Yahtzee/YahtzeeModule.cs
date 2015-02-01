using Autofac;
using System.Collections.Generic;
using System.Linq;
using Yahtzee.Framework;
using Yahtzee.Framework.DiceCombinationValidators;

namespace Yahtzee
{
	class YahtzeeModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DiceOfAKindValidator>().As<IDiceOfAKindValidator>();
			builder.RegisterType<FullHouseValidator>().As<IFullHouseValidator>();
			builder.RegisterType<StraightValidator>().As<IStraightValidator>();

			builder.Register(context =>
			{
				IDiceCup diceCup;
				var dice = Enumerable.Range(0, 5).Select(x => new Die() as IDie).ToList();
				diceCup = new DiceCup(dice);
				return diceCup;
			});

			builder.RegisterType<ScoreSheet>().As<IScoreSheet>();
		}
	}

	public static class ContainerBuilderExtensions
	{
		public static void RegisterYahtzee(this ContainerBuilder builder)
		{
			builder.RegisterModule<YahtzeeModule>();
		}
	}
}
