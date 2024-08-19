using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.Models
{
    public class PaymentModel : BaseModel
    {
        public bool? IsApproved { get; set; } = false;

        public PaymentStatus? PaymentStatus { get; set; }

        public string? BankPaidFrom { get; set; } = string.Empty;

        public DateTime? StatusChangeDate { get; set; } = DateTime.MinValue;

        public string? PaymentMethod { get; set; } = string.Empty;
     

        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course? Course { get; set; }

        public string? StudentId { get; set; } = string.Empty;
        [ForeignKey("StudentId")]
        public virtual ApplicationUser? Student { get; set; }
        public string? Image { get; set; }
    }

   
}
