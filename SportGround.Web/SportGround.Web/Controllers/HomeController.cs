using System;
using System.Web.Mvc;

namespace SportGround.Web.Controllers
{
	public class HomeController : Controller
	{
		[Authorize]
		public ActionResult Index()
		{
			return View();
		}
	}
}