using System.ComponentModel.DataAnnotations;

namespace School_Portal.Models
{
    public class CourseMaterialModel
    {

        [Key]
        public int CourseId { get; set; }
        public string? CourseMaterialName { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Image { get; set; }
    }
}
