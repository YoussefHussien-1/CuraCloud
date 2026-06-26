namespace CuraCloud.DTOS
{
    public class CreateAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public decimal Fees { get; set; }
        public int PatientId { get; set; }
        public int TenantId { get; set; }
        public string Status { get; set; } = "Pending";
        public bool IsPaid { get; set; } = false;
    }
}