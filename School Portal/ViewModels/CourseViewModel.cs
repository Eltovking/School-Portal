using School_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Portal.ViewModels
{
  
    public class CourseViewModel : BaseModel
    {
        public IFormFile? Image { get; set; }

        public int Price { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public int? CategoryId { get; set; }

        public IFormFile? CourseMaterials { get; set; }

        public virtual CourseCategory? CourseCategory { get; set; }

        public string? Description { get; set; }

        [ForeignKey(nameof(TeacherId))]
        public string? TeacherId { get; set; }

        public virtual ApplicationUser? Teacher { get; set; }

    }

}
