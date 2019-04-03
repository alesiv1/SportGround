using SportGround.Web.Models;
using System;
using System.Web.Mvc;

namespace SportGround.Web.Controllers
{
    public class CourtBookingController : Controller
    {
		public ActionResult BookingCourt(CourtBookingModel model)
        {
            return View();
        }

        public ActionResult DeclineBookingCourt(CourtBookingModel model)
        {
	        return View();
        }
	}
}