using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace CuraCloud.DTOS
{
    public record class PatientDto
    {
        public int id {  get; set; }
        public string FullName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
