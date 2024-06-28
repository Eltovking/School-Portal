using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.ViewModels;

namespace School_Portal.Controllers
{
	public class AdminController : Controller
	{
        private readonly AppDbContext db;
        private readonly IUserHelper _userHelper;
        public AdminController(AppDbContext dbContext ,IUserHelper userHelper)
		{
			db = dbContext;
			_userHelper = userHelper;	
		}
        public IActionResult Dashboard()
		{
			var adminDataViewModel = new AdminDataViewModel();
			var userCount = _userHelper.GetUsers().Count();
			var courseCount = _userHelper.GetCourses().Count();
			var categoryCount = _userHelper.GetCourseCategories().Count();

			adminDataViewModel.CourseCategoryCount = categoryCount;
			adminDataViewModel.UserCount = userCount;
			adminDataViewModel.CousreCount = courseCount;
			return View(adminDataViewModel);
		}
		[HttpGet]
		public IActionResult Profile(string id)
		{
			if (id != null)
			{
				var user = _userHelper.GetUserById(id);
				if (user != null) 
				{
					return View(user);
				}
            }
			return View();
		}

	}
}
