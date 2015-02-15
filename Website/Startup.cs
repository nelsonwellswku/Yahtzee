using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Website;
using Yahtzee;

[assembly: OwinStartup(typeof(Startup))]

namespace Website
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var builder = new ContainerBuilder();

			var config = new HubConfiguration();
			builder.RegisterHubs(Assembly.GetExecutingAssembly());
			builder.RegisterYahtzee();

			var container = builder.Build();
			config.Resolver = new AutofacDependencyResolver(container);

			app.UseAutofacMiddleware(container);
			app.MapSignalR("/signalr", config);

			ConfigureAuth(app);
		}
	}
}