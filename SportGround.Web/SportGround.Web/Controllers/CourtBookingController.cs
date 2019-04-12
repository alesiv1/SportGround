using SportGround.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Security.Claims;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;

namespace SportGround.Web.Controllers
{
    public class CourtBookingController : Controller
    {
	    private ICourtBookingService _bookingOperations;
	    private IUserService _userOperations;
	    private ICourtService _courtOperations;

		public CourtBookingController(ICourtBookingService booking, IUserService user, ICourtService courtOperations)
	    {
		    _bookingOperations = booking;
		    _userOperations = user;
		    _courtOperations = courtOperations;
	    }

		[Authorize]
		public ActionResult Index()
		{
			var email = ((ClaimsIdentity)this.User.Identity)
				.FindFirst(ClaimTypes.Email)?.Value;
			if (email != null)
			{
				var allUserBookings = new List<CourtBookingModel>();
				try
				{
					allUserBookings = _bookingOperations.GetAllUserBooking(email);
				}
				catch { }
				return View(allUserBookings);
			}
			return RedirectToAction("Index", "User");
		}

		[Authorize]
		public ActionResult BookingCourt(int courtId)
		{
			var email = ((ClaimsIdentity)this.User.Identity)
				.FindFirst(ClaimTypes.Email)?.Value;
			var user = _userOperations.GetUserList()
				.FirstOrDefault(em => em.Email == email);
			var court = _courtOperations.GetCourtById(courtId);
			var availableDateTime = _bookingOperations.GetAllAvailableDataTime(court.Id);
			if (availableDateTime.Count < 1)
			{
				RedirectToAction("Index", "Court");
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
				Date = Convert.ToDateTime(model.AvailableDate.FirstOrDefault())
	        };
	        _bookingOperations.Create(booking);
	        return RedirectToAction("Index", "Court");
        }

		[Authorize]
		public ActionResult DeclineBookingCourt(int id)
		{
			var booking = _bookingOperations.GetCourtBookingById(id);
			return View(booking);
		}

		[Authorize]
		[HttpPost]
		public ActionResult DeclineBookingCourt(int id, CourtBookingModel model)
        {
			try
			{
				_bookingOperations.Delete(id);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}