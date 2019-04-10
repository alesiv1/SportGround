using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using System.Web.Mvc;
using SportGround.Data.Enums;

namespace SportGround.Web.Controllers
{
    public class CourtWorkingHoursController : Controller
    {
	    private ICourtWorkingHoursOperations _courtWorkingHoursOperations;
	    private ICourtOperations _courtOperations;

		public CourtWorkingHoursController(ICourtWorkingHoursOperations operationsHours, ICourtOperations operations)
	    {
		    _courtWorkingHoursOperations = operationsHours;
		    _courtOperations = operations;
	    }

		[Authorize]
		public ActionResult Index(int courtId)
		{
			var allHours = _courtWorkingHoursOperations.GetAllForCourt(courtId);
			var isAvaAvailableDays = _courtWorkingHoursOperations.GetAllAvailableDays(courtId).Count > 0;
			CourtWithWorkingHoursModel courtWithWorkingHours = new CourtWithWorkingHoursModel()
			{
				Id = courtId,
				Name = _courtOperations.GetCourtById(courtId).Name,
				AllWorkingHours = allHours,
				IsAvailableDays = isAvaAvailableDays
			};
			return View(courtWithWorkingHours);
		}

		[Authorize]
		public ActionResult Details(int id)
        {
	        var hours = _courtWorkingHoursOperations.GetById(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create(int courtId)
		{
			var court = _courtOperations.GetCourtById(courtId);
			var days = _courtWorkingHoursOperations.GetAllAvailableDays(courtId);
			if (days.Count < 1)
			{
				return View("Index", new { courtId});
			}
			return View(new CourtWorkingHoursModel()
            {
				Court = court,
				Day = (DaysOfTheWeek) DateTime.Now.Day,
				AvailableDays = days,
				StartTime = DateTimeOffset.Now,
				EndTime = DateTimeOffset.Now,
            });
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(CourtWorkingHoursModel model)
        {
	        if (model.StartTime >= model.EndTime)
	        {
		        ModelState.AddModelError("StartTime", "Start time must be less then ent time!");
				return View(model);
			}
	        var id = model.Court.Id;
	        _courtWorkingHoursOperations.Create(id, model);
	        return RedirectToAction("Index","CourtWorkingHours", new { courtId = id });
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
        {
			var hours = _courtWorkingHoursOperations.GetById(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Edit(int id, CourtWorkingHoursModel model)
        {
	        if (model.StartTime >= model.EndTime)
	        {
		        ModelState.AddModelError("StartTime", "Start time must be less then ent time!");
		        return View(model);
			}
			_courtWorkingHoursOperations.Update(id, model);
	        return RedirectToAction("Index", new { courtId = model.Court.Id});
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
	        var hours = _courtWorkingHoursOperations.GetById(id);
	        return View(hours);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, CourtWorkingHoursModel model)
        {
	        var Id = _courtWorkingHoursOperations.GetById(id).Court.Id;
	        _courtWorkingHoursOperations.Delete(id);
			return RedirectToAction("Index", new { courtId = Id});
		}
    }
}
