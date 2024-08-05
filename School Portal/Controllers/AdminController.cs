using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.ViewModels;

namespace School_Portal.Controllers
{
	public class AdminController : Controller
	{
        private readonly AppDbContext db;
        private readonly IUserHelper _userHelper;
		private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(AppDbContext dbContext ,IUserHelper userHelper, UserManager<ApplicationUser> userManager)
		{
			db = dbContext;
			_userHelper = userHelper;	
			_userManager = userManager;
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
				var hasProfileBeenUpdated = _userHelper.UpdateProfile( UserDetail, currentLoggedInUsername);
                if (hasProfileBeenUpdated)
                {
                    return RedirectToAction("ViewProfile");
                }
			}
            return View(UserDetail);
          
		}

        [HttpGet]
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


		[HttpGet]
		public IActionResult Teacher()
		{
            var teachers =  _userHelper.GetTeacher2();
            if (teachers != null)
			{
				return View(teachers);
			}
			return View();
		}

        public async Task<JsonResult> RegisterTeacher(string teacherDetails)
        {
            if (teacherDetails != null)
            {
                var applicationUser = JsonConvert.DeserializeObject<ApplicationViewModel>(teacherDetails);
                if (applicationUser != null)
                {
                    var checkForEmail = await _userHelper.FindByEmailAsync(applicationUser.Email).ConfigureAwait(false);
                    if (checkForEmail != null)
                    {
                        return Json(new { isError = true, msg = "Email already exists" });
                    }
                        
                    var createStaff = await _userHelper.CreateTeacherDetails(applicationUser).ConfigureAwait(false);
                    if (createStaff != null)
                    {

                        return Json(new { isError = false, msg = "Teacher created successfully" });
                    }
                }
            }
            return Json(new { isError = true, msg = "Something went wrong" });
        }
        [HttpGet]
        public JsonResult EditTeacher(string userId)
        {
            if (userId != null)
            {
                var teacher = _userHelper.GetTeacherById(userId);
                if (teacher != null)
                {
                    return Json(new { isError = false, data = teacher });
                }
            }
            return Json(new { isError = true, msg = "Not find" });
        }

        [HttpPost]
        public JsonResult EditedTeacher(string teacherData)
        {
            if (teacherData == string.Empty)
            {
                return Json(new { isError = true, msg = "Not found" });
            }
            var editedTeacher = JsonConvert.DeserializeObject<ApplicationViewModel>(teacherData);
            if (editedTeacher != null)
            {
                var teacher = db.ApplicationUser.FirstOrDefault(r => r.Id == editedTeacher.Id);
                if (teacher != null)
                {
                    teacher.FirstName = editedTeacher.FirstName;
                    teacher.LastName = editedTeacher.LastName;
                    teacher.Email = editedTeacher.Email;
                    teacher.PhoneNumber = editedTeacher.PhoneNumber;
                    teacher.Address = editedTeacher.Address;

                    db.Update(teacher);
                    db.SaveChanges();
                    return Json(new { isError = false, msg = "Teacher details edited successfully" });
                }
            }
            return Json(new { isError = true, msg = "Unable to Edit" });
        }

