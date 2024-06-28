namespace School_Portal.Models
{
	public class Event : BaseModel
	{
		
		public string? image {  get; set; }
		public TimeSpan Time {  get; set; }
		public string? Location {  get; set; }
		public string? Description {  get; set; }
		
	}
}
