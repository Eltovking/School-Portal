using System.ComponentModel.DataAnnotations;

namespace School_Portal.ViewModels
{
	public class ApplicationViewModel
	{
		public string? Id { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Password { get; set; }
		public string? PhoneNumber { get; set; }
		public DateTime DateRegistered { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string? Email { get; set; }
        public string? Role { get; set; }
        public string? UserName { get; set; }
		public string? Address { get; set; }
		public string? FullName => FirstName + " " + LastName;
		public string? DOB { get; set; }
		public string? StateOfOrigin { get; set; }
	}
}
