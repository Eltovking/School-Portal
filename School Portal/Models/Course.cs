using System.ComponentModel.DataAnnotations.Schema;
using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.Models
{
	
    public class Course : BaseModel
    {
        public string? Image { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public int Price { get; set; } = 0;

        public string? CourseMaterials { get; set; } = string.Empty;

        public CourseStatus? Status { get; set; }

        public int? CategoryId { get; set; } = 0;
        [ForeignKey("CategoryId")]
        public virtual CourseCategory? CourseCategory { get; set; }

        public string? TeacherId { get; set; } = string.Empty;
        [ForeignKey("TeacherId")]
        public virtual ApplicationUser? Teacher { get; set; }
    }
}
