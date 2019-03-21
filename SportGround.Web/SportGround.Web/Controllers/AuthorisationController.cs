using Microsoft.AspNet.Identity;
using SportGround.BusinessLogic.Interfaces;
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

		public ActionResult Login()
	    {
		    return View();
	    }

		[HttpPost]
		public ActionResult Login(LogInModel model)
        {
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = _userOperations.GetAll().FirstOrDefault(i => i.FirstName == model.Login);

			if (user != null && model.Password == "password")
			{
				var identity = new ClaimsIdentity(new[] {
						new Claim(ClaimTypes.Name, user.FirstName),
						new Claim(ClaimTypes.Email, "test@test.gmail"),
						new Claim(ClaimTypes.Country, "Ukrain")
					},
					DefaultAuthenticationTypes.ApplicationCookie);

				var ctx = Request.GetOwinContext();
				var authManager = ctx.Authentication;

				authManager.SignIn(identity);

				return Redirect(GetRedirectUrl(""));
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

        private string GetRedirectUrl(string returnUrl)
        {
	        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
	        {
		        return Url.Action("Index", "Court");
	        }

	        return returnUrl;
        }
	}
}