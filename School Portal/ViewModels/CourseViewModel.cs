using School_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.ViewModels
{
    public class CourseViewModelEdit
    {
        public int? CategoryId { get; set; }
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public int Price { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public string? Date { get; set; }
        public string? TeacherId { get; set; }
        public virtual ApplicationUser? Teacher { get; set; }
    }
}
