using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using School_Portal.Data;
using School_Portal.Models;
using School_Portal.ViewModels;

namespace School_Portal.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly AppDbContext db;
		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AppDbContext dbContext)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			db = dbContext;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		//[HttpPost]
		//public IActionResult RegisterUser(ApplicationUser data)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return RedirectToAction("Register");
		//	}
		//	else
		//	{
		//		data.UserName = data.Email;
		//		data.DateRegistered = DateTime.Now;
		//		var registeredUser = _userManager.CreateAsync(data, data.Password).Result;
		//		if (registeredUser.Succeeded)
		//		{
		//			return RedirectToAction("Index", "Admin");
		//		}
		//		return RedirectToAction("Register");

		//	}
		//}


		[HttpPost]
		public JsonResult Register(string userDetails)
		{
			if (userDetails == null)
			{
				return Json(new { isError = true, msg = "Your information is required" });
			}
			var applicationUserViewModel = JsonConvert.DeserializeObject<ApplicationViewModel>(userDetails);
				
			var application =new ApplicationUser();
			application.Email = applicationUserViewModel.Email;
			application.PhoneNumber = applicationUserViewModel.PhoneNumber;
			application.LastName = applicationUserViewModel.LastName;
			application.FirstName = applicationUserViewModel.FirstName;
			application.UserName = applicationUserViewModel.Email;

            var userManager =  _userManager.CreateAsync(application, applicationUserViewModel.Password).Result;
			if (userManager.Succeeded)
			{
				return Json(new { isError = false, msg = "Registration successful" });
			}
			else
			{
				return Json(new { isError = true, msg = "Unable to Register" });
			}
		}



		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		// using Javascript we make use of JsonResult while IActionResult is for asp.Net
		public JsonResult Login(string email, string password)
		{
			if (email == null && password == null)
			{
				return Json(new { isError = true, msg = "Email and password is required" });
			}
			else
			{
				var user = db.ApplicationUser.Where(x => x.Email == email).FirstOrDefault();
				if (user == null)
				{
                    return Json(new { isError = true, msg = "No user found" });
                }
				else
				{
					// here i set the ispersistent to be true and the lockoutOnFailure to be false
					var userSignIn = _signInManager.PasswordSignInAsync(user, password, true, false).Result;
					if (userSignIn.Succeeded)
					{
                        return Json(new { isError = false });
                    }
					else
					{
                        return Json(new { isError = true, msg = "Unable to sign in" });
                    }
					
				}
			}
		}
		public IActionResult Logout()
		{
			 _signInManager.SignOutAsync();
			return RedirectToAction("Index","Home");
		}
	}
}
