namespace CuraCloud.DTOS
{
    public class CreateTenantDtos
    {
        // clinic name , doctor name , email
        public string clinicName { get; set; }
        public string doctorName { get; set; }
        public string email { get; set; }
        public DateTime DataOfBirth { get; set; }
    }
}
