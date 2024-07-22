using School_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Portal.ViewModels
{
    public class PaymentViewModel
    {
        public int CourseId { get; set; }
        
        public virtual Course? Course { get; set; }
        public string? StudentId { get; set; }
       
        public virtual ApplicationUser? Student { get; set; }
        public bool IsApproved { get; set; }
        public string? CourseName { get; set; }
        public int? CoursePrice { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }


   
}
