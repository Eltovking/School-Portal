using School_Portal.Models;
using School_Portal.ViewModels;

namespace School_Portal.Iservices
{
    public interface IUserHelper
    {
        List<CourseCategory> GetCourseCategories();
        ApplicationUser? GetUserById(string userId);
        bool ApproveCourse(int courseId);
        bool RemoveCourse(int courseId);
        HomeViewModel GetHomeData();
        ApplicationUser? GetUserByEmail(string email);
        List<Course> GetCourses();
        List<ApplicationUser> GetUsers();
        List<CourseCategory> GetCourseCategory();
        Task<ApplicationUser>? FindByEmailAsync(string email);
        Task<ApplicationUser> CreateTeacherDetails(ApplicationViewModel userModel);
        Task<ApplicationUser> CreateAdminDetails(ApplicationViewModel userModel);
        List<ApplicationUser> GetTeacher();
        List<ApplicationUser> GetTeacherName();
        ApplicationUser GetTeacherById(string userId);
        Task<List<ApplicationUser>> GetTeacherNamesAsync();
        List<ApplicationUser> GetTeacher2();
    }
}
