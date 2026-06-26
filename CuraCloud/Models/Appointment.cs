using System;
using CuraCloud.Models;

namespace CuraCloud.API.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; } 
        public string Status { get; set; } = "Pending"; 
        public decimal Fees { get; set; } 
        public bool IsPaid { get; set; } = false; 

        
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}