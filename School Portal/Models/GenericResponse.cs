namespace School_Portal.Models
{
   
    public class GenericResponse
    {
        public string? Message { get; set; } = string.Empty;
        public bool IsError { get; set; }
    }

}
