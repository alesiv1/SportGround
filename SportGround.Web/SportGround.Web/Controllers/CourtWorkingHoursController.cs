using System;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using System.Web.Mvc;
using SportGround.Data.Entities;
using SportGround.Data.Enums;

namespace SportGround.Web.Controllers
{
    public class CourtWorkingHoursController : Controller
    {
	    private ICourtWorkingHoursOperations _courtWorkingHoursOperations;
	    private ICourtOperations _courtOperations;

		public CourtWorkingHoursController(ICourtWorkingHoursOperations operationsHours, ICourtOperations operationsCourt)
	    {
		    _courtWorkingHoursOperations = operationsHours;
		    _courtOperations = operationsCourt;
		}

		[Authorize]
		public ActionResult Index(int courtId)
		{
			var allHours = _courtWorkingHoursOperations.GetAllForCourt(courtId);
			var court = _courtOperations.GetCourtById(courtId);
			CourtWithWorkingHoursModel courtWithWorkingHours = new CourtWithWorkingHoursModel()
			{
				Id = court.Id,
				Name = court.Name,
				AllWorkingHours = allHours
			};
            return View(courtWithWorkingHours);
        }

		[Authorize]
		public ActionResult Details(int id)
        {
	        var hours = _courtWorkingHoursOperations.GetCourtById(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create(int courtid)
		{
			var court = _courtOperations.GetCourtById(courtid);
			return View(new CourtWorkingHoursModel()
            {
				Court = court,
				Day = (DaysOfTheWeek) DateTime.UtcNow.Day,
				StartTime = DateTimeOffset.UtcNow,
				EndTime = DateTimeOffset.UtcNow,
            });
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(CourtWorkingHoursModel model)
        {
	        if (!ModelState.IsValid)
	        {
		        return View();
	        }
	        _courtWorkingHoursOperations.Create(model.Court.Id, model);
	        return RedirectToAction("Index", model.Court.Id);
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
        {
	        var hours = _courtWorkingHoursOperations.GetCourtById(id);
			return View();
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Edit(int id, CourtWorkingHoursModel model)
        {
	        if (!ModelState.IsValid)
	        {
		        return View();
	        }
	        _courtWorkingHoursOperations.Update(id, model);
	        return RedirectToAction("Index");
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
	        var hours = _courtWorkingHoursOperations.GetCourtById(id);
	        return View(hours);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, CourtWorkingHoursModel model)
        {
	        _courtWorkingHoursOperations.Delete(id);
	        return RedirectToAction("Index");
		}
    }
}
