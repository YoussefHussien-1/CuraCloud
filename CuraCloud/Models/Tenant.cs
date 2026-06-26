namespace CuraCloud.API.Models
{
    public class Tenant
    {
        public int Id { get; set; }
        public string ClinicName { get; set; }
        public string DoctorName { get; set; }
        public string Email { get; set; }
        public DateTime dateOfBirth { get; set; }

        public string? StripeSubscriptionId { get; set; } 
        public bool IsActive { get; set; } = true; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}