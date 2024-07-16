using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
//using static School_Portal.Data.SchoolPortalEnums;

namespace School_Portal.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? FirstName { get; set; }  
        public string? LastName { get; set; }  
        public string? Address { get; set; }    
        public DateTime? DateRegistered { get; set; }    
        public bool IsDeactiveted { get; set; }
        public string? RoleName { get; set; }
        [NotMapped]
        public string? Password { get; set; }
        [NotMapped]
        public string? Role { get; set; }
        [NotMapped]
        public string? FullName => FirstName + " " + LastName;
        // public UserStatus UserStatus { get; set; }
    }
}
