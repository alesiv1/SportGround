using System;
using Microsoft.AspNet.Identity;
using SportGround.BusinessLogic.Interfaces;
using SportGround.Web.Models;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using SportGround.BusinessLogic.Models;

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
			var user = _userOperations.Users()
				.FirstOrDefault(m => m.Email == model.Email);
			if (user != null)
			{
				var pass = _userOperations.GetPasswordHashCode(model.Password, user.Salt, 0);
				//d2AWUBBWhQgyHRJRvzEumw==
				if (user.Password != pass)
				{
					ModelState.AddModelError("Password", "Invalid password. Chack your password and try again!");
					return View();
				}
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
			ModelState.AddModelError("Email", "Invalid email. Chack your email and try again!");
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
		public ActionResult Registration(UserModelWithPassword user)
        {
	        if (!ModelState.IsValid)
	        {
		        return RedirectToAction("Registration", "Authorisation");
			}
	        try
	        {
		        _userOperations.Create(user);
		        return RedirectToAction("Index", "User");
	        }
	        catch { }
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
	}
}