using School_Portal.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace School_Portal.ViewModels
{
	public class CourseMaterialViewModel
	{
		public int CourseId { get; set; }
		[ForeignKey(nameof(CourseId))]
		public virtual Course? Course { get; set; }
		public string? CourseMaterialName { get; set; }
		public bool IsActive { get; set; }
		public DateTime DateCreated { get; set; }
		public string? Image { get; set; }
		
	}
}
