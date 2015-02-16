using System.Data.Entity;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.SignalR;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Website;
using Website.Controllers;
using Website.DAL;
using Yahtzee;
using AutofacDependencyResolver = Autofac.Integration.Mvc.AutofacDependencyResolver;

[assembly: OwinStartup(typeof(Startup))]

namespace Website
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var builder = new ContainerBuilder();

			// MVC Registrations
			builder.RegisterControllers(typeof(HomeController).Assembly);
			builder.RegisterType<ApplicationDbContext>().As<DbContext>().InstancePerRequest();

			// SignalR Registrations
			builder.RegisterType<ApplicationDbContext>();
			builder.RegisterHubs(typeof(YahtzeeHub).Assembly);

			builder.RegisterYahtzee();

			var container = builder.Build();
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

			app.UseAutofacMiddleware(container);

			ConfigureAuth(app);

			// SignalR must be mapped after auth!
			var config = new HubConfiguration {Resolver = new Autofac.Integration.SignalR.AutofacDependencyResolver(container)};
			app.MapSignalR("/signalr", config);
		}
	}
}