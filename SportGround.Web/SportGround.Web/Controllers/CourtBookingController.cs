using SportGround.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Security.Claims;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace SportGround.Web.Controllers
{
    public class CourtBookingController : Controller
    {
	    private IBookingService _bookingServices;
	    private IUserService _userServices;
	    private ICourtService _courtServices;

		public CourtBookingController(IBookingService bookingServices, IUserService userServices, ICourtService courtServices)
	    {
		    _bookingServices = bookingServices;
		    _userServices = userServices;
		    _courtServices = courtServices;
	    }

		[Authorize]
		public ActionResult Index()
		{
			var userId = -1;
			try
			{
				userId = Int32.Parse(((ClaimsIdentity)this.User.Identity).FindFirstValue("Id"));
			}
			catch { }
			if (userId != -1)
			{
				var allUserBookings = new List<CourtBookingModel>();
					allUserBookings = _bookingServices.GetAllUserBooking(userId);
				return View(allUserBookings);
			}
			return RedirectToAction("Index", "User");
		}

		[Authorize]
		public ActionResult DeclineBookingCourt(int id)
		{
			var booking = _bookingServices.GetCourtBookingById(id);
			return View(booking);
		}

		[Authorize]
		[HttpPost]
		public ActionResult DeclineBookingCourt(int id, CourtBookingModel model)
        {
			try
			{
				_bookingServices.Delete(id);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}