using System;
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

		public CourtWorkingHoursController(ICourtWorkingHoursOperations operationsHours)
	    {
		    _courtWorkingHoursOperations = operationsHours;
		}

		[Authorize]
		public ActionResult Index(int courtId)
		{
			var allHours = _courtWorkingHoursOperations.GetAllForCourt(courtId);
			var name = allHours.FirstOrDefault() != null ? allHours.FirstOrDefault().Court.Name : "";
			CourtWithWorkingHoursModel courtWithWorkingHours = new CourtWithWorkingHoursModel()
			{
				Id = courtId,
				Name = name,
				AllWorkingHours = allHours
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
			return View(new CourtWorkingHoursModel()
            {
				Court = new CourtModel(){Id = courtId, Name = "Just defolt"},
				Day = (DaysOfTheWeek) DateTime.UtcNow.Day,
				StartTime = DateTimeOffset.UtcNow,
				EndTime = DateTimeOffset.UtcNow,
            });
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Create(CourtWorkingHoursModel model)
        {
	        var id = model.Court.Id;
	        if (!ModelState.IsValid)
	        {
		        return View();
	        }
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
	        if (!ModelState.IsValid)
	        {
		        return View();
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
