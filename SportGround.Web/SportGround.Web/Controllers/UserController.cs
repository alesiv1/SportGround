using SportGround.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class UserController : Controller
    {
	    private IUserOperations _userOperations;

	    public UserController(IUserOperations operations)
	    {
		    _userOperations = operations;
	    }

		[Authorize]
		[Route("Users")]
		public ActionResult Index()
		{
			var allUsers = _userOperations.GetAll();
			return View(allUsers);
		}

		[Authorize]
		public ActionResult Details(int id)
		{
			var user = _userOperations.GetUserById(id);
			return View(user);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Create()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public ActionResult Create(UserModelWithPassword user)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			try
			{
				_userOperations.Create(user);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		[Authorize]
		public ActionResult Edit(int id)
		{
			var user= _userOperations.GetUserById(id);
			return View(user);
		}

		[Authorize]
		[HttpPost]
		public ActionResult Edit(int id, UserModel user)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			try
			{
				_userOperations.Update(id, user);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Delete(int id)
		{
			var user = _userOperations.GetUserById(id);
			return View(user);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult Delete(int id, UserModelWithRole user)
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

        [Authorize]
		public ActionResult ResetPassword(int id)
        {
			var user = _userOperations.Users().FirstOrDefault();
			if (user == null)
			{
				throw new ArgumentException("User does'nt exist in database!");
			}
			var userForChange = new UserModelWithPassword()
			{
				Id = id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Password = _userOperations.GetPasswordHashCode(user.Password, user.Salt),
				ConfirmPassword = "",
			};
			return View(userForChange);
		}

		[Authorize]
        [HttpPost]
        public ActionResult ResetPassword(int id, UserModelWithPassword user)
        {
			try
	        {		        
		        _userOperations.Update(id, user);
		        return RedirectToAction("Index");
	        }
	        catch
	        {
		        return View();
	        }
		}

        [Authorize(Roles = "Admin")]
		public ActionResult ChangeRole(int id)
        {
			var user = _userOperations.GetUserById(id);
			return View(user);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult ChangeRole(int id, UserModelWithRole user)
        {
			try
			{
				_userOperations.Update(id, user);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
