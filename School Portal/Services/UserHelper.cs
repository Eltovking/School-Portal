using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using School_Portal.Data;
using School_Portal.Iservices;
using School_Portal.Models;
using School_Portal.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
			var defaultVaule = new CourseCategory
			{
				Id = 0,
				Name = "Select Category"
			};
            courses = db.CourseCategories.Where(s => s.Id > 0 && s.IsActive == true).ToList();
			courses.Insert(0, defaultVaule);
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

            courses = db.ApplicationUser.Where(s => s.Id != null && s.RoleName == "Teacher").ToList();
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
					course.Status = CourseStatus.Approve;
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
					course.Status = CourseStatus.Remove;
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
				&& x.CategoryId != null && x.Status == CourseStatus.Approve)
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

    }
}
