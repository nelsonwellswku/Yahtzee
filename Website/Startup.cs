using Autofac;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using Autofac.Integration.SignalR;
using Yahtzee;

[assembly: OwinStartupAttribute(typeof(Website.Startup))]
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
