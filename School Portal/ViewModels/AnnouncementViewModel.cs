using School_Portal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace School_Portal.ViewModels
{
    public class AnnouncementViewModel
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime DurationFrom { get; set; }
        public DateTime DurationTill { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public DateTime DateCreated { get; internal set; }
    }
}
