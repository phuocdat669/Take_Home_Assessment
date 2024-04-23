namespace PatientInfo.API.Models
{
    public class PatientDto
    {
        public string? PatientName { get; set; }
        public string? SSN { get; set; }
        public int? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Status { get; set; }
    }
}
