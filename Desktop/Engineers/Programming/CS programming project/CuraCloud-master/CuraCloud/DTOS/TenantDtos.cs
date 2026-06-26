using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Runtime.CompilerServices;

namespace CuraCloud.DTOS
{
    public class TenantDtos
    {
        // the data will be showen it will be (clinic name , doctor name )
        public int Id { get; set; }
        public string clinicName { get; set; }
        public string doctorName { get; set; }
        public string email { get; set; }
        public DateTime dateOfBirth { get; set; }
    }
}
