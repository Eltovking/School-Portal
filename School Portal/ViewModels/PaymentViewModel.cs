using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.ViewModels
{
    public class PaymentViewModel
    {
        public int CourseId { get; set; }
        public string? StudentName { get; set; } = string.Empty;
        public bool? IsApproved { get; set; } = false;
        public bool IsDeclined { get; set; }
        public string? CourseName { get; set; } = string.Empty;
        public int? CoursePrice { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public string? BankPaidFrom { get; set; } = string.Empty;
        public string? PaymentMethod { get; set; } = string.Empty;              
    }
   
}
