using System.ComponentModel.DataAnnotations;

namespace School_Portal.Models
{
	public class BaseModel
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }
		public DateTime CreatedDate { get; set; }
		public bool IsActive { get; set; }
	}
}
