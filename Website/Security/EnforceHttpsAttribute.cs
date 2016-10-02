using System;
using System.Web.Mvc;

namespace Website.Security
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class EnforceHttpsAttribute : RequireHttpsAttribute
	{
		public override void OnAuthorization(AuthorizationContext filterContext)
		{
			if(filterContext == null)
			{
				throw new ArgumentNullException(nameof(filterContext));
			}

			if(filterContext.HttpContext.Request.IsSecureConnection)
			{
				return;
			}

			if(string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"],
				"https",
				StringComparison.InvariantCultureIgnoreCase))
			{
				return;
			}

			if(filterContext.HttpContext.Request.IsLocal)
			{
				return;
			}

			HandleNonHttpsRequest(filterContext);
		}
	}
}