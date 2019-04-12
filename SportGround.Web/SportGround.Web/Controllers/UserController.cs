using SportGround.BusinessLogic.Interfaces;
using System;
using System.Linq;
using System.Web.Mvc;
using SportGround.BusinessLogic.Models;
using System.Security.Claims;
using SportGround.Data.entities;

namespace SportGround.Web.Controllers
{
    public class UserController : Controller
    {
	    private IUserService _userOperations;

	    public UserController(IUserService operations)
	    {
		    _userOperations = operations;
	    }

		[Authorize]
		[Route("Users")]
		public ActionResult Index()
		{
			var allUsers = _userOperations.GetUserList();
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
			if (_userOperations.UserExists(user.Email))
			{
				ModelState.AddModelError("Email", "This email   " + user.Email + "   already exist!");
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
		public ActionResult EditAuthorizedUser()
		{
			var id = GetIdForAuthorizedUser();
			return RedirectToAction("Edit", new {id});
		}

		[Authorize]
		public ActionResult Edit(int id)
		{
			if (GetIdForAuthorizedUser() != id && this.User.IsInRole("User"))
			{
				return View("Index");
			}
			var user = _userOperations.GetUserById(id);
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
			if (_userOperations.UserExists(user.Email))
			{
				ModelState.AddModelError("Email", "This email already using! You can nott edit your email on email like this " + user.Email + " ! Write another email.");
				return View(user);
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

		[Authorize]
		public ActionResult DeleteAuthorizedUser()
		{
			var id = GetIdForAuthorizedUser();
			return RedirectToAction("Delete", new { id });
		}

		[Authorize]
		public ActionResult Delete(int id)
		{
			if (GetIdForAuthorizedUser() != id && this.User.IsInRole("User"))
			{
				return View("Index");
			}
			var user = _userOperations.GetUserById(id);
			return View(user);
		}

		[HttpPost]
        public ActionResult Delete(int id, UserModelWithRole user)
        {
			try
			{
				var activeId = GetIdForAuthorizedUser();
				if (activeId == id || this.User.IsInRole("Admin"))
		        {
					_userOperations.Delete(id);
					if (activeId == id)
					{
						return RedirectToAction("LogOut", "Authorisation");
					}
				}
				return RedirectToAction("Index");
	        }
	        catch
	        {
		        return View();
	        }
		}

        [Authorize]
        public ActionResult ResetPasswordAuthorizedUser()
        {
	        var id = GetIdForAuthorizedUser();
	        return RedirectToAction("ResetPassword", new { id });
        }

		[Authorize]
		public ActionResult ResetPassword(int id)
		{
			if (GetIdForAuthorizedUser() != id && this.User.IsInRole("User"))
			{
				return View("Index");
			}
			var user = _userOperations.GetUserEntity(id);
			if (user == null)
			{
				throw new ArgumentException("User does'nt exist in database!");
			}
			var userForChange = new UserModelWithPassword()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Password = _userOperations.GetDecodePassword(user.Password, user.Salt),
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

        private int GetIdForAuthorizedUser()
        {
	        var email = ((ClaimsIdentity)this.User.Identity)
		        .FindFirst(ClaimTypes.Email)?.Value;
	        var user = _userOperations.GetUserList()
		        .FirstOrDefault(em => em.Email == email);
	        return user != null ? user.Id : -1;
        }
	}
}
