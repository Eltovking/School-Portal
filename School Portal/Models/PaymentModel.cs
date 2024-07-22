using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Portal.Models
{
    public class PaymentModel : BaseModel
    {

        public int CourseId { get; set; }
        [ForeignKey(nameof(CourseId))]
        public virtual Course? Course { get; set; }
        public string? StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual ApplicationUser? Student { get; set; }
        public bool IsApproved { get; set; }

    }
    public class GenericResponse
    {
        public string? Message { get; set; }
        public bool IsError { get; set; }
        public object? Data { get; set; }
    }
}
