using System;
using System.Web.Mvc;

namespace SportGround.Web.Controllers
{
    public class CourtBookingController : Controller
    {
        public ActionResult BookingCourt()
        {
            return View();
        }

        public ActionResult DeclineBookingCourt()
        {
	        return View();
        }
	}
}