using System.ComponentModel.DataAnnotations;

namespace School_Portal.Models
{
	public class BaseModel
	{
		[Key]
		public int Id { get; set; } 
		public string? Name { get; set; } = string.Empty;
		public DateTime CreatedDate { get; set; } = DateTime.MinValue;
		public bool IsActive { get; set; } = false;
	}
}
