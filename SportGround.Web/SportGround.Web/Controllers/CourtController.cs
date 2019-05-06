using System;
using System.Linq;
using System.Web.Mvc;
using FluentValidation.Results;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.BusinessLogic.Validations;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
		private ICourtService _courtServices;
		private IBookingService _bookingServices;
		private ICourtWorkingDaysService _courtWorkingDaysServices;
		private CourtValidation courtValid = new CourtValidation();

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
			allCourt.ForEach(court => court.CanBooking = _courtWorkingDaysServices.GetWorkingDaysForCourt(court.Id).Count > 0);
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
	        var validationResult = courtValid.Validate(court);
	        if (!validationResult.IsValid)
	        {
		        foreach (ValidationFailure data in validationResult.Errors)
		        {
			        ModelState.AddModelError(data.PropertyName, data.ErrorMessage);
		        }
		        return View(court);
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
			var validationResult = courtValid.Validate(court);
			if (!validationResult.IsValid)
			{
				foreach (ValidationFailure data in validationResult.Errors)
				{
					ModelState.AddModelError(data.PropertyName, data.ErrorMessage);
				}
				return View(court);
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
