using System;
using System.Web.Mvc;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
		private ICourtService _courtServices;
		private IBookingService _bookingServices;

		public CourtController(ICourtService services, IBookingService bookingServices)
	    {
		    _courtServices = services;
		    _bookingServices = bookingServices;
	    }

		[Authorize]
		[Route("Court")]
		public ActionResult Index()
		{
			var allCourt = _courtServices.GetCourtList();
			allCourt.ForEach(canbook => canbook.CanBooking = _bookingServices.GetAllAvailableDataTime(canbook.Id).Count > 0);
			return View(allCourt);
        }

		[Authorize]
		public ActionResult Details(int id)
        {
	        var court = _courtServices.GetCourtById(id);
            return View(court);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create()
        {
            return View();
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(CourtModel court)
        {
	        if (!ModelState.IsValid)
	        {
		        return View();
	        }
			_courtServices.Create(court);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
        {
	        var court = _courtServices.GetCourtById(id);
            return View(court);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Edit(int id, CourtModel court)
        {
			if (!ModelState.IsValid)
			{
				return View();
			}
			_courtServices.Update(id, court);
			return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
	        var court = _courtServices.GetCourtById(id);
			return View(court);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, CourtModel court)
        {
			_courtServices.DeleteCourt(id);
			return RedirectToAction("Index");
        }
    }
}
