using System;
using System.Web.Mvc;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
	    public CourtController() { }
		private ICourtOperations _courtOperations;

	    public CourtController(ICourtOperations operations)
	    {
		    _courtOperations = operations;
	    }

		// GET: Court
		public ActionResult Index()
		{
			var allCourt = _courtOperations.GetAll();
			if (allCourt == null)
			{
				return new HttpNotFoundResult();
			}
			return View(allCourt);
        }

        // GET: Court/Details/5
        public ActionResult Details(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
            return View(court);
        }

        // GET: Court/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Court/Create
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
        public ActionResult Edit(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
            return View(court);
        }

        // POST: Court/Edit/5
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
        public ActionResult Delete(int id)
        {
	        var court = _courtOperations.GetCourtById(id);
			return View(court);
        }

        // POST: Court/Delete/5
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
	        int id = Convert.ToInt32(Request.Form["Id"]);
			string name = Convert.ToString(Request.Form["Name"]);
	        return new CourtModel()
	        {
				Id = id,
				Name = name
	        };
        }
    }
}
