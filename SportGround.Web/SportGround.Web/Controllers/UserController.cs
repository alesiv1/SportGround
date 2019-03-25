using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportGround.BusinessLogic.Enums;

namespace SportGround.Web.Controllers
{
    public class UserController : Controller
    {
	    private IUserOperations _userOperations;

	    public UserController(IUserOperations operations)
	    {
		    _userOperations = operations;
	    }

		// GET: User
		[Authorize]
		[Route("Users")]
		public ActionResult Index()
		{
			var allUsers = _userOperations.GetAll();
			return View(allUsers);
		}

		// GET: User/Details/5
		[Authorize]
		public ActionResult Details(int id)
		{
			var user = _userOperations.GetUserById(id);
			return View(user);
		}

		// GET: User/Create
		[Authorize]
		public ActionResult Create()
		{
			return View();
		}

		// POST: User/Create
		[Authorize]
		[HttpPost]
		public ActionResult Create(FormCollection collection)
		{
			try
			{
				var user = GetUser(collection);
				_userOperations.Create(user);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: User/Edit/5
		[Authorize]
		public ActionResult Edit(int id)
		{
			var user= _userOperations.GetUserById(id);
			return View(user);
		}

		// POST: User/Edit/5
		[Authorize]
		[HttpPost]
		public ActionResult Edit(int id, FormCollection collection)
		{
			try
			{
				var user = GetUser(collection);
				_userOperations.Update(id, user);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: User/Delete/5
		[Authorize]
		public ActionResult Delete(int id)
		{
			var user = _userOperations.GetUserById(id);
			return View(user);
		}

		// POST: User/Delete/5
		[HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
	        try
	        {
		        _userOperations.Delete(id);
		        return RedirectToAction("Index");
	        }
	        catch
	        {
		        return View();
	        }
		}

        private UserModel GetUser(FormCollection collectio)
        {
	        int id = Convert.ToInt32(Request.Form["Id"]) + 1;
	        string firstName = Convert.ToString(Request.Form["FirstName"]);
	        string lastName = Convert.ToString(Request.Form["LastName"]);
	        string role = Convert.ToString(Request.Form["Role"]);
	        string email = Convert.ToString(Request.Form["Email"]);
			string password = Convert.ToString(Request.Form["Password"]);
			return new UserModel()
	        {
		        Id = id,
		        FirstName = firstName,
				LastName = lastName,
				Email = email,
				Role = role == "Admin" ? UserRole.Admin : UserRole.User,
				Password = password
			};
        }
	}
}
