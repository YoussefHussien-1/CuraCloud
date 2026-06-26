namespace CuraCloud.DTOS
{
    public class AppointmentDto
    {
        public int id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set;  }
        public decimal Fees { get; set; }
        public bool IsPaid { get; set; }
        public string PatientName { get; set; }
        public string ClinicName { get; set; }
    }
}