        [HttpPost]
        public JsonResult deleteTeacherById(string id)
        {
            if (id == null )
            {
                return Json(new { isError = true, msg = "Not found" });
            }
            var teacher = db.ApplicationUser.Where(y => y.Id == id && !y.IsDeactiveted).FirstOrDefault();
            if (teacher != null)
            {
                teacher.IsDeactiveted = true;
                db.ApplicationUser.Update(teacher);
                db.SaveChanges();
                return Json(new { isError = false, msg = "Deleted Successfully." });
            }
            return Json(new { isError = true, msg = "Not found" });
        }
        [HttpGet]
		public IActionResult Payments()
		{
			var paymentModels = _userHelper.GetPendingPaymentList();
			if (paymentModels.Any())
			{
				return View(paymentModels);
			}
			return View();
		}
        [HttpPost]
        public JsonResult ApprovedCoursePayment(int id)
        {
            if (id < 1)
            {
                return Json(new { isError = true, msg = "No course purchased" });
            }
            var paidCourse = db.PaymentModels.Where(p => p.Id == id && p.PaymentStatus == SchoolPortalEnums.PaymentStatus.Pending).FirstOrDefault();
            if (paidCourse != null)
            {
                paidCourse.IsApproved = true;
                paidCourse.IsActive = true;

                paidCourse.PaymentStatus = SchoolPortalEnums.PaymentStatus.Approve;
                paidCourse.StatusChangeDate = DateTime.Now;
                db.Update(paidCourse);
                db.SaveChanges();
                return Json(new { isError = false, msg = "Approved" });
            }
            return Json(new{ isError = true, msg = " Payment Details Not Fount"});
        }
        public JsonResult DeclinePayment(int id)
        {
            if (id < 1)
            {
                return Json(new { isError = true, msg = "No course purchased" });
            }
            var paidCourse = db.PaymentModels.Where(p => p.Id == id && p.PaymentStatus == SchoolPortalEnums.PaymentStatus.Pending).FirstOrDefault();
            if (paidCourse != null)
            {
                paidCourse.PaymentStatus = SchoolPortalEnums.PaymentStatus.Decline;
                paidCourse.StatusChangeDate= DateTime.Now;
                db.Update(paidCourse);
                db.SaveChanges();
                return Json(new { isError = false, msg = "Payment decline successfully" });
            }
            return Json(new { isError = true, msg = " Payment Details Not Found" });
        }

        public IActionResult paymentHistory()
        {
            var paymentModels = _userHelper.GetPaymentHistory();
            if (paymentModels.Any())
            {
                return View(paymentModels);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Announcement()
        {
            var listofAnnouncement = _userHelper.ListofAnnouncementForAdmin();
            return View(listofAnnouncement);
        }

        public JsonResult CreateAnnouncement(string details)
        {
            if (details != null)
            {
                var loggedInUser = _userHelper.GetUserByUserName(User.Identity.Name);
                var announcementDetails = JsonConvert.DeserializeObject<AnnouncementViewModel>(details);
                if (announcementDetails != null)
                {
                    if (announcementDetails.DurationTill < announcementDetails.DurationFrom)
                    {
                        return Json(new { isError = true, msg = "Add the correct duration" });
                    }

                    if (announcementDetails.Description == "")
                    {
                        return Json(new { isError = true, msg = "Please add announcement detail" });
                    }

                    if (announcementDetails.Title == "")
                    {
                        return Json(new { isError = true, msg = "Please add announcement Title" });
                    }

                    var announcement = _userHelper.AddAnnouncements(announcementDetails, loggedInUser);
                    if (announcement)
                    {
                        return Json(new { isError = false, msg = "Announcement added successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable to add " });
                }
            }
            return Json(new { isError = true, msg = "Network Failure" });
        }

        public JsonResult GetAnnounceToEdit(int id)
        {
            if (id > 0)
            {
                var currentAnnouncement = _userHelper.GetAnnouncement(id);
                if (currentAnnouncement != null)
                {
                    return Json(currentAnnouncement);
                }
                return Json(new { isError = true, msg = " Could not get Annoucement" });
            }
            return Json(new { isError = true, msg = "Network Failure" });
        }

        public JsonResult SaveEditedAnnouncement(string announcedetails)
        {
            if (announcedetails != null)
            {
                var loggedInUser = _userHelper.GetUserByUserName(User.Identity.Name);
                var announcementDetails = JsonConvert.DeserializeObject<AnnouncementViewModel>(announcedetails);
                if (announcementDetails != null)
                {
                    if (announcementDetails.DurationTill < announcementDetails.DurationFrom)
                    {
                        return Json(new { isError = true, msg = "Add the correct durations" });
                    }
                    var editannouncement = _userHelper.EditAnnouncement(announcementDetails, loggedInUser);
                    if (editannouncement)
                    {
                        return Json(new { isError = false, msg = "Announcement Edited successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable to Edit " });
                }
            }
            return Json(new { isError = true, msg = "Network Failure" });
        }
        public JsonResult DelAnnounce(int id)
        {
            if (id > 0)
            {
                var delete = _userHelper.DeleteAnnounce(id);
                if (delete)
                {
                    return Json(new { isError = false, msg = "Announcement Deleted sucessfully" });
                }
            }
            return Json(new { isError = true, msg = "Announcement not found" });
        }
    }
}
