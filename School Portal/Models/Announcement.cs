using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace School_Portal.Models
{
    public class Announcement
    {
            [Key]
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Description { get; set; }
            public DateTime DurationFrom { get; set; }
            public DateTime DurationTill { get; set; }
            public DateTime DateCreated { get; set; }
            public bool IsActive { get; set; }
            public bool IsDeleted { get; set; }
            public string? UserId { get; set; }
            [ForeignKey("UserId")]
            public virtual ApplicationUser? User { get; set; }
        
    }
}
