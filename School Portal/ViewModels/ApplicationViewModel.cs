using System.ComponentModel.DataAnnotations;

namespace School_Portal.ViewModels
{
	public class ApplicationViewModel
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Password { get; set; }
		public string? PhoneNumber { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
		public string? UserName { get; set; }
	}
}
