using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using System.Web.Mvc;
using SportGround.Data.Enums;
using FluentValidation.Results;
using SportGround.BusinessLogic.Validation;

namespace SportGround.Web.Controllers
{
    public class CourtWorkingDaysController : Controller
    {
	    private ICourtWorkingDaysService _courtWorkingDaysServices;
	    private ICourtService _courtServices;
	    private WorkingDaysValidation workingDaysValid = new WorkingDaysValidation();

		public CourtWorkingDaysController(ICourtWorkingDaysService servicesDays, ICourtService services)
	    {
		    _courtWorkingDaysServices = servicesDays;
		    _courtServices = services;
	    }

		[Authorize]
		public ActionResult Index(int courtId)
		{
			var allDays = _courtWorkingDaysServices.GetWorkingDaysForCourt(courtId);
			var isWorkingDays = allDays.Count < 1;
			CourtWithWorkingDaysModel courtWithWorkingHours = new CourtWithWorkingDaysModel()
			{
				Id = courtId,
				Name = _courtServices.GetCourtById(courtId).Name,
				AllWorkingDays = allDays,
				IsWorkingDays = isWorkingDays
			};
			return View(courtWithWorkingHours);
		}

		[Authorize]
		public ActionResult Details(int id)
        {
	        var hours = _courtWorkingDaysServices.GetWorkingDay(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		public ActionResult Create(int courtId)
		{
			var court = _courtServices.GetCourtById(courtId);
			var days = new List<DaysOfTheWeek>(){ DaysOfTheWeek.Monday};
			if (days.Count < 1)
			{
				return View("Index", new { courtId});
			}
			return View(new CourtWorkingDaysModel()
            {
				Court = court,
				Day = (DaysOfTheWeek) DateTime.Now.Day,
				StartTime = DateTimeOffset.Now,
				EndTime = DateTimeOffset.Now,
            });
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(CourtWorkingDaysModel model)
        {
	        var id = model.Court.Id;
			var validationResult = workingDaysValid.Validate(model);
	        if (!validationResult.IsValid)
	        {
		        foreach (ValidationFailure data in validationResult.Errors)
		        {
			        ModelState.AddModelError(data.PropertyName, data.ErrorMessage);
		        }
		        return View(model);
	        }
	        _courtWorkingDaysServices.Create(id, model);
	        return RedirectToAction("Index","CourtWorkingDays", new { courtId = id });
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
        {
			var hours = _courtWorkingDaysServices.GetWorkingDay(id);
			return View(hours);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Edit(int id, CourtWorkingDaysModel model)
        {
	        var validationResult = workingDaysValid.Validate(model);
	        if (!validationResult.IsValid)
	        {
		        foreach (ValidationFailure data in validationResult.Errors)
		        {
			        ModelState.AddModelError(data.PropertyName, data.ErrorMessage);
		        }
		        return View(model);
	        }
			_courtWorkingDaysServices.Update(id, model);
	        return RedirectToAction("Index", new { courtId = model.Court.Id});
		}

        [Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
	        var hours = _courtWorkingDaysServices.GetWorkingDay(id);
	        return View(hours);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, CourtWorkingDaysModel model)
        {
	        var Id = _courtWorkingDaysServices.GetWorkingDay(id).Court.Id;
	        _courtWorkingDaysServices.DeleteWorkingDays(id);
			return RedirectToAction("Index", new { courtId = Id});
		}
    }
}
