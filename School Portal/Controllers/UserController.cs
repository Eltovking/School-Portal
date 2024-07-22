using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
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
			return View();
		}

		public IActionResult StudentCourses()
		{
            var courses = db.Courses
               .Where(x => x.Name != null && x.IsActive && x.Status == CourseStatus.Approve)
               .Include(v => v.CourseCategory)
               .Include(y => y.Teacher)
               .ToList();
            return View(courses);
        }
		public IActionResult Payment()
		{
			var loggedInUser = _userHelper.GetUserByUserName(User.Identity.Name);

            var paymentModels = _userHelper.GetPaymentList(loggedInUser.Id);
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
              .Where(x => x.StudentId == user.Id && x.IsActive && x.IsApproved)
              .Include(v => v.Course!.CourseCategory)
              .Include(y => y.Student).Select(c=>c.Course)
              .ToList();
            return View(courses);
		}
	}
}
