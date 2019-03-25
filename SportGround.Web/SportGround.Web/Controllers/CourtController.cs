using System;
using System.Web.Mvc;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.BusinessLogic.Operations;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
		private ICourtOperations _courtOperations;

		public CourtController(ICourtOperations operations)
	    {
		    _courtOperations = operations;
	    }

		// GET: Court
		[Authorize]
		[Route("Court")]
		public ActionResult Index()
		{
			var allCourt = _courtOperations.GetAll();
			return View(allCourt);
        }

		// GET: Court/Details/5
		[Authorize]
		public ActionResult Details(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
            return View(court);
        }

		// GET: Court/Create
		[Authorize]
		public ActionResult Create()
        {
            return View();
        }

		// POST: Court/Create
		[Authorize]
		[HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
	            var court = GetCourt(collection);
				_courtOperations.Create(court);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		// GET: Court/Edit/5
		[Authorize]
		public ActionResult Edit(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
            return View(court);
        }

		// POST: Court/Edit/5
		[Authorize]
		[HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
				var court = GetCourt(collection);
				_courtOperations.Update(id, court);
				return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		// GET: Court/Delete/5
		[Authorize]
		public ActionResult Delete(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
			return View(court);
        }

		// POST: Court/Delete/5
		[Authorize]
		[HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
				_courtOperations.Delete(id);
				return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		private CourtModel GetCourt(FormCollection collectio)
        {
	        int id = Convert.ToInt32(Request.Form["Id"]) + 1;
			string name = Convert.ToString(Request.Form["Name"]);
	        return new CourtModel()
	        {
				Id = id,
				Name = name
	        };
        }
    }
}
