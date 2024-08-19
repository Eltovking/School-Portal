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
        Task<ApplicationUser> CreateStudentDetails(ApplicationViewModel userModel);
        List<ApplicationUser> GetTeacher();
        List<ApplicationUser> GetTeacherName();
        ApplicationUser GetTeacherById(string userId);
        Task<List<ApplicationUser>> GetTeacherNamesAsync();
        List<ApplicationUser> GetTeacher2();
		ApplicationUser? GetUserByUserName(string? userName);
		bool UpdateProfile(ApplicationUser applicationUserFromForm, string userName);
		Task<ApplicationUser> CreateAdminDetails(ApplicationViewModel userModel);
        Course GetCourse(int courseId);
        GenericResponse IntiateCoursePayment(PaymentViewModel paymentViewModel, string username, string base64);
        Task<List<PaymentViewModel>> GetLoggedInUserPaymentList(string userId);
        public List<PaymentViewModel> GetPendingPaymentList();
        bool SavePayment(PaymentViewModel pay);
        public List<Course> GetCoursesDropDown();
        public List<PaymentViewModel> GetPaymentHistory();
        bool AddAnnouncements(AnnouncementViewModel announcenent, ApplicationUser loggedInUser);
        public List<AnnouncementViewModel> ListofAnnouncementForAdmin();
        public AnnouncementViewModel GetAnnouncement(int id);
        bool EditAnnouncement(AnnouncementViewModel announcementdetails, ApplicationUser loggedInUser);
        bool DeleteAnnounce(int id);
        public List<AnnouncementViewModel> ListofAnnouncement();
		
	}
}
