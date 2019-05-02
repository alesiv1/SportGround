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
		public ActionResult BookingCourt(int courtId)
		{
			var userId = -1;
			try
			{
				userId = Int32.Parse(((ClaimsIdentity) this.User.Identity).FindFirstValue("Id"));
			}
			catch
			{
				return RedirectToAction("Index", "Court");
			}
			var user = _userServices.GetUserById(userId);
			var court = _courtServices.GetCourtById(courtId);
			var availableDateTime = _bookingServices.GetAllAvailableDataTime(court.Id);
			if (availableDateTime.Count < 1)
			{
				return RedirectToAction("Index", "Court");
			}
			List<string> availableDate = new List<string>();
			foreach (var date in availableDateTime)
			{
				availableDate.Add(date.Date.ToString("yyyy-M-d dddd"));
			}
			CreateCourtBookingModel booking = new CreateCourtBookingModel()
			{
				User = user,
				Court = court,
				AvailableDate = availableDate
			};
			return View(booking);
		}

		[Authorize]
		[HttpPost]
		public ActionResult BookingCourt(CreateCourtBookingModel model)
        {
			CourtBookingModel booking = new CourtBookingModel()
	        {
				Id = model.Id,
				User = model.User,
				Court = model.Court,
				StartDate = Convert.ToDateTime(model.AvailableDate.FirstOrDefault()),
				EndDate = Convert.ToDateTime(model.AvailableDate.FirstOrDefault()),
			};
	        _bookingServices.Create(booking);
	        return RedirectToAction("Index", "Court");
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