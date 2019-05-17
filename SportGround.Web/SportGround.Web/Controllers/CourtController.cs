using System;
using System.Web.Mvc;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
		private readonly ICourtService _courtServices;
		private readonly IBookingService _bookingServices;
		private readonly ICourtWorkingDaysService _courtWorkingDaysServices;

		public CourtController(ICourtService services, IBookingService bookingServices, ICourtWorkingDaysService servicesDays)
	    {
		    _courtServices = services;
		    _bookingServices = bookingServices;
		    _courtWorkingDaysServices = servicesDays;
		}

		[Authorize]
		[Route("Court")]
		public ActionResult Index()
		{
			var allCourt = _courtServices.GetCourtList();
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
			if (_courtServices.CourtExists(court.Name))
			{
				ModelState.AddModelError("Name", "Court with name " + court.Name + "  already exists!");
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
