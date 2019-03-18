using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.BusinessLogic.Operations;

namespace SportGround.Web.Controllers
{
    public class CourtController : Controller
    {
	    private CourtOperations _courtData;

	    public CourtController(CourtOperations courtOperations)
	    {
		    _courtData = courtOperations;
	    }

        // GET: Court
        public ActionResult Index()
        {
	        var allCourt = _courtData.GetAll();
			return View();
        }

        // GET: Court/Details/5
        public ActionResult Details(int id)
        {
	        var court = _courtData.GetCourtById(id);
			return View(court);
        }

        // GET: Court/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Court/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
				var court = GetModel(collection);
				_courtData.Create(court);
				return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Court/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_courtData.GetCourtById(id));
        }

        // POST: Court/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
				var court = GetModel(collection);
				_courtData.Update(id, court);
				return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Court/Delete/5
        public ActionResult Delete(int id)
        {
            return View(_courtData.GetCourtById(id));
        }

        // POST: Court/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
				_courtData.Delete(id);
				return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private CourtModel GetModel(IFormCollection collection)
        {
	        int id = Convert.ToInt32(collection["Id"]);
	        string name = Convert.ToString(collection["Name"]);
			return new CourtModel()
			{
				Id = id,
				Name = name
			};
        }
    }
}