using System;
using Microsoft.AspNet.Identity;
using SportGround.BusinessLogic.Interfaces;
using SportGround.Web.Models;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using SportGround.BusinessLogic.Models;

namespace SportGround.Web.Controllers
{
    public class AuthorisationController : Controller
    {
	    private readonly IUserService _userServices;

	    public AuthorisationController(IUserService service)
	    {
		    _userServices = service;
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
			var user = _userServices.GetUserByEmail(model.Email);
			if (user == null && model.Email == "admin@admin.com" && model.Password == "admin1")
			{
				_userServices.CreateDefaultUser();
				user = _userServices.GetUserByEmail(model.Email);
			}
			if (user != null)
			{
				var pass = _userServices.GetPasswordHashCode(model.Password, user.Salt);
				if (user.Password != pass)
				{
					ModelState.AddModelError("Password", "Invalid password. Chack your password and try again!");
					return View();
				}
				var identity = new ClaimsIdentity(new[] {
						new Claim("Id", user.Id.ToString()),
						new Claim(ClaimTypes.Email, user.Email),
						new Claim(ClaimTypes.Name, user.FirstName),
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
	        if (_userServices.UserExists(user.Email))
	        {
		        ModelState.AddModelError("Email", "This email  " + user.Email + "  already exist!");
		        return View();
			}
			try
	        {
		        _userServices.Create(user);
		        var newUser = _userServices
			        .GetUserByEmail(user.Email);
		        var identity = new ClaimsIdentity(new[] {
				        new Claim("Id", newUser.Id.ToString()),
						new Claim(ClaimTypes.Email, newUser.Email),
				        new Claim(ClaimTypes.Name, newUser.FirstName),
				        new Claim(ClaimTypes.Role, newUser.Role.ToString())
			        },
					DefaultAuthenticationTypes.ApplicationCookie);
		        var ctx = Request.GetOwinContext();
		        var authManager = ctx.Authentication;
		        authManager.SignIn(identity);
		        return Redirect(GetRedirectUrl(null));
	        }
	        catch
	        {
		        return RedirectToAction("Registration", "Authorisation");
			}
		}

        private string GetRedirectUrl(string returnUrl)
        {
	        if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
	        {
		        if (this.User.IsInRole("Admin"))
		        {
			        return Url.Action("Index", "User");
				}
	        }
			return Url.Action("Profile", "User");
		}
	}
}