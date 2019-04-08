using SportGround.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Security.Claims;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class CourtBookingController : Controller
    {
	    private ICourtBookingOperations _bookingOperations;
	    private IUserOperations _userOperations;
	    private ICourtOperations _courtOperations;

		public CourtBookingController(ICourtBookingOperations booking, IUserOperations user, ICourtOperations courtOperations)
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
			var userId = _userOperations
				.GetAll()
				.FirstOrDefault(em => em.Email == email)?.Id;
			var bookingCourts = _bookingOperations
				.GetAll()
				.Where(user => user.User.Id == userId)
				.ToList();
			bookingCourts.ForEach(booking => booking.IsActive = booking.Date.Date >= DateTimeOffset.Now.Date);
		    return View(bookingCourts);
	    }

		[Authorize]
		public ActionResult BookingCourt(int courtId)
		{
			var email = ((ClaimsIdentity)this.User.Identity)
				.FindFirst(ClaimTypes.Email)?.Value;
			var user = _userOperations.GetAll()
				.FirstOrDefault(em => em.Email == email);
			var court = _courtOperations.GetCourtById(courtId);
			var availableDataTime = _bookingOperations.GetAllAvailableDataTime(court.Id);
			if (availableDataTime.Count < 1)
			{
				RedirectToAction("Index", "Court");
			}
			CreateCourtBookingModel booking = new CreateCourtBookingModel()
			{
				User = user,
				Court = court,
				AvailableDate = availableDataTime
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
				CourtId = model.Court.Id,
				Date = model.AvailableDate.FirstOrDefault()
	        };
	        _bookingOperations.Create(booking);
	        return RedirectToAction("Index", "Court");
        }

		[Authorize]
		public ActionResult DeclineBookingCourt(int id)
		{
			var booking = _bookingOperations.GetById(id);
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