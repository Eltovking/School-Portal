using System.ComponentModel.DataAnnotations.Schema;
using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.Models
{

	public class CourseViewModel : BaseModel
	{
		public IFormFile? Image { get; set; }
		public int Price { get; set; }
		[ForeignKey(nameof(CategoryId))]
		public int? CategoryId { get; set; }
		public IFormFile? LessonNote { get; set; }
		public virtual CourseCategory? CourseCategory { get; set; }
        public string? Description { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public string? TeacherId { get; set; }
        public virtual ApplicationUser? Teacher { get; set; }

    }

    public class Course : BaseModel
    {
        public string? Image { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public CourseStatus? Status { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual CourseCategory? CourseCategory { get; set; }
        public string? TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual ApplicationUser? Teacher { get; set; }
    }
}
