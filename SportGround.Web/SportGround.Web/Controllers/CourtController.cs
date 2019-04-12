using System;
using System.Web.Mvc;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
		private ICourtService _courtOperations;

		public CourtController(ICourtService operations)
	    {
		    _courtOperations = operations;
	    }

		[Authorize]
		[Route("Court")]
		public ActionResult Index()
		{
			var allCourt = _courtOperations.GetCourtList();
			return View(allCourt);
        }

		[Authorize]
		public ActionResult Details(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
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
			_courtOperations.Create(court);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
		public ActionResult Edit(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
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
			_courtOperations.Update(id, court);
			return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
			return View(court);
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, CourtModel court)
        {
			_courtOperations.Delete(id);
			return RedirectToAction("Index");
        }
    }
}
