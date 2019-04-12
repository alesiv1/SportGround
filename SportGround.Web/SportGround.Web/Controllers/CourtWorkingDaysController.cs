using System;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using System.Web.Mvc;
using SportGround.Data.Enums;

namespace SportGround.Web.Controllers
{
    public class CourtWorkingDaysController : Controller
    {
	    private ICourtWorkingDaysService _courtWorkingDaysOperations;
	    private ICourtService _courtOperations;

		public CourtWorkingDaysController(ICourtWorkingDaysService operationsDays, ICourtService operations)
	    {
		    _courtWorkingDaysOperations = operationsDays;
		    _courtOperations = operations;
	    }

		[Authorize]
		public ActionResult Index(int courtId)
		{
			var allHours = _courtWorkingDaysOperations.GetWorkingDaysForCourt(courtId);
			var isAvaAvailableDays = _courtWorkingDaysOperations.GetAllAvailableDays(courtId).Count > 0;
			CourtWithWorkingDaysModel courtWithWorkingHours = new CourtWithWorkingDaysModel()
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
	        var hours = _courtWorkingDaysOperations.GetWorkingDay(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create(int courtId)
		{
			var court = _courtOperations.GetCourtById(courtId);
			var days = _courtWorkingDaysOperations.GetAllAvailableDays(courtId);
			if (days.Count < 1)
			{
				return View("Index", new { courtId});
			}
			return View(new CourtWorkingDaysModel()
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
        public ActionResult Create(CourtWorkingDaysModel model)
        {
	        if (model.StartTime >= model.EndTime)
	        {
		        ModelState.AddModelError("StartTime", "Start time must be less then ent time!");
				return View(model);
			}
	        if (model.StartTime.Hour < 0)
	        {
		        ModelState.AddModelError("StartTime", "Incorect time format!");
		        return View(model);
			}
	        if (model.EndTime.Hour > 24)
	        {
		        ModelState.AddModelError("EndTime", "Incorect time format!");
		        return View(model);
			}
	        var id = model.Court.Id;
	        _courtWorkingDaysOperations.Create(id, model);
	        return RedirectToAction("Index","CourtWorkingDays", new { courtId = id });
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
        {
			var hours = _courtWorkingDaysOperations.GetWorkingDay(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Edit(int id, CourtWorkingDaysModel model)
        {
	        if (model.StartTime >= model.EndTime)
	        {
		        ModelState.AddModelError("StartTime", "Start time must be less then ent time!");
		        return View(model);
			}
			_courtWorkingDaysOperations.Update(id, model);
	        return RedirectToAction("Index", new { courtId = model.Court.Id});
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
	        var hours = _courtWorkingDaysOperations.GetWorkingDay(id);
	        return View(hours);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, CourtWorkingDaysModel model)
        {
	        var Id = _courtWorkingDaysOperations.GetWorkingDay(id).Court.Id;
	        _courtWorkingDaysOperations.Delete(id);
			return RedirectToAction("Index", new { courtId = Id});
		}
    }
}
