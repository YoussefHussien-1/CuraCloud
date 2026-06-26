using CuraCloud.API.Models;

namespace CuraCloud.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public int TenantId { get; set; }
        public API.Models.Tenant Tenant { get; set; }

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
