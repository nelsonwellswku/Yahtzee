using System.Web.Mvc;
using Website.Security;

namespace Website.Controllers
{
	[EnforceHttps]
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult HowToPlay()
		{
			return View();
		}
	}
}