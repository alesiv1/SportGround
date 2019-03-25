using Microsoft.AspNet.Identity;
using SportGround.BusinessLogic.Enums;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SportGround.Web.Controllers
{
    public class AuthorisationController : Controller
    {
	    private IUserOperations _userOperations;

	    public AuthorisationController(IUserOperations operations)
	    {
		    _userOperations = operations;
	    }

		public ActionResult Login(string returnUrl)
	    {
		    return View();
	    }

		[HttpPost]
		public ActionResult Login(LogInModel model, string returnUrl)
        {
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = _userOperations.GetAll()
				.FirstOrDefault(m => m.FirstName == model.Login && m.Password == model.Password);

			if (user != null)
			{
				var identity = new ClaimsIdentity(new[] {
						new Claim(ClaimTypes.Name, user.FirstName),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Role, user.Role.ToString())
					},
					DefaultAuthenticationTypes.ApplicationCookie);

				var ctx = Request.GetOwinContext();
				var authManager = ctx.Authentication;

				authManager.SignIn(identity);

				return Redirect(GetRedirectUrl(returnUrl));
			}

			ModelState.AddModelError("", "Invalid email or password");
			return View();
		}

		public ActionResult LogOut()
		{
			var ctx = Request.GetOwinContext();
			var authManager = ctx.Authentication;

			authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
			return RedirectToAction("Login", "Authorisation");
		}

		public ActionResult Registration()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Registration(FormCollection collection)
        {
	        if (!ModelState.IsValid)
	        {
		        return RedirectToAction("Registration", "Authorisation");
			}

			var user = GetUser(collection);
	        if (!_userOperations.GetAll().Any(s => s.Email == user.Email && s.FirstName == user.FirstName) ||
	            user.Password != user.ConfirmPassword)
	        {
		        _userOperations.Create(user);
		        return RedirectToAction("Index", "User");
			}
			return RedirectToAction("Registration", "Authorisation");
		}

        private string GetRedirectUrl(string returnUrl)
        {
	        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
	        {
		        return Url.Action("Index", "User");
	        }

	        return returnUrl;
        }

        private UserRegistrationModel GetUser(FormCollection collectio)
        {
	        int id = Convert.ToInt32(Request.Form["Id"]);
	        string firstName = Convert.ToString(Request.Form["FirstName"]);
	        string lastName = Convert.ToString(Request.Form["LastName"]);
	        string role = Convert.ToString(Request.Form["Role"]) ?? "User";
	        string email = Convert.ToString(Request.Form["Email"]);
	        string password = Convert.ToString(Request.Form["Password"]);
	        var confirmPassword = Convert.ToString(Request.Form["ConfirmPassword"]);
			return new UserRegistrationModel()
	        {
		        Id = id,
		        FirstName = firstName,
		        LastName = lastName,
		        Email = email,
		        Role = role == "Admin" ? UserRole.Admin : UserRole.User,
				Password = password,
				ConfirmPassword = confirmPassword
	        };
        }
	}
}