using SportGround.BusinessLogic.Interfaces;
using System;
using System.Web.Mvc;
using SportGround.BusinessLogic.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using FluentValidation.Results;
using SportGround.BusinessLogic.Validations;

namespace SportGround.Web.Controllers
{
    public class UserController : Controller
    {
	    private IUserService _userServices;
	    private UserValidation userValid = new UserValidation();
	    private UserWithPasswordValidation userwithRoleValid = new UserWithPasswordValidation();

		public UserController(IUserService services)
	    {
		    _userServices = services;
	    }

		[Authorize]
		[Route("Profile")]
		public ActionResult Profile()
		{
			var user = _userServices.GetUserById(GetIdForAuthorizedUser());
			return View(user);
		}

		[Authorize(Roles = "Admin")]
		[Route("Users")]
		public ActionResult Index()
		{
			var users = _userServices.GetUserList();
			return View(users);
		}

		[Authorize(Roles = "Admin")]
		public ActionResult Details(int id)
		{
			var user = _userServices.GetUserById(id);
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
			var validationResult = userwithRoleValid.Validate(user);
			if (!validationResult.IsValid)
			{
				foreach (ValidationFailure data in validationResult.Errors)
				{
					ModelState.AddModelError(data.PropertyName, data.ErrorMessage);
				}
				return View(user);
			}
			if (_userServices.UserExists(user.Email))
			{
				ModelState.AddModelError("Email", "This email   " + user.Email + "   already exist!");
				return View();
			}
			_userServices.Create(user);
			return RedirectToAction("Index");
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
			var user = _userServices.GetUserById(id);
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
			var validationResult = userValid.Validate(user);
			if (!validationResult.IsValid)
			{
				foreach (ValidationFailure data in validationResult.Errors)
				{
					ModelState.AddModelError(data.PropertyName, data.ErrorMessage);
				}
				return View(user);
			}
			var userEmail = _userServices.GetUserById(id)?.Email;
			if (_userServices.UserExists(user.Email) && userEmail != user.Email)
			{
				ModelState.AddModelError("Email", "This email already using! You can nott edit your email on email like this " + user.Email + " ! Write another email.");
				return View(user);
			}
			_userServices.Update(id, user);
			return this.User.IsInRole("Admin") ? RedirectToAction("Index") : RedirectToAction("Profile");
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
			var user = _userServices.GetUserById(id);
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
					_userServices.Delete(id);
					if (activeId == id)
					{
						return RedirectToAction("LogOut", "Authorisation");
					}
				}
				return this.User.IsInRole("Admin") ? RedirectToAction("Index") : RedirectToAction("Profile");
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
			var user = _userServices.GetUserWithPassword(id);
			if (user == null)
			{
				return View("Index");
			}
			return View(user);
		}

		[Authorize]
        [HttpPost]
        public ActionResult ResetPassword(int id, UserModelWithPassword user)
        {
			try
	        {		        
		        _userServices.Update(id, user);
		        return this.User.IsInRole("Admin") ? RedirectToAction("Index") : RedirectToAction("Profile");
			}
	        catch
	        {
		        return View();
	        }
		}

        [Authorize(Roles = "Admin")]
		public ActionResult ChangeRole(int id)
        {
			var user = _userServices.GetUserById(id);
			return View(user);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public ActionResult ChangeRole(int id, UserModelWithRole user)
        {
			try
			{
				_userServices.Update(id, user);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

        private int GetIdForAuthorizedUser()
        {
	        try
	        {
		        return Int32.Parse(((ClaimsIdentity) this.User.Identity).FindFirstValue("Id"));
	        }
	        catch
	        {
		        return -1;
	        }
        }
	}
}
