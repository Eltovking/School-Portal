using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.ViewModels;

namespace School_Portal.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly AppDbContext db;
		private readonly IUserHelper _userHelper;
		public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AppDbContext dbContext, IUserHelper userHelper)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			db = dbContext;
			_userHelper = userHelper;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}


        [HttpGet]
        public IActionResult RegisterAdmin()
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
		public async Task<JsonResult> RegisterAdmin(string userDetails)
		{
			if (userDetails == null)
			{
				return Json(new { isError = true, msg = "Your information is required" });
			}
			var applicationUser = JsonConvert.DeserializeObject<ApplicationViewModel>(userDetails);
			if(applicationUser != null)
			{
                var checkForEmail = await _userHelper.FindByEmailAsync(applicationUser.Email).ConfigureAwait(false);
                if (checkForEmail != null)
                {
                    return Json(new { isError = true, msg = "The email you entered is already in use.Please try logging in. " });
                }
                var createStaff = await _userHelper.CreateAdminDetails(applicationUser).ConfigureAwait(false);
                if (createStaff != null)
                {
                    return Json(new { isError = false, msg = "Admin created successfully" });
                }
            }
			return Json(new { isError = true, msg = "Unable to Register" });
		}

		[HttpPost]
		public async Task<JsonResult> RegisterStudent(string userDetails)
		{

			if (userDetails == null)
			{
				return Json(new { isError = true, msg = "Your information is required" });
			}
			var applicationUser = JsonConvert.DeserializeObject<ApplicationViewModel>(userDetails);
			if (applicationUser != null)
			{
				var checkForEmail = await _userHelper.FindByEmailAsync(applicationUser.Email).ConfigureAwait(false);
				if (checkForEmail != null)
				{
					return Json(new { isError = true, msg = "The email you entered is already in use.Please try logging in." });
				}
				var createStudent = await _userHelper.CreateStudentDetails(applicationUser).ConfigureAwait(false);
				if (createStudent != null)
				{
					return Json(new { isError = false, msg = "Student created successfully" });
				}
			}
			return Json(new { isError = true, msg = "Unable to Register" });
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
						var url = "";
						var userRoles = _userManager.GetRolesAsync(user).Result;
						if (userRoles != null)
						{
							if (userRoles.Contains("Admin"))
							{
								url = "/Admin/Dashboard";
							}
							else
							{
								url = "/User/Index";
							}
						}
                        return Json(new { isError = false, dashboard = url });
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
