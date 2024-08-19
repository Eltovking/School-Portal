using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.ViewModels;
using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.Services
{
    public class UserHelper : IUserHelper
    {
		private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext db;
        public UserHelper(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
		{
			_userManager = userManager;
			db = dbContext;
		}

		//Return Type
		//Method name
		//parameters

		public ApplicationUser? GetUserById(string userId)
		{
			if (string.IsNullOrEmpty(userId))
			{
				return null;
			}
			else
			{
				var user = db.ApplicationUser.Where(x=>x.Id == userId).FirstOrDefault();
				return user;
			}
		}

		public ApplicationUser? GetUserByEmail(string email)
		{
			if(string.IsNullOrEmpty(email))
			{
				return null;
			}
			else
			{
				var user = db.ApplicationUser.Where(x=>x.Email == email).FirstOrDefault();
				return user;
			}
		}

		public ApplicationUser? GetUserByPhoneNumber(string PhoneNumber) 
		{
			if(PhoneNumber == null)
			{
				return null;
			}
			else
			{
				var user = db.ApplicationUser.Where(x=>x.PhoneNumber == PhoneNumber).FirstOrDefault();
				return user;
			}
		}
		//this the method of how to get list items from Db
		public List<CourseCategory> GetCourseCategories()
		{
			var courses = new List<CourseCategory>();
			//var defaultVaule = new CourseCategory
			//{
			//	Id = 0,
			//	Name = "Select Category"
			//};
            courses = db.CourseCategories.Where(s => s.Id > 0 && s.IsActive == true).ToList();
			//courses.Insert(0, defaultVaule);
			return courses;
		}

		public List<ApplicationUser> GetTeacher()
		{
			var courses = new List<ApplicationUser>();
			var defaultVaule = new ApplicationUser
			{
				Id = "",
				FirstName = "Select"
			};
			//courses = db.ApplicationUser.Where(s => s.Id != null && s.IsDeactiveted == false && s.RoleName == "Teacher").ToList();
            courses = db.ApplicationUser.Where(s => s.Id != null && s.RoleName == "Teacher" && !s.IsDeactiveted).ToList();
            courses.Insert(0, defaultVaule);
			return courses;
		}

		public List<Course> GetCoursesDropDown()
		{
			var courses = new List<Course>();
			var defaultVaule = new Course
			{
				Id = 0,
				Name = "Select"
			};
			courses = db.Courses.Where(s => s.Id != null && s.IsActive).ToList();
			
			courses.Insert(0, defaultVaule);	
			return courses;
		}
		public List<ApplicationUser> GetTeacher2()
        {
            var courses = new List<ApplicationUser>();
            
            //courses = db.ApplicationUser.Where(s => s.Id != null && s.IsDeactiveted == false && s.RoleName == "Teacher").ToList();

            courses = db.ApplicationUser.Where(s => s.Id != null && !s.IsDeactiveted && s.RoleName == "Teacher").ToList();
            return courses;
        }
        public bool ApproveCourse(int courseId)
		{
			if(courseId > 0)
			{
				var course = db.Courses.Where(y => y.Id == courseId && y.IsActive).FirstOrDefault();
				if(course != null)
				{
					course.Status = CourseStatus.Approved;
					db.Update(course);
					db.SaveChanges();
					return true;
				}
			}
			return false;
			
		}
		public bool RemoveCourse(int courseId)
		{
			if(courseId > 0)
			{
				var course = db.Courses.Where(x => x.Id == courseId && x.IsActive).FirstOrDefault();
				if(course != null)
				{
					course.Status = CourseStatus.Declined;
					db.Update(course);
					db.SaveChanges();
					return true;
				}
			}
			return false;
		}
		public HomeViewModel GetHomeData()
		{
			var homeData = new HomeViewModel();
            homeData.Courses = db.Courses
				.Where(x => x.IsActive && x.Id != 0 && (x.Image != "" || x.Image != null) 
				&& x.CategoryId != null && x.Status == CourseStatus.Approved)
				.Include(w => w.CourseCategory)
				.OrderByDescending(x=>x.CreatedDate)
				.Select(course => new CourseViewModelEdit
				{
                    Id = course.Id,
					Name = course.Name,
					Price = course.Price,
					CategoryId = course.CategoryId,
					Description = course.Description,
					CategoryName = course.CourseCategory != null ? course.CourseCategory.Name : "",
					Image = course.Image,
					Date = course.CreatedDate.ToShortDateString(),
				}).Take(5).ToList();
            return homeData;
		}

		public List<Course> GetCourses()
		{
			var course = new List<Course>();
			course = db.Courses.Where(x => x.IsActive ).ToList();
			if (course != null)
			{
				return course;
			}
			return (course); 
		}

		public List<ApplicationUser> GetUsers()
		{
			var users = new List<ApplicationUser>();
			users = db.ApplicationUser.Where(u => u.IsDeactiveted == false).ToList();
			if (users != null)
			{
				return users;
			}
			return users;
		}
		public List<CourseCategory> GetCourseCategory()
		{
			var categories = new List<CourseCategory>();
			categories = db.CourseCategories.Where(x => x.IsActive).ToList();
			if (categories != null)
			{
				return categories;
			}
			return categories;
		}
        public async Task<ApplicationUser>? FindByEmailAsync(string email)
        {
            try
            {
                var user = await db.ApplicationUser
                    .Where(s => s.Email == email && !s.IsDeactiveted)
                    .FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    return user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
        public async Task<ApplicationUser> CreateTeacherDetails(ApplicationViewModel userModel)
        {
            var user = new ApplicationUser();
            user.UserName = userModel.Email;
            user.Email = userModel.Email;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.PhoneNumber = userModel.PhoneNumber;
            user.RoleName = "Teacher";
            user.DateRegistered = DateTime.Now;
			user.Address = userModel.Address;
            user.IsDeactiveted = false;
            var createdUser = await _userManager.CreateAsync(user, userModel.Password).ConfigureAwait(false);
            if (createdUser.Succeeded)
            {
               var isAddedToRole = await _userManager.AddToRoleAsync(user, "Teacher");
				if (isAddedToRole.Succeeded)
				{
                    return user;
                }
                return user;
            }
           return null;
        }
		
		
		
		public async Task<ApplicationUser> CreateAdminDetails(ApplicationViewModel userModel)
        {
            var user = new ApplicationUser();
            user.UserName = userModel.Email;
            user.Email = userModel.Email;
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.PhoneNumber = userModel.PhoneNumber;
            user.DateRegistered = DateTime.Now;
			user.Address = userModel.Address;
            user.IsDeactiveted = false;
			user.RoleName = "Admin";
            var createdUser = await _userManager.CreateAsync(user, userModel.Password).ConfigureAwait(false);
            if (createdUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return user;
            }
           return null;
        }

		public async Task<ApplicationUser> CreateStudentDetails(ApplicationViewModel userModel)
		{
			try
			{
				var user = new ApplicationUser();
				user.UserName = userModel.Email;
				user.Email = userModel.Email;
				user.FirstName = userModel.FirstName;
				user.LastName = userModel.LastName;
				user.PhoneNumber = userModel.PhoneNumber;
				user.DateRegistered = DateTime.Now;
				user.Address = userModel.Address;
				user.IsDeactiveted = false;
				user.RoleName = "User";
				user.DOB = userModel.DOB;
				user.StateOfOrigin = userModel.StateOfOrigin;

				var createdUser = await _userManager.CreateAsync(user, userModel.Password).ConfigureAwait(false);

				if (createdUser.Succeeded)
				{
					var roleResult = await _userManager.AddToRoleAsync(user, "User");

					if (roleResult.Succeeded)
					{
						return user;
					}

					return user;
				}
				
				return null;
			}
			catch (Exception ex)
			{
				throw new Exception("An error occurred while creating the user.", ex);
			}
		}

		public async Task<ApplicationUser> CreateStudentDetailss(ApplicationViewModel userModel)
		{
			var user = new ApplicationUser();
			user.UserName = userModel.Email;
			user.Email = userModel.Email;
			user.FirstName = userModel.FirstName;
			user.LastName = userModel.LastName;
			user.PhoneNumber = userModel.PhoneNumber;
			user.DateRegistered = DateTime.Now;
			user.Address = userModel.Address;
			user.IsDeactiveted = false;
			user.RoleName = "User";
			user.DOB = userModel.DOB;
			user.StateOfOrigin = userModel.StateOfOrigin;
			var createdUser = await _userManager.CreateAsync(user, userModel.Password).ConfigureAwait(false);

			if (createdUser.Succeeded)
			{
				var roleResult = await _userManager.AddToRoleAsync(user, "User");

				if (roleResult.Succeeded)
				{
					return user;
				}

				return user;
			}
			return null;
		}
		public List<ApplicationUser> GetTeacherName()
        {
            var courses = new List<ApplicationUser>();
            courses = db.ApplicationUser.Where(s => s.Id != null && s.IsDeactiveted == false && s.RoleName == "Teacher").ToList();
            return courses;
        }

        public async Task<List<ApplicationUser>> GetTeacherNamesAsync()
        {
			try
			{
                var activeUsers = await db.ApplicationUser
                             .Where(s => s.Id != null && !s.IsDeactiveted)
                             .ToListAsync()
                             .ConfigureAwait(false);

                var teachers = new List<ApplicationUser>();

                foreach (var user in activeUsers)
                {
                    var roles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                    if (roles.Contains("Teacher"))
                    {
                        teachers.Add(user);
                    }
                }

                return teachers;
            }
			catch (Exception ex)
			{

				throw ex;
			}
        }

        public ApplicationUser GetTeacherById(string userId)
        {
            var teacher = db.ApplicationUser.Where(s => s.Id == userId && !s.IsDeactiveted && s.RoleName == "Teacher").FirstOrDefault();
            if (teacher != null)
            {
                return teacher;
            }
            return null;
        }


		public ApplicationUser? GetUserByUserName(string? userName)
		{
			if (string.IsNullOrEmpty(userName))
			{
				return null;
			}
			else
			{
				var user = db.ApplicationUser.Where(x => x.UserName == userName).FirstOrDefault();
				return user;
			}
		}

		public bool UpdateProfile(ApplicationUser applicationUserFromForm, string userName)
		{
			try
			{
				if (applicationUserFromForm != null)
				{
					var user = GetUserByUserName(userName);
					if (user != null)
					{
						user.FirstName = applicationUserFromForm.FirstName;
						user.LastName = applicationUserFromForm.LastName;
						user.Email = applicationUserFromForm.Email;
						user.PhoneNumber = applicationUserFromForm.PhoneNumber;
						db.Update(user);
						db.SaveChanges();
						return true;
					}
					return false;
				}
				else
				{
					return false;
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}

		}

        public Course GetCourse(int courseId)
        {
            if (courseId > 0)
            {
                var appView = new Course();
			   appView = db.Courses.Where(a => a.Id == courseId && a.IsActive).Include(x => x.CourseCategory)
            .Select(a => new Course()
            {
                Description = a.Description,
                Image = a.Image,
                Name = a.Name,
				Teacher = a.Teacher, 
                Id = a.Id,
                Price = a.Price,
				CourseCategory = a.CourseCategory,
				CategoryId = a.CategoryId,
                
            }).FirstOrDefault();

                return appView;
            }
            return null;
        } 
		public GenericResponse IntiateCoursePayment(PaymentViewModel paymentViewModel, string username, string base64)
        {
			var result = new GenericResponse();

            var user = GetUserByUserName(username);
			var checkUserPaidCourse = db.PaymentModels.Where(p => p.StudentId == user.Id && p.CourseId == paymentViewModel.CourseId && p.PaymentStatus == PaymentStatus.Pending).Any();// check if user have alerady pay this course previously using username and courseid
			if (checkUserPaidCourse)
			{
				result.Message = "Course is pending";
				result.IsError = true;
				return result;
			}
			if (paymentViewModel != null  && user != null)
            {
				var payment = new PaymentModel();
				payment.StudentId = user.Id;
				payment.CourseId = paymentViewModel.CourseId;
				payment.CreatedDate = DateTime.Now;
				payment.IsActive = false;
                payment.BankPaidFrom = paymentViewModel.BankPaidFrom;
				payment.PaymentMethod = paymentViewModel.PaymentMethod;
				payment.PaymentStatus = PaymentStatus.Pending;
				payment.IsActive = true;
				payment.Image = base64;
				db.Add(payment);
                db.SaveChanges();
				result.Message = "payment submitted successfully";
				result.IsError = false;
                return result;
            }
			result.Message = "payment failed";
            return result;
        }


        public async Task<List<PaymentViewModel>> GetLoggedInUserPaymentList(string userId)
        {
            try
            {
                var paymentViewModels = await db.PaymentModels
                    .Where(x => x.StudentId == userId && x.IsActive )
                    .Include(x => x.Course)
                    .Select(x => new PaymentViewModel
                    {
                        Id = x.Id,
                        CourseName = x.Course != null ? x.Course.Name : "Course",
                        CoursePrice = x.Course != null ? x.Course.Price : 0,
                        CreatedDate = x.CreatedDate,
                        IsActive = x.IsActive,
                        IsApproved = x.IsApproved,
						PaymentStatus = x.PaymentStatus
					})
                    .ToListAsync();

                return paymentViewModels;
            }
            catch (Exception ex)
            {
                throw; // Rethrow the original exception without losing the stack trace
            }
        }



		/// <summary>
		///  In this method am trying to add a method for getting  all PENDING Payments from  the DB
		/// </summary>
		/// <returns></returns>
		public List<PaymentViewModel> GetPendingPaymentList()
		{
			List<PaymentViewModel> paymentViewModels = new List<PaymentViewModel>();
			var pendingPayments = db.PaymentModels.Where(x => x.IsActive && x.PaymentStatus == PaymentStatus.Pending).Include(x => x.Course).Include(x => x.Student).ToList();

            paymentViewModels = pendingPayments.Select(x => new PaymentViewModel

			{
				Id = x.Id,
				CourseName = x.Course?.Name,
				CoursePrice = x.Course?.Price,
				CreatedDate = x.CreatedDate,
				IsActive = x.IsActive,
				IsApproved = x.IsApproved,
				StudentName = x.Student?.FullName,
                Imageurl = x.Image != null ? $"data:image/jpg;base64,{x.Image}" : null,

			}).ToList();

			return paymentViewModels;
		}




		public bool SavePayment(PaymentViewModel pay)
		{
			var payment = db.PaymentModels.FirstOrDefault(p => p.CourseId == pay.CourseId);
			if (payment != null)
			{
				payment.PaymentMethod = pay.PaymentMethod;
				payment.BankPaidFrom = pay.BankPaidFrom;
				db.PaymentModels.Update(payment);
				db.SaveChanges();
				return true;
			}
			return false;
		}
		public List<PaymentViewModel> GetPaymentHistory()
		{
			var paymentViewModels = new List<PaymentViewModel>();
			paymentViewModels = db.PaymentModels.Where(x => x.Id > 0 && x.IsActive && x.PaymentStatus == PaymentStatus.Approve || x.PaymentStatus == PaymentStatus.Decline).Include(x => x.Course).Include(x => x.Student)
                .Select(x => new PaymentViewModel
                {
                    Id = x.Id,
                    CourseName = x.Course.Name,
                    CoursePrice = x.Course.Price,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                    IsApproved = x.IsApproved,
                    StudentName = x.Student.FullName,
					PaymentStatus = x.PaymentStatus,
                }).ToList();
            return paymentViewModels;
        }

        public bool AddAnnouncements(AnnouncementViewModel announcenent, ApplicationUser loggedInUser)
        {
            if (announcenent != null)
            {
                var addAnnouncement = new Announcement()
                {
                    Title = announcenent?.Title,
                    Description = announcenent?.Description,
                    DurationFrom = announcenent.DurationFrom,
                    DurationTill = announcenent.DurationTill,
                    DateCreated = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    UserId = loggedInUser.Id,
                };
                db.Add(addAnnouncement);
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public List<AnnouncementViewModel> ListofAnnouncementForAdmin()
        {
            var announcementViewModel = new List<AnnouncementViewModel>();
            announcementViewModel = db.Announcements.Where(a => a.Id > 0 && a.IsActive == true && !a.IsDeleted)
            .Select(a => new AnnouncementViewModel()
            {
                Title = a.Title,
                Description = a.Description,
                DurationFrom = a.DurationFrom,
                DurationTill = a.DurationTill,
                DateCreated = a.DateCreated,
                Id = a.Id,
            }).ToList();

            return announcementViewModel;
        }

        public AnnouncementViewModel GetAnnouncement(int id)
        {
            var announceViewModel = new AnnouncementViewModel();
			announceViewModel = db.Announcements.Where(a => a.Id == id && a.IsActive && !a.IsDeleted).Include(a => a.User)
        .Select(a => new AnnouncementViewModel()
        {
            Title = a.Title,
			UserId = a.UserId,
            Description = a.Description,
            DurationFrom = a.DurationFrom,
            DurationTill = a.DurationTill,
            DateCreated = a.DateCreated,
            Id = a.Id,
        }).FirstOrDefault();

            return announceViewModel;
        }

        public bool EditAnnouncement(AnnouncementViewModel announcementdetails, ApplicationUser loggedInUser)
        {
            if (announcementdetails != null)
            {
                var editAnnoucement = db.Announcements.Where(x => x.Id == announcementdetails.Id && x.IsActive && !x.IsDeleted).FirstOrDefault();
                if (editAnnoucement != null)
                {
                    editAnnoucement.Title = announcementdetails.Title;
                    editAnnoucement.Description = announcementdetails.Description;
                    editAnnoucement.DurationFrom = announcementdetails.DurationFrom;
                    editAnnoucement.DurationTill = announcementdetails.DurationTill;
                    editAnnoucement.UserId = loggedInUser.Id;

                    db.Update(editAnnoucement);
                    db.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeleteAnnounce(int id)
        {
            var announceToDelete = db.Announcements.Where(a => a.Id == id && !a.IsDeleted).FirstOrDefault();
            if (announceToDelete != null)
            {
                announceToDelete.IsDeleted = true;
                announceToDelete.IsActive = false;
                db.Update(announceToDelete);
                db.SaveChanges();
                return true;
            }
            return false;
        }
        public List<AnnouncementViewModel> ListofAnnouncement()
        {
            var announcementViewModel = new List<AnnouncementViewModel>();
            announcementViewModel = db.Announcements.Where(a => a.Id > 0 && a.IsActive && !a.IsDeleted 
			&& DateTime.Now.Date <= a.DurationTill.Date && DateTime.Now.Date >= a.DurationFrom.Date)
            .Select(a => new AnnouncementViewModel()
            {
                Title = a.Title,
                Description = a.Description,
                DurationFrom = a.DurationFrom,
                DurationTill = a.DurationTill,
                DateCreated = a.DateCreated,
                Id = a.Id,
            }).ToList();

            return announcementViewModel;
        }
    }
}
