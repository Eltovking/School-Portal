using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.ViewModels;
using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.Controllers
{
	public class UserController : Controller
	{
		private readonly AppDbContext db;
		private readonly IUserHelper _userHelper;
		private readonly UserManager<ApplicationUser> _userManager;
		public UserController(AppDbContext dbContext, IUserHelper userHelper, UserManager<ApplicationUser> userManager)
		{
			db = dbContext;
			_userHelper = userHelper;
			_userManager = userManager;
		}
		public IActionResult Index()
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

		public IActionResult StudentCourses()
		{
            var courses = db.Courses
               .Where(x => x.Name != null && x.IsActive && x.Status == CourseStatus.Approved)
               .Include(v => v.CourseCategory)
               .Include(y => y.Teacher)
               .ToList();
            return View(courses);
        }
		public async Task<IActionResult> Payment()
		{
			var loggedInUser = _userHelper.GetUserByUserName(User.Identity.Name);
            var paymentModels = await _userHelper.GetLoggedInUserPaymentList(loggedInUser.Id);
				if (paymentModels.Any())
				{
					return View(paymentModels);
				}
				return View();
		}
		public IActionResult CourseMaterials()
		{
            var user = _userHelper.GetUserByUserName(User.Identity.Name );
            var courses = db.PaymentModels
              .Where(x => x.StudentId == user.Id && x.IsActive && x.IsApproved == true)
              .Include(v => v.Course!.CourseCategory)
              .Include(y => y.Student).Select(c=>c.Course)
              .ToList();
            return View(courses);
		}
		public IActionResult PaymentForm(int courseId)
		{
            var loggedInUser = _userHelper.GetUserByUserName(User.Identity.Name);

            if (courseId != 0)
			{
                var boughtBefore = db.PaymentModels.Where(x => x.StudentId == loggedInUser.Id && x.CourseId == courseId && x.PaymentStatus != PaymentStatus.Decline && x.IsActive)
					.FirstOrDefault();
                if (boughtBefore != null)
                {
                    TempData["ErrorMessage"] = "You have bought this course before,You can't buy muiltiple times";
                    return RedirectToAction("StudentCourses");
                }
                
                var model = new PaymentViewModel
                {
                    CourseId = courseId,
                };
                return View(model);
            }
			return View();
		}
		[HttpGet]
		public IActionResult Profile()
		{
			string? currentLoggedInUsername = User?.Identity?.Name;

			var user = _userHelper.GetUserByUserName(currentLoggedInUsername);
			if (user != null)
			{
				return View(user);
			}
			return View();
		}
		public IActionResult ViewProfile()
		{
			string? currentLoggedInUsername = User?.Identity?.Name;

			var user = _userHelper.GetUserByUserName(currentLoggedInUsername);
			if (user != null)
			{
				return View(user);
			}
			return View();
		}
		[HttpPost]
        public IActionResult Profile(ApplicationUser UserDetail)
        {
            if (UserDetail == null)
            {
                return View();
            }
            else
            {
                string? currentLoggedInUsername = User?.Identity?.Name;
                var hasProfileBeenUpdated = _userHelper.UpdateProfile(UserDetail, currentLoggedInUsername);
                if (hasProfileBeenUpdated)
                {
                    return RedirectToAction("ViewProfile");
                }
            }
            return View(UserDetail);
        }
		public IActionResult AnnouncementDetails()
		{
            var listofAnnouncement = _userHelper.ListofAnnouncement();
            return View(listofAnnouncement);
        }
    }
}
